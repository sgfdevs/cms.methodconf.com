using MethodConf.Cms.Converters;
using MethodConf.Cms.Infrastructure;
using MethodConf.Cms.Services;
using Microsoft.AspNetCore.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var searchIndexingEnabled = builder.Configuration.GetValue(
    "MethodConf:Site:SearchIndexingEnabled", true);

builder.CreateUmbracoBuilder()
    .AddBackOffice(mvc =>
    {
        mvc.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new TwoDimensionalArrayConverterFactory());
        });
    })
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

builder.Services.Configure<OpenApiOptions>("default", options =>
{
    options.AddSchemaTransformer<MultiDimensionalArraySchemaFilter>();
});

builder.AddApplicationInfrastructure();
builder.AddApplicationServices();

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true;
});

await app.BootUmbracoAsync();

if (!searchIndexingEnabled)
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
        await next();
    });
}

app.MapGet("/robots.txt", () => Results.Text(
    searchIndexingEnabled
        ? "User-agent: *\nAllow: /\n"
        : "User-agent: *\nDisallow: /\n",
    "text/plain")).AllowAnonymous();

app.MapControllerRoute("default", "/",
    new { Controller = "Home", Action = "Index" });

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
