using System;
using System.Threading;

namespace Extensions.Parallelism
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Thread safe event call
        /// </summary>
        public static void SafeInvoke<T>(this EventHandler<T> evt, object sender, T arg)
        {
            var handler = Volatile.Read(ref evt);
            handler?.Invoke(sender, arg);
        }

        /// <summary>
        /// Thread safe event call in new thread
        /// </summary>
        public static void SafeInvokeAsync<T>(this EventHandler<T> evt, object sender, T arg)
        {
            var handler = Volatile.Read(ref evt);
            if (handler != null)
            {
                foreach (Delegate @delegate in handler.GetInvocationList())
                {
                    var deleg = (EventHandler<T>)@delegate;
                    deleg.BeginInvoke(sender, arg, null, null);
                }
            }
        }

        /// <summary>
        /// Thread safe event call in new thread
        /// </summary>
        public static void SafeInvokeAsync<T>(this Action<T> evt, T value)
        {
            var handler = Volatile.Read(ref evt);
            if (handler != null)
            {
                foreach (var @delegate in handler.GetInvocationList())
                {
                    var deleg = (Action<T>)@delegate;
                    deleg.BeginInvoke(value, null, null);
                }
            }
        }

        /// <summary>
        /// Thread safe event call
        /// </summary>
        public static void SafeInvoke<T>(this Action<T> evt, T value)
        {
            var handler = Volatile.Read(ref evt);
            handler?.Invoke(value);
        }

        /// <summary>
        /// Thread safe event call
        /// </summary>
        public static void SafeInvoke(this Action evt)
        {
            var handler = Volatile.Read(ref evt);
            handler?.Invoke();
        }
    }
}
