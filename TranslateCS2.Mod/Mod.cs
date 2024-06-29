using System;
using System.Collections.Generic;
using System.IO;

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
            if (false) {
                this.TestToRetrieveModDirectories(modManager);
            }
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

    private void TestToRetrieveModDirectories(ModManager modManager) {
        IEnumerator<ModManager.ModInfo> enumerator = modManager.GetEnumerator();
        while (enumerator.MoveNext()) {
            ModManager.ModInfo current = enumerator.Current;
            ExecutableAsset asset = current.asset;

            // no need to care about
            // current.isLoaded / !current.isLoaded
            // a mod could be loaded after this one

            // no need to care about
            // current.isBursted

            if (!current.isValid
                || !asset.isMod) {
                // invalid or no mod (additional libraries within mod)
                continue;
            }
            if (asset.isDirty
                || asset.isDummy) {
                continue;
            }
            if (!asset.isLocal
                && !asset.isEnabled
                && !asset.isInActivePlayset
                ) {
                // neither local
                // nor enabled in active playset
                continue;
            }
            int index = asset.path.IndexOf(asset.subPath);
            string modsSubscribedPath = asset.path.Substring(0, index);
            string modPath = Path.Combine(modsSubscribedPath, asset.subPath);
            // TODO: specific dictionary
            string specificDictionaryPath = Path.Combine(modPath);
            DirectoryInfo directoryInfo = new DirectoryInfo(specificDictionaryPath);
            if (directoryInfo.Exists) {
                IEnumerable<FileInfo> files = directoryInfo.EnumerateFiles(ModConstants.JsonSearchPattern);
                // TODO: 
            }
        }
    }

    private void Init(GameManager gameManager) {
        this.RuntimeContainer = this.CreateRuntimeContainer(gameManager);
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
        IBuiltInLocaleIdProvider builtInLocaleIdProvider = localeAssetProvider;
        IIndexCountsProvider indexCountsProvider = new IndexCountsProvider(localeAssetProvider);
        return new ModRuntimeContainer(logProvider,
                                       this,
                                       locManagerProvider,
                                       intSettingsProvider,
                                       indexCountsProvider,
                                       builtInLocaleIdProvider,
                                       paths);
    }

}
