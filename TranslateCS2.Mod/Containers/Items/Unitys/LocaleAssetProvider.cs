using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.IO.AssetDatabase;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
[MyExcludeFromCoverage]
internal class LocaleAssetProvider : IBuiltInLocaleIdProvider {
    private readonly IAssetDatabase global;
    public LocaleAssetProvider(IAssetDatabase global) {
        this.global = global;
    }
    public LocaleAsset? Get(string localeId) {
        IEnumerable<LocaleAsset> localeAssets =
            this.GetLocaleAssets()
                .Where(item => item.localeId.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (localeAssets.Any()) {
            return localeAssets.First();
        }
        return null;
    }

    public IReadOnlyList<string> GetBuiltInLocaleIds() {
        IReadOnlyList<string> localeIds =
            this.GetLocaleAssets()
                .Select(item => item.localeId)
                .Distinct()
                .ToList();
        return localeIds;
    }

    private IEnumerable<LocaleAsset> GetLocaleAssets() {
        return
            this.global
                .GetAssets(default(SearchFilter<LocaleAsset>));
    }
}
