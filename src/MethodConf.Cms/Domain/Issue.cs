using System.ComponentModel.DataAnnotations;

namespace MethodConf.Cms.Domain;

public class Issue
{
    public Guid Id { get; set; }

    public Guid ConferenceId { get; set; }

    [MaxLength(10_000)]
    public required string Message { get; set; }

    [MaxLength(10_000)]
    public string? Resolution { get; set; }

    [MaxLength(255)]
    public string? Name { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(255)]
    public string? PhoneNumber { get; set; }
}
