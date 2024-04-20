using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal.IO.AssetDatabase;
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
    public static ILog Logger = LogManager.GetLogger(Name).SetShowsErrorsInUI(false);
    private string StrangerThings => "failed to load the entire mod: {0}";
    private ModSettings? _setting;
    private TranslationFileService _fileHelper;

    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Mod.Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                this._fileHelper = new TranslationFileService(asset);
                this._setting = new ModSettings(this, this._fileHelper);
                this._setting.RegisterInOptionsUI();
                GameManager.instance.localizationManager.AddSource("en-US", new ModSettingsLocale(this._setting));
                //
                //
                this._fileHelper.Load();
                //
                //
                AssetDatabase.global.LoadSettings(Name, this._setting, this._setting);
                if (false) {
                    // TODO: quick n dirty
                    // TODO: this is crap
                    IEnumerable<string> lines = File.ReadLines(Path.Combine(this._fileHelper.AssetDirectoryPath, "..", "..", "Settings.coc"));
                    lines = lines.Where(line => line.Contains("\"locale\": "));
                    if (lines.Any()) {
                        string locale = lines.First().Replace("\"", String.Empty).Replace("locale", String.Empty).Replace(":", String.Empty);
                        GameManager.instance.localizationManager.SetActiveLocale("en-US");
                        GameManager.instance.localizationManager.SetActiveLocale(locale);
                    }
                }

            }
        } catch (Exception ex) {
            Mod.Logger.LogCritical(this.GetType(),
                                   this.StrangerThings,
                                   [ex]);
        }
    }

    public void OnDispose() {
        Mod.Logger.LogInfo(this.GetType(), nameof(OnDispose));
        this._setting?.UnregisterInOptionsUI();
    }
}
