using System;

using Colossal.IO.AssetDatabase;
using Colossal.Localization;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;
using TranslateCS2.Mod.Services;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    public const string Name = $"{nameof(TranslateCS2)}.{nameof(Mod)}";
    public const string NameSettings = $"{nameof(TranslateCS2)}{nameof(Mod)}";
    public static ILog Logger = LogManager.GetLogger(Name).SetShowsErrorsInUI(false);
    private static readonly GameManager gameManager = GameManager.instance;
    private static readonly LocalizationManager localizationManager = gameManager.localizationManager;
    private string StrangerThings => "failed to load the entire mod: {0}";
    private ModSettings? _setting;
    private TranslationFileService? _translationFileService;

    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                this._translationFileService = new TranslationFileService(asset);
                this._setting = new ModSettings(this, this._translationFileService);
                this._setting.RegisterInOptionsUI();
                localizationManager.AddSource("en-US", new ModSettingsLocale(this._setting));
                //
                //
                this._translationFileService.Load();
                //
                //
                AssetDatabase.global.LoadSettings(NameSettings, this._setting, this._setting);
                gameManager.settings.userInterface.onSettingsApplied += this._setting.ApplyAndSaveAlso;
            }
        } catch (Exception ex) {
            Logger.LogCritical(this.GetType(),
                                   this.StrangerThings,
                                   [ex]);
        }
    }

    public void OnDispose() {
        Logger.LogInfo(this.GetType(), nameof(OnDispose));
        this._setting?.UnregisterInOptionsUI();
        if (this._setting != null) {
            // dont replicate os lang into this mods settings
            gameManager.settings.userInterface.onSettingsApplied -= this._setting.ApplyAndSaveAlso;
        }
        // reset to os-language, if the mod is not used next time the game starts
        gameManager.settings.userInterface.currentLocale = LocalizationManager.kOsLanguage;
        gameManager.settings.userInterface.locale = LocalizationManager.kOsLanguage;
        gameManager.settings.userInterface.ApplyAndSave();
    }
}
