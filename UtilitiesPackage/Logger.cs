using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AILib;

namespace UtilitiesPackage
{
    public static class Logger<T>
    {
        private static readonly List<ILoggerItem<T>> LoggerItems = new List<ILoggerItem<T>>();

        public static bool IsEnabled { get; set; }

        public static void Log(T message)
        {
            if (IsEnabled)
            {
                foreach (var item in LoggerItems)
                {
                    item.Log(message);
                }
            }
        }

        public static void AddLoggerItem(ILoggerItem<T> item)
        {
            LoggerItems.Add(item);
        }

        public static void RemoveLoggerItem(ILoggerItem<T> item)
        {
            LoggerItems.Remove(item);
        }
    }
}
