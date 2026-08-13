using ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;
using ModsenFinanceTracker.Domain.Configutaion;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;
using ModsenFinanceTracker.Infrastructure.JsonStorage.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ModsenFinanceTracker.Infrastructure.JsonStorage;

public class JsonDbContext
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ITransactionFactoryResolver _factoryResolver;

    public List<Wallet> Wallets { get; private set; } = new();
    public List<Category> Categories { get; private set; } = new();
    public List<Transaction> Transactions { get; private set; } = new();

    public JsonDbContext(
        AppConfiguration configuration,
        ITransactionFactoryResolver factoryResolver)
    {
        _filePath = configuration.DataFilePath;
        _factoryResolver = factoryResolver;

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) 
        };

        LoadAsync().GetAwaiter().GetResult();
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!File.Exists(_filePath))
            {
                Wallets = new List<Wallet>();
                Categories = new List<Category>();
                Transactions = new List<Transaction>();
                return;
            }

            var json = await File.ReadAllTextAsync(_filePath, cancellationToken);
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var snapshot = JsonSerializer.Deserialize<DataSnapshot>(json, _jsonOptions);
            if (snapshot == null) return;

            Categories = snapshot.Categories.Select(c => new Category(
                c.Id,
                c.Name,
                Enum.Parse<TransactionType>(c.TransactionType),
                c.BudgetLimit
            )).ToList();

            Wallets = snapshot.Wallets.Select(w => new Wallet(
                w.Id,
                w.Name
            )).ToList();

            Transactions = new List<Transaction>();
            foreach (var tModel in snapshot.Transactions)
            {
                var category = Categories.FirstOrDefault(c => c.Id == tModel.CategoryId);
                var wallet = Wallets.FirstOrDefault(w => w.Id == tModel.WalletId);

                if (category == null || wallet == null) continue;

                var type = Enum.Parse<TransactionType>(tModel.TransactionType);

                var factory = _factoryResolver.GetFactory(type);

                var transaction = factory.Create(
                    tModel.Id,
                    tModel.Amount,
                    category,
                    tModel.Description,
                    tModel.DateTime);

                Transactions.Add(transaction);
                wallet.AddTransaction(transaction);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var snapshot = new DataSnapshot
            {
                Categories = Categories.Select(c => new CategoryDataModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    TransactionType = c.TransactionType.ToString(),
                    BudgetLimit = c.BudgetLimit
                }).ToList(),

                Wallets = Wallets.Select(w => new WalletDataModel
                {
                    Id = w.Id,
                    Name = w.Name
                }).ToList(),

                Transactions = Transactions.Select(t =>
                {
                    var wallet = Wallets.FirstOrDefault(w => w.Transactions.Any(tr => tr.Id == t.Id));

                    return new TransactionDataModel
                    {
                        Id = t.Id,
                        TransactionType = t.Category.TransactionType.ToString(),
                        Amount = t.Amount,
                        WalletId = wallet?.Id ?? Guid.Empty,
                        CategoryId = t.Category.Id,
                        Description = t.Description,
                        DateTime = t.DateTime
                    };
                }).ToList()
            };

            var json = JsonSerializer.Serialize(snapshot, _jsonOptions);

            var tempFilePath = _filePath + ".tmp";
            await File.WriteAllTextAsync(tempFilePath, json, cancellationToken);

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            File.Move(tempFilePath, _filePath);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
