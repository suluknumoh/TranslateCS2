using System.Collections.Generic;
using System.Linq;

namespace TranslateCS2.Inf;
public static class DictionaryHelper {
    public static IDictionary<string, string> GetNonEmpty(Dictionary<string, string> localizationDictionary) {
        return
            localizationDictionary
                .Where(item =>
                    !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Key)
                    && !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Value))
                .ToDictionary(item => item.Key, item => item.Value);
    }
}
