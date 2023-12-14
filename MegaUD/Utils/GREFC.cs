using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaUD.Utils
{
    /// <summary>
    /// Get a random element from a collection.
    /// </summary>
    public static class GREFC<T> where T : class
    {
        public static T? GetRandomElement(IList<T>? collection)
        {
            if (collection == null) return null;
            Random rnd = new Random();
            var randomIndex = rnd.Next(collection.Count);

            return collection[randomIndex];
        }
    }
}
