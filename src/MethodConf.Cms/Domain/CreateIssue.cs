namespace MethodConf.Cms.Domain;

public class CreateIssue
{
    public required string Message { get; set; }

    public string? Resolution { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}
