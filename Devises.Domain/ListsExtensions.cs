namespace Devises.Domain
{
    using System.Collections.Generic;

    public static class ListsExtensions
    {
        public static void EnqueueAll<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }
    }
}
