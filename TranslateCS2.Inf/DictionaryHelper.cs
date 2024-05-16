using System.Collections.Generic;
using System.Linq;

namespace TranslateCS2.Inf;
public static class DictionaryHelper {
    public static IDictionary<string, string> GetNonEmpty(IDictionary<string, string> localizationDictionary) {
        return
            localizationDictionary
                .Where(item =>
                    !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Key)
                    && !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Value))
                .ToDictionary(item => item.Key, item => item.Value);
    }
    public static void AddAll<K, V>(IDictionary<K, V> source, IDictionary<K, V> destination) {
        foreach (KeyValuePair<K, V> entry in source) {
            destination[entry.Key] = entry.Value;
        }
    }
}
