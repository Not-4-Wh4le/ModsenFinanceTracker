using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Events;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionFactoryResolver _transactionFactoryResolver;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    public CreateTransactionCommandHandler(
        ICategoryRepository categoryRepository,
        IWalletRepository walletRepository,
        ITransactionFactoryResolver transactionFactoryResolver,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _categoryRepository = categoryRepository;
        _walletRepository = walletRepository;
        _transactionFactoryResolver = transactionFactoryResolver;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAsync(request.WalletId, cancellationToken);

        if (wallet == null)
        {
            return Guid.Empty;
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category == null)
        {
            return Guid.Empty;
        }

        var factory = _transactionFactoryResolver.GetFactory(request.Type);

        var transaction = factory.Create(
            Guid.NewGuid(),
            request.Amount,
            category,
            request.Description,
            request.DateTime);

        wallet.AddTransaction(transaction);
        
        await _walletRepository.SaveAsync(wallet, cancellationToken);

        await _domainEventDispatcher.DispatchAndClearAsync(wallet, cancellationToken);

        return transaction.Id;
    }
}
