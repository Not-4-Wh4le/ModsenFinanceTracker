using ModsenFinanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
        Task SaveAsync(Wallet wallet, CancellationToken cancellationToken = default);
    }
}
