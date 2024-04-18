using Colossal.IO.AssetDatabase;

using Game.Modding;

namespace TranslateCS2.Mod;
[FileLocation(Mod.Name)]
public class Setting : ModSetting {
    public Setting(IMod mod) : base(mod) { }
    public override void SetDefaults() {
        //
    }
}
