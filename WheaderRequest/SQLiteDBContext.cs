using Microsoft.EntityFrameworkCore;

namespace WheaderRequest
{
    public class SQLiteDBContext : DbContext
    {
        public DbSet<AdressInfo> AdressInfo { get; set; }
        public DbSet<WsProcessInfo> WsProcessInfo { get; set; }
        public DbSet<SummaryData> SummaryData { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=wheaderrequest.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdressInfo>().ToTable("AdressInfos");

        }
    }
}
