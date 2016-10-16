using System;
using System.Data.Entity;
using WebSiteSpeedTest.Models.Db.Entities;

namespace WebSiteSpeedTest.Models.Db
{
    public class SpeedTestContextInitializer : DropCreateDatabaseIfModelChanges<SpeedTestContext>
    {
        protected override void Seed(SpeedTestContext context)
        {
            var t = new HistoryRow
            {
                Date = DateTime.Now,
                MaxTime = new TimeSpan(0, 0, 0, 1),
                MinTime = new TimeSpan(0, 0, 0, 1),
                Url = @"http://metanit.com/sharp/entityframework/"
            };
            context.HistoryRows.Add(t);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}