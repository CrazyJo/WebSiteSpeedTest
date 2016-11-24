using System;
using System.Collections.Generic;
using System.Data.Entity;
using Core.Model;

namespace Data
{
    public class SpeedTestContextInitializer : DropCreateDatabaseIfModelChanges<SpeedTestContext>
    {
        protected override void Seed(SpeedTestContext context)
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid().ToString();

            var t = new HistoryRow
            {
                Id = id,
                Date = now,
                MaxTime = new TimeSpan(0, 0, 0, 1),
                MinTime = new TimeSpan(0, 0, 0, 1),
                Url = @"http://metanit.com/sharp/entityframework/"
            };

            var s1 = new SitemapRow
            {
                HistoryRowId = id,
                Id = 1,
                MaxTime = new TimeSpan(0, 0, 0, 4),
                MinTime = new TimeSpan(0, 0, 0, 2),
                Url = @"https://www.kinopoisk.ru/"
            };

            var s2 = new SitemapRow
            {
                HistoryRowId = id,
                Id = 2,
                MaxTime = new TimeSpan(0, 0, 0, 6),
                MinTime = new TimeSpan(0, 0, 0, 3),
                Url = @"http://coub.com/"
            };

            context.HistoryRows.Add(t);
            context.SitemapRows.AddRange(new List<SitemapRow> {s1, s2});

            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            base.Seed(context);
        }
    }
}