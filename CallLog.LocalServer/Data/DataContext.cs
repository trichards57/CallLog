using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CallLog.LocalServer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Controller> Controllers { get; set; }
        public DbSet<LoggedEvent> Events { get; set; }
        public DbSet<LogLine> LogLines { get; set; }
    }
}
