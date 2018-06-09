using System;
using System.Collections.Generic;
using System.Linq;

namespace Marcus.Common
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var elem in list)
                action(elem);
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T, int> action)
        {
            var i = 0;
            foreach (var elem in list)
            {
                action(elem, i);
                i++;
            }
        }

        public static void Remove<T>(this IList<T> list, Func<T, bool> predicat)
        {
            var toBeRemoved = new List<T>();
            list.Where(predicat).ForEach(x => toBeRemoved.Add(x));

            toBeRemoved.ForEach(x => list.Remove(x));
        }
    }
}