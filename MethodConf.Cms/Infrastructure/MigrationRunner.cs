using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace MethodConf.Cms.Infrastructure;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class MigrationRunner : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private readonly AppDbContext _dbContext;

    public MigrationRunner(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
    {
        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();


        if (pendingMigrations.Any())
        {
            await _dbContext.Database.MigrateAsync();
        }
    }

    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class Composer : IComposer
    {
        public void Compose(IUmbracoBuilder builder) => builder.AddNotificationAsyncHandler<UmbracoApplicationStartedNotification, MigrationRunner>();
    }
}
