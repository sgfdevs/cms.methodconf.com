namespace MethodConf.Cms.Domain;


public abstract class ScheduleItem;

public class TrackItem : ScheduleItem
{
    public required SessionItem[] Sessions { get; set; }
}

public class SessionItem : ScheduleItem
{
    public required string Key { get; set; }

    public required DateTime Start { get; set; }

    public required DateTime End { get; set; }

    public TimeSpan Duration => End - Start;
}
