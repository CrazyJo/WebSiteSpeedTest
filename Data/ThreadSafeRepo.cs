namespace Data
{
    public class ThreadSafeRepo<T>: Repo<T> where T : class, new()
    {
        protected readonly object LockObj = new object();

        public ThreadSafeRepo(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }

        public override void Add(T obj)
        {
            lock (LockObj)
            {
                base.Add(obj); 
            }
        }
    }
}
