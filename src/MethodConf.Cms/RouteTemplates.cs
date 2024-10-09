namespace MethodConf.Cms;

public static class RouteTemplates
{
    public const string Base = "api/v{version:apiVersion}";
    public const string ConferenceSchedule = $"{Base}/conference/{{conferenceId:guid}}/schedule";
    public const string ConferenceIssue = $"{Base}/conference/{{conferenceId:guid}}/issue";
    public const string SessionFeedback = $"{Base}/session/{{sessionId:guid}}/feedback";
}
