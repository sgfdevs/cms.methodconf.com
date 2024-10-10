namespace MethodConf.Cms.Dtos;

public class SessionFeedbackResponseDto
{
    public Guid Id { get; set; }

    public required int SpeakerRating { get; set; }

    public required int ContentRating { get; set; }

    public required int VenueRating { get; set; }

    public string? Comments { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }
}
