using MethodConf.Cms.Domain;
using Microsoft.EntityFrameworkCore;

namespace MethodConf.Cms.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public required DbSet<SessionFeedback> SessionFeedback { get; set; }

    public required DbSet<Issue> Issues { get; set; }
}
