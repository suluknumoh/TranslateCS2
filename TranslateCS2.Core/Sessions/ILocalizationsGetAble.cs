using System;
using System.Collections.Generic;
using System.Linq;

using TranslateCS2.Inf;

namespace TranslateCS2.Core.Sessions;
public interface ILocalizationsGetAble<T> where T : ICollection<ILocalizationEntry> {
    T Localizations { get; }
    IDictionary<string, string> GetLocalizationsAsDictionary(bool addMergeValues) {
        Func<ILocalizationEntry, bool> predicate = item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation);
        Func<ILocalizationEntry, string> keySelector = (item) => item.Key;
        Func<ILocalizationEntry, string> valueSelector = (item) => item.Translation;
        if (addMergeValues) {
            predicate = item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation) || !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.ValueMerge);

            valueSelector = (item) => {
                if (StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation)) {
                    return item.ValueMerge;
                };
                return item.Translation;
            };
        }
        return this.Localizations
                .Where(predicate)
                .ToDictionary(keySelector, valueSelector);
    }
}
