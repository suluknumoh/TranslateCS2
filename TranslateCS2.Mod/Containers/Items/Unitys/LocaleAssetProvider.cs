using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.IO.AssetDatabase;

using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
[MyExcludeFromCoverage]
internal class LocaleAssetProvider {
    private readonly IAssetDatabase global;
    public LocaleAssetProvider(IAssetDatabase global) {
        this.global = global;
    }
    public LocaleAsset? Get(string localeId) {
        IEnumerable<LocaleAsset> localeAssets =
            this.global
                .GetAssets(default(SearchFilter<LocaleAsset>))
                .Where(item => item.localeId.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (localeAssets.Any()) {
            return localeAssets.First();
        }
        return null;
    }
}
