using Colossal.IO.AssetDatabase;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
[MyExcludeFromCoverage]
internal class SettingsSaver : ISettingsSaver {
    private readonly AssetDatabase assetDatabase;
    public SettingsSaver(AssetDatabase assetDatabase) {
        this.assetDatabase = assetDatabase;
    }
    public void SaveSettingsNow() {
        this.assetDatabase.SaveSettingsNow();
    }
}
