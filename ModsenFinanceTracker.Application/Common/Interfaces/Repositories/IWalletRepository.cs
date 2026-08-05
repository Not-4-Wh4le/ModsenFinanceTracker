using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
        Task SaveAsync(Wallet wallet, CancellationToken cancellationToken = default);
    }
}
