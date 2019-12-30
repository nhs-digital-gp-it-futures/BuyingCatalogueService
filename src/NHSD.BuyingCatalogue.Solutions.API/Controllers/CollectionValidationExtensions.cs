using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    internal static class CollectionValidationExtensions
    {
        internal static Dictionary<string, string> ToConstantValueDictionary(this HashSet<string> keys, string name) => keys.ToDictionary(k => k, v => name);

        internal static Dictionary<TKey, T> Combine<TKey, T>(this List<Dictionary<TKey, T>> dictionaries) =>
            dictionaries
            .SelectMany(dict => dict)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
