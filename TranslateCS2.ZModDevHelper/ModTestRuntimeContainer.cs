using Colossal.Localization;
using Colossal.Logging;

using Game.Settings;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Helpers;

namespace TranslateCS2.ZModDevHelper;
internal class ModTestRuntimeContainer : IModRuntimeContainer {
    public ILog? Logger { get; }
    public LocalizationManager? LocManager { get; }
    public InterfaceSettings? IntSetting { get; }
    public string UserDataPath => PathHelper.UserDataPath;
    public string StreamingDataPath => "D:\\Games\\Steam\\steamapps\\common\\Cities Skylines II\\Cities2_Data\\StreamingAssets";
    public LocaleHelper LocaleHelper { get; }
    public FileSystemHelper FileSystemHelper { get; }
    public ErrorMessageHelper ErrorMessageHelper { get; }
    public ModTestRuntimeContainer() {
        this.LocaleHelper = new LocaleHelper(this);
        this.FileSystemHelper = new FileSystemHelper(this);
        this.ErrorMessageHelper = new ErrorMessageHelper(this);
    }
}
