using ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;
using ModsenFinanceTracker.Domain.Configutaion;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;
using ModsenFinanceTracker.Infrastructure.Extension;
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

    public List<Wallet> Wallets { get; private set; } = [];
    public List<Category> Categories { get; private set; } = [];

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
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            var json = await File.ReadAllTextAsync(_filePath, cancellationToken);

            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            var snapshot = JsonSerializer.Deserialize<DataSnapshot>(json, _jsonOptions);

            if (snapshot == null)
            {
                return;
            }

            MapFromSnapshot(snapshot);
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
            var snapshot = MapToSnapshot();
            var json = JsonSerializer.Serialize(snapshot, _jsonOptions);

            var tempFilePath= $"{_filePath}.tmp";

            await File.WriteAllTextAsync(tempFilePath, json, cancellationToken);

            File.Move(tempFilePath, _filePath, overwrite:true);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void MapFromSnapshot(DataSnapshot snapshot)
    {
        Categories = snapshot.Categories
            .Where(c => Enum.TryParse<TransactionType>(c.TransactionType, out _))
            .Select(c => new Category(
                c.Id,
                c.Name,
                Enum.Parse<TransactionType>(c.TransactionType),
                c.BudgetLimit))
            .ToList();

        Wallets = snapshot.Wallets
            .Select(w => new Wallet(w.Id, w.Name))
            .ToList();

        var transactions = new List<Transaction>();

        var walletDict = Wallets.ToDictionary(w => w.Id);
        var categoryDict = Categories.ToDictionary(c => c.Id);

        var walletGroups = snapshot.Transactions
            .Where(t => 
                walletDict.ContainsKey(t.WalletId)
                && categoryDict.ContainsKey(t.CategoryId))
            .GroupBy(t => t.WalletId)
            .Select(g =>
                new
                {
                    Wallet = walletDict[g.Key],
                    Transactions = g
                        .Select(t =>
                        {
                            var type = Enum.Parse<TransactionType>(t.TransactionType);
                            var factory = _factoryResolver.GetFactory(type);
                            var category = categoryDict[t.CategoryId];

                            var transaction = factory.Create(
                                t.Id,
                                t.Amount,
                                category,
                                t.Description,
                                t.DateTime);

                            return transaction;
                        }).ToList()
                }
            ).ToList();

        foreach(var wallet in walletGroups)
        {
            wallet.Wallet.SetPrivateField("_transactions", wallet.Transactions);
        }
        
    }

    private DataSnapshot MapToSnapshot()
    {
        var snapshot = new DataSnapshot( 
            Wallets.Select(w => new WalletDataModel(
                w.Id,
                w.Name)).ToList(),

            Categories.Select(c => new CategoryDataModel(
                c.Id,
                c.Name,
                c.TransactionType.ToString(),
                c.BudgetLimit)).ToList(),

            Wallets.SelectMany(w => w.Transactions.Select(t => new TransactionDataModel(
                t.Id,
                t.Category.TransactionType.ToString(),
                t.Amount,
                w.Id,
                t.Category.Id,
                t.Description,
                t.DateTime))).ToList()
            );
        return snapshot;
    }
}
