using MethodConf.Cms.Domain;
using MethodConf.Cms.Services;
using MethodConf.Cms.Services.Interfaces;
using MethodConf.Cms.Tests.Utilities;

namespace MethodConf.Cms.Tests;

public class ScheduleGridGeneratorTests
{
    private IScheduleGridGenerator _sut;
    private DateTime _now;

    [SetUp]
    public void Setup()
    {
        _sut = new ScheduleGridGenerator();
        _now = DateTime.Now;
    }

    [Test]
    public void GenerateGrid_WithNoTracks_ReturnsValidGrid()
    {
        var schedule = new List<ScheduleItem>
        {
            new SessionItem { Key = "session1", Start = _now, End = _now.AddHours(1) },
            new SessionItem { Key = "session3", Start = _now.AddHours(3), End = _now.AddHours(4) },
            new SessionItem { Key = "session2", Start = _now.AddHours(2), End = _now.AddHours(3) },
        };

        var grid = _sut.GenerateGrid(schedule);

        // Assert.

        GridAssert.ThatGridsAreEqual(grid, new[,] {
            {"session1"},
            {"session2"},
            {"session3"},
        });
    }

    [Test]
    public void GenerateGrid_WithSimpleSchedule_ReturnsValidGrid()
    {
        var schedule = new List<ScheduleItem>
        {
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track1session1", Start = _now, End = _now.AddHours(1) },
                    new SessionItem { Key = "track1session2", Start = _now.AddHours(1), End = _now.AddHours(2) },
                ]
            },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track2session2", Start = _now.AddHours(1), End = _now.AddHours(2) },
                    new SessionItem { Key = "track2session1", Start = _now, End = _now.AddHours(1) },
                ]
            },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track3session1", Start = _now, End = _now.AddHours(1) },
                    new SessionItem { Key = "track3session2", Start = _now.AddHours(1), End = _now.AddHours(2) },
                ]
            },
        };

        var grid = _sut.GenerateGrid(schedule);

        GridAssert.ThatGridsAreEqual(grid, new[,] {
           {"track1session1", "track2session1", "track3session1"},
           {"track1session2", "track2session2", "track3session2"},
        });
    }

    [Test]
    public void GenerateGrid_WithTopLevelSession_ReturnsValidGrid()
    {
        var schedule = new List<ScheduleItem>
        {
            new SessionItem { Key = "opening", Start = _now, End = _now.AddHours(1) },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track1session1", Start = _now.AddHours(1), End = _now.AddHours(2) },
                    new SessionItem { Key = "track1session2", Start = _now.AddHours(3), End = _now.AddHours(4) },
                ]
            },
            new SessionItem { Key = "break", Start = _now.AddHours(2), End = _now.AddHours(3) },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track2session2", Start = _now.AddHours(3), End = _now.AddHours(4) },
                    new SessionItem { Key = "track2session1", Start = _now.AddHours(1), End = _now.AddHours(2) },
                ]
            }
        };

        var grid = _sut.GenerateGrid(schedule);

        GridAssert.ThatGridsAreEqual(grid, new[,] {
            {"opening", "opening"},
            {"track1session1", "track2session1"},
            {"break", "break"},
            {"track1session2", "track2session2"},
        });
    }

    [Test]
    public void GenerateGrid_WithMultiBlockSession_ReturnsValidGrid()
    {
        var schedule = new List<ScheduleItem>
        {
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track1session1", Start = _now, End = _now.AddHours(1) },
                    new SessionItem { Key = "track1session2", Start = _now.AddHours(1), End = _now.AddHours(2) },
                    new SessionItem { Key = "track1session3", Start = _now.AddHours(2), End = _now.AddHours(3) },
                ]
            },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track2session1", Start = _now, End = _now.AddHours(2) },
                    new SessionItem { Key = "track2session2", Start = _now.AddHours(2), End = _now.AddHours(3) },
                ]
            }
        };

        var grid = _sut.GenerateGrid(schedule);

        GridAssert.ThatGridsAreEqual(grid, new[,] {
            {"track1session1", "track2session1"},
            {"track1session2", "track2session1"},
            {"track1session3", "track2session2"},
        });
    }

    [Test]
    public void GenerateGrid_WithMissingSession_ReturnsValidGrid()
    {
        var schedule = new List<ScheduleItem>
        {
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track1session1", Start = _now, End = _now.AddHours(1) },
                    new SessionItem { Key = "track1session2", Start = _now.AddHours(1), End = _now.AddHours(2) },
                    new SessionItem { Key = "track1session3", Start = _now.AddHours(2), End = _now.AddHours(3) },
                    new SessionItem { Key = "track1session4", Start = _now.AddHours(3), End = _now.AddHours(4) },
                ]
            },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track2session1", Start = _now, End = _now.AddHours(2) },
                    new SessionItem { Key = "track2session2", Start = _now.AddHours(3), End = _now.AddHours(4) },
                ]
            },
            new SessionItem { Key = "closing", Start = _now.AddHours(4), End = _now.AddHours(5) },

        };

        var grid = _sut.GenerateGrid(schedule);

        GridAssert.ThatGridsAreEqual(grid, new[,] {
            {"track1session1", "track2session1"},
            {"track1session2", "track2session1"},
            {"track1session3", null},
            {"track1session4", "track2session2"},
            {"closing", "closing"},
        });
    }

    [Test]
    public void GenerateGrid_WithBreaksInSessions_ReturnsValidGrid()
    {
        var schedule = new List<ScheduleItem>
        {
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track1session1", Start = _now, End = _now.AddHours(1) },
                    new SessionItem { Key = "track1session2", Start = _now.AddHours(1.25), End = _now.AddHours(2.25) },
                    new SessionItem { Key = "track1session3", Start = _now.AddHours(2.5), End = _now.AddHours(3.5) },
                    new SessionItem { Key = "track1session4", Start = _now.AddHours(3.75), End = _now.AddHours(4.75) },
                    new SessionItem { Key = "track1session5", Start = _now.AddHours(5), End = _now.AddHours(6) },
                ]
            },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track2session1", Start = _now, End = _now.AddHours(2.25) },
                    new SessionItem { Key = "track2session2", Start = _now.AddHours(3.76), End = _now.AddHours(6) },
                ]
            },
        };

        var grid = _sut.GenerateGrid(schedule);

        GridAssert.ThatGridsAreEqual(grid, new[,] {
            {"track1session1", "track2session1"},
            {"track1session2", "track2session1"},
            {"track1session3", null},
            {"track1session4", "track2session2"},
            {"track1session5", "track2session2"},
        });
    }

    [Test]
    public void GenerateGrid_WithShorterSession_ReturnsValidGrid()
    {
        var schedule = new List<ScheduleItem>
        {
            new SessionItem { Key = "opening", Start = _now, End = _now.AddHours(0.5) },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track1session1", Start = _now.AddHours(0.75), End = _now.AddHours(1.75) },
                    new SessionItem { Key = "track1session2", Start = _now.AddHours(2), End = _now.AddHours(3) },
                    new SessionItem { Key = "track1session3", Start = _now.AddHours(3.25), End = _now.AddHours(4.25) },
                ]
            },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track2session1", Start = _now.AddHours(0.75), End = _now.AddHours(3) },
                ]
            },
            new SessionItem { Key = "closing", Start = _now.AddHours(4.5), End = _now.AddHours(5) },
        };

        var grid = _sut.GenerateGrid(schedule);

        GridAssert.ThatGridsAreEqual(grid, new[,] {
            {"opening", "opening"},
            {"track1session1", "track2session1"},
            {"track1session2", "track2session1"},
            {"track1session3", null},
            {"closing", "closing"},
        });
    }

    [Test]
    public void GenerateGrid_WithRealisticData_ReturnsValidGrid()
    {
        var schedule = new List<ScheduleItem>
        {
            new SessionItem { Key = "breakfast", Start = _now, End = _now.AddHours(0.75) },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track1session1", Start = _now.AddHours(0.75), End = _now.AddHours(1.75) },
                    new SessionItem { Key = "track1session2", Start = _now.AddHours(2), End = _now.AddHours(3) },
                    new SessionItem { Key = "track1session3", Start = _now.AddHours(5.75), End = _now.AddHours(6.75) },
                    new SessionItem { Key = "track1session4", Start = _now.AddHours(7), End = _now.AddHours(8) },
                    new SessionItem { Key = "track1session5", Start = _now.AddHours(8.25), End = _now.AddHours(9.25) },
                ]
            },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track2session1", Start = _now.AddHours(0.75), End = _now.AddHours(1.75) },
                    new SessionItem { Key = "track2session2", Start = _now.AddHours(2), End = _now.AddHours(3) },
                    new SessionItem { Key = "track2session3", Start = _now.AddHours(5.75), End = _now.AddHours(6.75) },
                    new SessionItem { Key = "track2session4", Start = _now.AddHours(7), End = _now.AddHours(8) },
                    new SessionItem { Key = "track2session5", Start = _now.AddHours(8.25), End = _now.AddHours(9.25) },
                ]
            },
            new TrackItem
            {
                Sessions =
                [
                    new SessionItem { Key = "track3session1", Start = _now.AddHours(0.75), End = _now.AddHours(3) },
                    new SessionItem { Key = "track3session2", Start = _now.AddHours(5.75), End = _now.AddHours(8) },
                ]
            },
            new SessionItem { Key = "keynote", Start = _now.AddHours(3.25), End = _now.AddHours(4.25) },
            new SessionItem { Key = "lunch", Start = _now.AddHours(4.5), End = _now.AddHours(5.5) },
            new SessionItem { Key = "closing", Start = _now.AddHours(9.5), End = _now.AddHours(10) },
        };

        var grid = _sut.GenerateGrid(schedule);

        GridAssert.ThatGridsAreEqual(grid, new[,] {
            {"breakfast", "breakfast", "breakfast"},
            {"track1session1", "track2session1", "track3session1"},
            {"track1session2", "track2session2", "track3session1"},
            {"keynote", "keynote", "keynote"},
            {"lunch", "lunch", "lunch"},
            {"track1session3", "track2session3", "track3session2"},
            {"track1session4", "track2session4", "track3session2"},
            {"track1session5", "track2session5", null},
            {"closing", "closing", "closing"},
        });
    }
}
