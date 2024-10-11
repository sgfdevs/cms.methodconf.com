namespace MethodConf.Cms.Dtos;

public class NewIssueAppResponseViewModel
{
    public required string Message { get; set; }
    public string? Resolution { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
