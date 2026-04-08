using Microsoft.EntityFrameworkCore;

namespace MethodConf.Cms.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddUmbracoDbContext<AppDbContext>((servicesProvider, options, _, _) =>
        {
            options.UseUmbracoDatabaseProvider(servicesProvider);
        });

        return builder;
    }
}
