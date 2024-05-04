using Colossal.Localization;
using Colossal.Logging;
using Colossal.PSI.Environment;

using Game.SceneFlow;
using Game.Settings;

using TranslateCS2.Mod.Helpers;

namespace TranslateCS2.Mod.Containers;
internal class ModRuntimeContainer : IModRuntimeContainer {
    private readonly GameManager gameManager;
    public ILog? Logger { get; }
    public LocalizationManager? LocManager => this.gameManager.localizationManager;
    public InterfaceSettings? IntSetting => this.gameManager.settings.userInterface;
    public LocaleHelper LocaleHelper { get; }
    public FileSystemHelper FileSystemHelper { get; }
    public ErrorMessageHelper ErrorMessageHelper { get; }
    public string UserDataPath => EnvPath.kUserDataPath;
    public string StreamingDataPath => EnvPath.kStreamingDataPath;
    public ModRuntimeContainer(GameManager gameManager, ILog logger) {
        this.gameManager = gameManager;
        this.Logger = logger;
        this.LocaleHelper = new LocaleHelper(this);
        this.FileSystemHelper = new FileSystemHelper(this);
        this.ErrorMessageHelper = new ErrorMessageHelper(this);
    }
}
