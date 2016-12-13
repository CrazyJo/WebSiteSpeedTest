using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Enumerable
{
    public static class EnumerableExtensions
    {
        public static T GetMax<T>(this IEnumerable<T> collection, params T[] values) where T : IComparable<T>
        {
            T left = collection.Max();
            T right = values.Max();
            return left.CompareTo(right) > 0 ? left : right;
        }

        public static T GetMin<T>(this IEnumerable<T> collection, params T[] values) where T : IComparable<T>
        {
            T left = collection.Min();
            T right = values.Min();
            return left.CompareTo(right) < 0 ? left : right;
        }
    }
}
