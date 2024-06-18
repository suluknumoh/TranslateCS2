using System.Collections.Generic;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.ZModTests.TestHelpers.Models;
internal class TestDictionarySource : IDictionarySource {
    private readonly MyLocalization<string> localization;
    private TestDictionarySource(MyLocalization<string> localization) {
        this.localization = localization;
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        DictionaryHelper.AddAll(this.localization.Source.IndexCounts, indexCounts);
        return this.localization.Source.Localizations;
    }

    public void Unload() {
        //
    }
    public static IDictionarySource FromMyLocalization(MyLocalization<string> localization) {
        return new TestDictionarySource(localization);
    }
}
