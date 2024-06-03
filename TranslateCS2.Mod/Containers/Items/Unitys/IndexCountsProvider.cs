using System.Collections.Generic;

using Colossal.IO.AssetDatabase;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
[MyExcludeFromCoverage]
internal class IndexCountsProvider : IIndexCountsProvider {
    private readonly LocaleAssetProvider localeAssetProvider;
    public IndexCountsProvider(LocaleAssetProvider localeAssetProvider) {
        this.localeAssetProvider = localeAssetProvider;
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
    /// <summary>
    ///     never ever change content!!!
    ///     <br/>
    ///     <br/>
    ///     returns a ref to <see cref="LocaleAsset.data"/>s <see cref="LocaleData.indexCounts"/>
    /// </summary>
    private IReadOnlyDictionary<string, int>? GetIndexCounts(string localeId) {
        LocaleAsset? asset = this.localeAssetProvider.Get(localeId);
        if (asset is null) {
            return null;
        }
        LocaleData data = asset.data;
        return data.indexCounts;
    }
}
