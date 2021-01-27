using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Infrastructure
{
    public static class CollectionExtensions
    {
        public static Dictionary<string, string> ToConstantValueDictionary(this HashSet<string> keys, string name) =>
            keys.ToDictionary(k => k, _ => name);

        public static Dictionary<TKey, T> Combine<TKey, T>(this List<Dictionary<TKey, T>> dictionaries) =>
            dictionaries
            .SelectMany(dict => dict)
            .ToDictionary(i => i.Key, i => i.Value);

        public static Dictionary<TKey, T> FilterNulls<TKey, T>(this Dictionary<TKey, T> dictionary) =>
            dictionary
            .Where(p => p.Value is not null)
            .ToDictionary(i => i.Key, i => i.Value);
    }
}
