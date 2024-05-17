using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using TranslateCS2.Inf.Keyz;
using TranslateCS2.Inf.Models;

namespace TranslateCS2.Inf;
public static class IndexCountHelper {
    public static Regex IndexMatcher { get; } = new Regex("\\:\\d+$");
    public static void FillIndexCountsAndAutocorrect(IDictionary<string, string> localizationDictionary,
                                                                 IDictionary<string, int> indexCounts) {
        // only those, that end with a colon and at least one digit
        IEnumerable<KeyValuePair<string, string>> indexedValues =
            localizationDictionary.Where(item => IndexMatcher.IsMatch(item.Key));

        if (!indexedValues.Any()) {
            return;
        }
        IEnumerable<IGrouping<string, KeyValuePair<string, string>>> groupedForIndexCountKeys =
            indexedValues
                .GroupBy(item => GetCountKeyFromKey(item.Key));
        if (!groupedForIndexCountKeys.Any()) {
            return;
        }
        foreach (IGrouping<string, KeyValuePair<string, string>> indexCountGroupItem in groupedForIndexCountKeys) {
            bool exists = indexCounts.TryGetValue(indexCountGroupItem.Key, out int existingCount);



            // one group per KeyValuePair
            IEnumerable<IGrouping<IMyKey, KeyValuePair<string, string>>> groupForOrder =
                indexCountGroupItem.GroupBy(item => new MyKey(item.Key));

            // order by
            IOrderedEnumerable<IGrouping<IMyKey, KeyValuePair<string, string>>> orderedIndexValues =
                groupForOrder.OrderBy(item => item.Key.Index);


            int newIndexCount = existingCount;
            foreach (IGrouping<IMyKey, KeyValuePair<string, string>> indexedValue in orderedIndexValues) {
                if (indexedValue.Key.Index < existingCount) {
                    continue;
                }
                // no need to iterate over indexedValue - its one group per KeyValuePair
                if (indexedValue.Key.Index == newIndexCount) {
                    newIndexCount++;
                    continue;
                }
                KeyValuePair<string, string> originalPair = indexedValue.ElementAt(0);
                string newKey = BuildNewKey(indexCountGroupItem.Key, newIndexCount);
                KeyValuePair<string, string> newPair = new KeyValuePair<string, string>(newKey, originalPair.Value);
                localizationDictionary.Remove(originalPair);
                localizationDictionary.Add(newPair);
                newIndexCount++;
            }
            if (newIndexCount > 0) {
                indexCounts[indexCountGroupItem.Key] = newIndexCount;
            }
        }
    }

    internal static string BuildNewKey(string key, int newIndex) {
        return $"{key}:{newIndex:D0}";
    }

    public static int GetIndexFromKey(string key) {
        string indexCountKey = GetCountKeyFromKey(key);
        string indexString =
                    key
                        .Replace(indexCountKey, String.Empty);
        if (IndexMatcher.IsMatch(key)) {
            indexString = indexString.Substring(1);
        }
        bool parsed = Int32.TryParse(indexString, out int index);
        if (parsed) {
            return index;
        }
        return 0;
    }

    public static string GetCountKeyFromKey(string key) {
        return IndexMatcher.Replace(key, String.Empty);
    }

    public static IndexCountHelperValidationResult ValidateForKey<T>(ICollection<T> localizationDictionary, string key) where T : IMyKey {
        IMyKey newKey = new MyKey(key);
        IOrderedEnumerable<T> ordered =
            localizationDictionary
                .Where(item => item.CountKey == newKey.CountKey)
                .OrderBy(item => item.Index);
        if (ordered.Any()) {
            IMyKey last = ordered.Last();
            int nextFreeIndex = last.Index + 1;
            if (newKey.Index > nextFreeIndex) {
                return IndexCountHelperValidationResult.InValid(key, nextFreeIndex);
            }
        } else if (newKey.Index > 0) {
            return IndexCountHelperValidationResult.InValid(key, 0);
        }
        return IndexCountHelperValidationResult.Valid();
    }

    public static void AutoCorrect<T>(IList<T> localizationDictionary) where T : IMyKey {
        IEnumerable<T> indexed =
            localizationDictionary
                .Where(item => item.IsIndexed);
        IEnumerable<IGrouping<string, T>> grouped = indexed.GroupBy(item => item.CountKey);
        foreach (IGrouping<string, T> group in grouped) {
            int newIndex = 0;
            IOrderedEnumerable<T> ordered = group.OrderBy(item => item.Index);
            foreach (T item in ordered) {
                int itemIndex = localizationDictionary.IndexOf(item);
                string newKey = BuildNewKey(item.CountKey, newIndex);
                localizationDictionary[itemIndex].Key = newKey;
                newIndex++;
            }
        }
    }
}
