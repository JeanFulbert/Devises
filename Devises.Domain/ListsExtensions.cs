namespace Devises.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ListsExtensions
    {
        public static void EnqueueAll<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) =>
            !source?.Any() ?? false;
    }
}
