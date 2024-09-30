using MethodConf.Cms.Domain;

namespace MethodConf.Cms.Services.Interfaces;

public interface IScheduleGridGenerator
{
    public string?[,] GenerateGrid(IEnumerable<ScheduleItem> scheduleItems);
}
