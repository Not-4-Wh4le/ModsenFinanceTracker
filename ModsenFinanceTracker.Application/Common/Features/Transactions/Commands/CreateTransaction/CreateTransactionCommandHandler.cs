using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionFactoryResolver _transactionFactoryResolver;
    public CreateTransactionCommandHandler(
        ICategoryRepository categoryRepository,
        IWalletRepository walletRepository,
        ITransactionFactoryResolver transactionFactoryResolver)
    {
        _categoryRepository = categoryRepository;
        _walletRepository = walletRepository;
        _transactionFactoryResolver = transactionFactoryResolver;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAsync(cancellationToken)
            ?? throw new InvalidOperationException("Wallet not found");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new InvalidOperationException("Category not found");

        var factory = _transactionFactoryResolver.GetFactory(request.Type);

        var transaction = factory.Create(
            Guid.NewGuid(),
            request.Amount,
            category,
            request.Description,
            request.DateTime);

        wallet.AddTransaction(transaction);
        await _walletRepository.SaveAsync(wallet, cancellationToken);

        return transaction.Id;
    }
}
