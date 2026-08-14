using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<(IReadOnlyCollection<Wallet>, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task SaveAsync(Wallet wallet, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
