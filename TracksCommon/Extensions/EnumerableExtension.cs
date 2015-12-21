using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracksCommon.Extensions
{
    public static class EnumerableExtension
    {
        public static Queue<KeyValuePair<TKey, TValue>> ToQueue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dic)
        {
            var queue = new Queue<KeyValuePair<TKey, TValue>>();
            foreach (var o in dic)
            {
                queue.Enqueue(o);
            }

            return queue;
        }
    }
}
