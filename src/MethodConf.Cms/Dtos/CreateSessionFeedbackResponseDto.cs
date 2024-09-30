namespace MethodConf.Cms.Dtos;

public class SessionFeedbackResponseDto
{
    public Guid Id { get; set; }

    public required int SpeakerRating { get; set; }

    public required int ContentRating { get; set; }

    public required int VenueRating { get; set; }

    public required string Comments { get; set; }
}
