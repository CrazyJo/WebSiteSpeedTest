using System.Data.Entity;
using Core.Model;

namespace Data
{

    public class SpeedTestContext : DbContext
    {
        static SpeedTestContext()
        {
            //Database.SetInitializer(new SpeedTestContextInitializer());
        }

        public SpeedTestContext() : base("SpeedTestContext")
        {
        }

        public virtual DbSet<HistoryRow> HistoryRows { get; set; }

        public virtual DbSet<SitemapRow> SitemapRows { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<HistoryRow>().HasKey(e => e.Id);

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}