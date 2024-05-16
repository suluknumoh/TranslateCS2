using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TranslateCS2.Inf;
public static class IndexCountHelper {
    public static Regex IndexMatcher { get; } = new Regex("\\:\\d+$");

    public static void FillIndexCountsFromLocalizationDictionary(IDictionary<string, string> localizationDictionary,
                                                                 IDictionary<string, int> indexCounts,
                                                                 IList<KeyValuePair<string, int>> errors) {
        // only those, that end with a colon and at least one digit
        IEnumerable<KeyValuePair<string, string>> indexedValues =
            localizationDictionary.Where(item => IndexMatcher.IsMatch(item.Key));

        if (!indexedValues.Any()) {
            return;
        }
        IEnumerable<IGrouping<string, KeyValuePair<string, string>>> groupedForIndexCountKeys =
            indexedValues
                .GroupBy(item => IndexMatcher.Replace(item.Key, String.Empty));
        if (!groupedForIndexCountKeys.Any()) {
            return;
        }
        foreach (IGrouping<string, KeyValuePair<string, string>> indexCountGroupItem in groupedForIndexCountKeys) {
            bool exists = indexCounts.TryGetValue(indexCountGroupItem.Key, out int existingCount);



            // one group per KeyValuePair
            IEnumerable<IGrouping<(string IndexCountKey, int Index), KeyValuePair<string, string>>> groupForOrder =
                indexCountGroupItem.GroupBy(SelectKey);

            // order by
            IOrderedEnumerable<IGrouping<(string IndexCountKey, int Index), KeyValuePair<string, string>>> orderedIndexValues =
                groupForOrder.OrderBy(item => item.Key.Index);


            int newIndexCount = existingCount;
            foreach (IGrouping<(string IndexCountKey, int Index), KeyValuePair<string, string>> indexedValue in orderedIndexValues) {
                if (indexedValue.Key.Index < existingCount) {
                    continue;
                }
                // no need to iterate over indexedValue - its one group per KeyValuePair
                if (indexedValue.Key.Index == newIndexCount) {
                    newIndexCount++;
                    continue;
                }
                errors.Add(new KeyValuePair<string, int>(indexCountGroupItem.Key, newIndexCount));
                break;
            }
            if (newIndexCount > 0) {
                indexCounts[indexCountGroupItem.Key] = newIndexCount;
            }
        }
    }

    private static (string IndexCountKey, int Index) SelectKey(KeyValuePair<string, string> pair) {
        string indexCountString = IndexMatcher.Replace(pair.Key, String.Empty);
        string indexString =
                    pair.Key
                        .Replace(indexCountString, String.Empty)
                        .Substring(1);
        int index = Int32.Parse(indexString);
        return (indexCountString, index);
    }
}
