namespace MethodConf.Cms.Domain;

public class CreateSessionFeedback
{
    public required int SpeakerRating { get; set; }

    public required int ContentRating { get; set; }

    public required int VenueRating { get; set; }

    public required string Comments { get; set; }

    public string? Email { get; set; }
}
