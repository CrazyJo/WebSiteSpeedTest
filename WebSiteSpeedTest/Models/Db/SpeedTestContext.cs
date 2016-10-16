using System;
using System.Data.Entity;
using System.Linq;
using WebSiteSpeedTest.Models.Db.Entities;

namespace WebSiteSpeedTest.Models.Db
{

    public class SpeedTestContext : DbContext
    {
        static SpeedTestContext()
        {
            Database.SetInitializer(new SpeedTestContextInitializer());
        }

        public SpeedTestContext() : base("SpeedTestContext")
        {
        }

        public virtual DbSet<HistoryRow> HistoryRows { get; set; }

        public virtual DbSet<SitemapRow> SitemapRows { get; set; }
    }

}