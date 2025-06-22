using Microsoft.EntityFrameworkCore;

namespace DesktopApp
{
    public class DataContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = DataFile.db");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
    }
}
