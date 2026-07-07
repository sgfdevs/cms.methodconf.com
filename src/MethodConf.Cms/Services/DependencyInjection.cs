using MethodConf.Cms.Mapping;
using MethodConf.Cms.Services.Interfaces;

namespace MethodConf.Cms.Services;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IScheduleGridGenerator, ScheduleGridGenerator>();
        builder.Services.AddScoped<IConferenceScheduleService, ConferenceScheduleService>();
        builder.Services.AddScoped<ISessionFeedbackService, SessionFeedbackService>();
        builder.Services.AddScoped<IConferenceIssueService, ConferenceIssueService>();
        builder.Services.AddScoped<ScheduleMapper>();
        builder.Services.AddScoped<SessionFeedbackMapper>();
        builder.Services.AddScoped<IssueMapper>();
        builder.Services.AddRazorTemplating();

        return builder;
    }
}
