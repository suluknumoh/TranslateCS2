using System;
using System.Collections.Generic;
using System.Linq;

namespace TranslateCS2.Inf;
public static class IndexCountHelper {
    public delegate void OnCorruptIndexedKeyValue(IGrouping<string, KeyValuePair<string, string>> group,
                                                  KeyValuePair<string, string> groupItem);
    public static void FillIndexCountsFromLocalizationDictionary(IDictionary<string, string> localizationDictionary,
                                                                IDictionary<string, int> indexCounts,
                                                                OnCorruptIndexedKeyValue? onCorruptIndexedKeyValue) {
        // TODO: have to test it
        IEnumerable<KeyValuePair<string, string>> indexedValues =
            localizationDictionary.Where(item => item.Key.Contains(":"));

        if (!indexedValues.Any()) {
            return;
        }
        IEnumerable<IGrouping<string, KeyValuePair<string, string>>> grouped =
            indexedValues.GroupBy(item => item.Key.Split(':')[0]);

        if (!grouped.Any()) {
            return;
        }
        foreach (IGrouping<string, KeyValuePair<string, string>> group in grouped) {
            int countNew = group.Count();
            string key = group.Key;
            foreach (KeyValuePair<string, string> groupItem in group) {
                string[] splitted = groupItem.Key.Split(':');
                if (splitted.Length != 2) {
                    --countNew;
                    onCorruptIndexedKeyValue?.Invoke(group, groupItem);
                    continue;
                }
                string indexString = splitted[1];
                if (Int32.TryParse(indexString, out int index)) {
                    if (index < 0
                        || index >= countNew) {
                        --countNew;
                        onCorruptIndexedKeyValue?.Invoke(group, groupItem);
                    }
                }
            }
            if (countNew > 0) {
                if (
                //
                // is 'new' indexed value
                !indexCounts.TryGetValue(key, out int countExisting)
                //
                // localizationDictionary has more valid indexed values
                || countExisting < countNew
                //
                ) {
                    // set new indexcount for key
                    indexCounts[key] = countNew;
                    //
                    // just to clarify:
                    // if localizationDictionary has less valid indexed values
                    // it does not matter,
                    // those that are present within this translation file are used
                    // others are taken from fallback/builtin
                }
            }
        }
    }
}
