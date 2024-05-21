using System.Collections.Generic;

using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.ZZZModTestLib.Containers.Items.Unitys;
public class TestIndexCountsProvider : IIndexCountsProvider {
    private readonly IDictionary<string, int> content;
    public TestIndexCountsProvider(IDictionary<string, int> content) {
        this.content = content;
    }

    internal static IIndexCountsProvider WithEmptyContent() {
        return new TestIndexCountsProvider(new Dictionary<string, int>());
    }

    public void AddIndexCounts(Dictionary<string, int> indexCounts, string localeId) {
        foreach (KeyValuePair<string, int> item in this.content) {
            indexCounts.Add(item.Key, item.Value);
        }
    }
}
