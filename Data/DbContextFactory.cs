using System.Data.Entity;

namespace Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly SpeedTestContext _dbContext;

        public DbContextFactory()
        {
            _dbContext = new SpeedTestContext();
        }
        public DbContext GetContext()
        {
            return _dbContext;
        }
    }
}