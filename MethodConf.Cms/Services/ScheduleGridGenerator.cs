using MethodConf.Cms.Domain;
using MethodConf.Cms.Services.Interfaces;

namespace MethodConf.Cms.Services;

public class ScheduleGridGenerator : IScheduleGridGenerator
{
    public string?[,] GenerateGrid(IEnumerable<ScheduleItem> scheduleItems)
    {
        var schedule = scheduleItems as ScheduleItem[] ?? scheduleItems.ToArray();
        var tracks = schedule.OfType<TrackItem>().ToArray();

        if (tracks.Length == 0)
        {
            return GenerateSessionOnlyGrid(schedule);
        }

        var topLevelSessions = schedule.OfType<SessionItem>().ToArray();

        var allSessions = schedule
            .SelectMany(item =>
            {
                return item switch
                {
                    SessionItem session => [session],
                    TrackItem track => track.Sessions,
                    _ => []
                };
            })
            .OrderBy(session => session.Start)
            .ToArray();

        var firstSessionStart = allSessions.First().Start;
        var blockDuration = GetMostCommonDuration(allSessions);
        var breakDuration = GetMostCommonBreakDuration(allSessions);

        foreach (var track in tracks)
        {
            track.Sessions = track.Sessions
                .Concat(topLevelSessions)
                .OrderBy(session => session.Start)
                .ToArray();
        }

        var totalBlocks = GetMaxBlocksFromTracks(tracks, blockDuration + breakDuration);

        var grid = new string?[totalBlocks, tracks.Length];

        var blockStart = firstSessionStart;

        for (var blockIndex = 0; blockIndex < grid.GetLength(0); blockIndex++)
        {
            var blockSessions = new List<SessionItem?>();
            for (var trackIndex = 0; trackIndex < grid.GetLength(1); trackIndex++)
            {
                var track = tracks[trackIndex];
                var session = GetSessionForBlock(track.Sessions, blockStart, blockStart + blockDuration, breakDuration);
                blockSessions.Add(session);
                grid[blockIndex, trackIndex] = session?.Key;
            }

            var shortestSession = blockSessions
                .MinBy(session => session?.Duration ?? blockDuration);

            blockStart = shortestSession?.End ?? blockStart + blockDuration;
        }

        return grid;
    }

    private static string?[,] GenerateSessionOnlyGrid(ScheduleItem[] schedule)
    {
        var sessions = schedule.OfType<SessionItem>()
            .OrderBy(session => session.Start)
            .ToArray();

        var sessionOnlyGrid = new string?[sessions.Length, 1];

        for (var i = 0; i < sessionOnlyGrid.Length; i++)
        {
            sessionOnlyGrid[i, 0] = sessions[i].Key;
        }

        return sessionOnlyGrid;
    }

    private static TimeSpan GetMostCommonDuration(SessionItem[] sessions)
    {
        var durations = sessions
            .Select(session => session.Duration)
            .ToList();

        var durationGroups = durations
            .GroupBy(d => d)
            .OrderByDescending(g => g.Count());

        var mostCommonDuration = durationGroups.First().Key;

        return mostCommonDuration;
    }

    public static TimeSpan GetMostCommonBreakDuration(SessionItem[] sortedSessions)
    {
        var breakDurations = new List<TimeSpan>();

        for (var i = 1; i < sortedSessions.Length; i++)
        {
            var breakDuration = sortedSessions[i].Start - sortedSessions[i - 1].End;

            if (breakDuration > TimeSpan.Zero)
            {
                breakDurations.Add(breakDuration);
            }
        }

        var breakDurationGroups = breakDurations
            .GroupBy(bd => bd)
            .OrderByDescending(g => g.Count())
            .ToArray();

        return !breakDurationGroups.Any() ? TimeSpan.Zero : breakDurationGroups.First().Key;
    }

    private int GetMaxBlocksFromTracks(TrackItem[] tracks, TimeSpan blockWithBreakDuration)
    {
        var trackBlocks = new List<int>();

        foreach (var track in tracks)
        {
            var totalBlocks = 0;

            foreach (var session in track.Sessions)
            {
                var blockCount = Math.Round(session.Duration.TotalSeconds / blockWithBreakDuration.TotalSeconds, 2);

                var blocksForThisSession = (int)Math.Ceiling(blockCount);

                if (blocksForThisSession < 1)
                {
                    blocksForThisSession = 1;
                }

                totalBlocks += blocksForThisSession;
            }

            trackBlocks.Add(totalBlocks);
        }

        return trackBlocks.Max();
    }

    private SessionItem? GetSessionForBlock(SessionItem[] trackSessions, DateTime blockStart, DateTime blockEnd, TimeSpan tolerance)
    {
        foreach (var session in trackSessions)
        {
            if (Math.Abs((session.Start - blockStart).TotalSeconds) <= tolerance.TotalSeconds)
            {
                return session;
            }
        }

        foreach (var session in trackSessions)
        {
            if (session.Start < blockEnd && session.End > blockStart)
            {
                return session;
            }
        }

        // If no matching session is found, return null
        return null;
    }
}
