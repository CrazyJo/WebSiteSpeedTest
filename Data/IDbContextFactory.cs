using System.Data.Entity;

namespace Data
{
    public interface IDbContextFactory
    {
        DbContext GetContext();
    }
}
