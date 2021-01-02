using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSleigh.Core.Persistence
{
    public interface ISagaStateRepository
    {
        Task<(TD state, Guid lockId)> LockAsync<TD>(Guid correlationId, TD newState = null, CancellationToken cancellationToken = default) where TD : SagaState;
        Task UpdateAsync<TD>(TD state, Guid lockId, bool releaseLock = false, CancellationToken cancellationToken = default) where TD : SagaState;
    }
}