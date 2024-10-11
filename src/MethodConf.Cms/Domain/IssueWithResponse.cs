namespace MethodConf.Cms.Domain;

public class IssueWithResponse
{
    public Guid Id { get; set; }

    public Guid ConferenceId { get; set; }

    public required string Message { get; set; }

    public string? Resolution { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public required string ResponseMarkup { get; set; }
}
