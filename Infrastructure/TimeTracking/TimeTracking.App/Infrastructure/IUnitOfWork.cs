using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuranoBot.Infrastructure.TimeTracking.App.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
