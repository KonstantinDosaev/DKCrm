using DKCrm.Shared.Iterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DKCrm.Server.Data
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (eventData.Context is null) return new ValueTask<InterceptionResult<int>>(result);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {

                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) continue;
                if (delete.IsFullDeleted) continue;

                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
                delete.DateTimeUpdate = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);

            }
            return new ValueTask<InterceptionResult<int>>(result);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {

                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) continue;
                if (delete.IsFullDeleted) continue;

                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
                delete.DateTimeUpdate = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);

            }
            return result;
        }


    }
}
