using Colossal.Localization;
using Colossal.Logging;
using Colossal.PSI.Environment;

using Game.SceneFlow;
using Game.Settings;

using TranslateCS2.Inf;

namespace TranslateCS2.Mod.Containers;
internal class ModRuntimeContainer : AModRuntimeContainer {
    private readonly GameManager gameManager;


    public override LocalizationManager? LocManager => this.gameManager.localizationManager;
    public override InterfaceSettings? IntSetting => this.gameManager.settings.userInterface;


    public ModRuntimeContainer(GameManager gameManager, ILog logger) : base(logger,
                                                                            new Paths(true,
                                                                                            EnvPath.kStreamingDataPath,
                                                                                            EnvPath.kUserDataPath)) {
        this.gameManager = gameManager;
    }
}
