using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WarrAttachmentManagementService.Application.Interfaces;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Interceptors;

internal class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuditableEntitySaveChangesInterceptor(
        ICurrentUser currentUser,
        IDateTimeProvider dateTimeProvider)
    {
        _currentUser = currentUser;
        _dateTimeProvider = dateTimeProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData, nameof(eventData));
        ArgumentNullException.ThrowIfNull(eventData.Context, nameof(eventData.Context));

        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ArgumentNullException.ThrowIfNull(eventData, nameof(eventData));
        ArgumentNullException.ThrowIfNull(eventData.Context, nameof(eventData.Context));

        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    private void UpdateEntities(DbContext context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedByID = _currentUser.Id;
            }

            if (entry.State == EntityState.Modified
                || HasChangedOwnedEntities(entry))
            {
                entry.Property(e => e.CreatedByID).IsModified = false;
                entry.Property(e => e.CreatedDateTime).IsModified = false;

                entry.Entity.UpdatedByID = _currentUser.Id;
                entry.Entity.UpdatedDateTime = _dateTimeProvider.GetNow();
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.Property(e => e.CreatedByID).IsModified = false;
                entry.Property(e => e.CreatedDateTime).IsModified = false;

                entry.Entity.Deleted = true;
                entry.Entity.DeletionDateTime = _dateTimeProvider.GetNow();

                entry.State = EntityState.Modified;
            }
        }
    }

    private static bool HasChangedOwnedEntities(EntityEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry, nameof(entry));

        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added
                || r.TargetEntry.State == EntityState.Modified));
    }
}
