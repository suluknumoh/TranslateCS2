using System;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Environment;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod;
/// <summary>
///     dont move this <see langword="class" /> somewhere else
///     <br/>
///     dont rename this <see langword="class" />
///     <br/>
///     <br/>
///     its <see langword="namespace" /> and name are used to build an id for the <see cref="ModSettingsLocale"/>
///     <br/>
///     <seealso cref="Game.Modding.ModSetting.ModSetting(IMod)"/>
/// </summary>
[MyExcludeFromCoverage]
public class Mod : IMod {
    private static readonly ILog Logger = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    private IModRuntimeContainer? RuntimeContainer { get; set; }
    public Mod() {
        // ctor. is never called/used by unity/co
    }
    public void OnLoad(UpdateSystem updateSystem) {
        try {
            GameManager gameManager = GameManager.instance;
            ModManager modManager = gameManager.modManager;
            if (modManager.TryGetExecutableAsset(this,
                                                 out ExecutableAsset asset)) {
                this.Init(gameManager);
            }
        } catch (Exception ex) {
            // user LogManagers Logger
            // runtimeContainerHandler might not be initialized
            Logger.Critical(ex, nameof(OnLoad));
        }
    }

    private void Init(GameManager gameManager) {
        this.RuntimeContainer = this.CreateRuntimeContainer(gameManager);
        this.RuntimeContainer.Logger.LogInfo(this.GetType(), nameof(OnLoad));

        this.RuntimeContainer.Init(AssetDatabase.global.LoadSettings, true);

        if (this.RuntimeContainer.Languages.HasErroneous) {
            this.RuntimeContainer.ErrorMessages.DisplayErrorMessageForErroneous(this.RuntimeContainer.Languages.Erroneous,
                                                                                false);
        }
    }

    public void OnDispose() {
        try {
            this.RuntimeContainer.Dispose(true);
        } catch (Exception ex) {
            // user LogManagers Logger
            // runtimeContainerHandler might not be initialized
            Logger.Critical(ex, nameof(OnDispose));
        }
    }


    private IModRuntimeContainer CreateRuntimeContainer(GameManager gameManager) {
        IMyLogProvider logProvider = new ModLogProvider(Logger);
        Paths paths = new Paths(true,
                                EnvPath.kStreamingDataPath,
                                EnvPath.kUserDataPath);
        ILocManagerProvider locManagerProvider = new LocManagerProvider(gameManager.localizationManager);
        IIntSettingsProvider intSettingsProvider = new IntSettingsProvider(gameManager.settings.userInterface);
        LocaleAssetProvider localeAssetProvider = new LocaleAssetProvider(AssetDatabase.global);
        IIndexCountsProvider indexCountsProvider = new IndexCountsProvider(localeAssetProvider);
        return new ModRuntimeContainer(logProvider,
                                       this,
                                       locManagerProvider,
                                       intSettingsProvider,
                                       indexCountsProvider,
                                       paths);
    }

}
