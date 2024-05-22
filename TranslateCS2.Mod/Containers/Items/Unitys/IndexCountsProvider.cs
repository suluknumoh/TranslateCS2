using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.IO.AssetDatabase;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
[MyExcludeClassFromCoverage]
internal class IndexCountsProvider : IIndexCountsProvider {
    private readonly IAssetDatabase global;
    public IndexCountsProvider(IAssetDatabase global) {
        this.global = global;
    }
    /// <summary>
    ///     never ever change content!!!
    ///     <br/>
    ///     <br/>
    ///     returns a ref to <see cref="LocaleAsset.data"/>s <see cref="LocaleData.indexCounts"/>
    /// </summary>
    private IReadOnlyDictionary<string, int>? GetIndexCounts(string localeId) {
        IEnumerable<LocaleAsset> localeAssets =
            this.global
                .GetAssets(default(SearchFilter<LocaleAsset>))
                .Where(item => item.localeId.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (localeAssets.Any()) {
            LocaleAsset asset = localeAssets.First();
            LocaleData data = asset.data;
            return data.indexCounts;
        }
        return null;
    }
    public void AddIndexCounts(Dictionary<string, int> indexCounts, string localeId) {
        /// WARNING: a ref to <see cref="LocaleAsset.data"/>s <see cref="LocaleData.indexCounts"/>
        IReadOnlyDictionary<string, int>? localIndedxCounts = this.GetIndexCounts(localeId);
        if (localIndedxCounts is null) {
            return;
        }
        foreach (KeyValuePair<string, int> entry in localIndedxCounts) {
            indexCounts.Add(entry.Key, entry.Value);
        }
    }
}
