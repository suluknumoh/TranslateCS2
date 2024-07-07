using System;
using System.IO;

using Colossal.Json;

using Game.Settings;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Helpers;

namespace TranslateCS2.Mod.Containers.Items;
internal partial class ModSettings {



    public const string GenerateGroup = nameof(GenerateGroup);



    [Exclude]
    [SettingsUIButton]
    [SettingsUIDeveloper]
    [SettingsUISection(Section, GenerateGroup)]
    public bool LogMarkdownAndCultureInfoNames {
        set => this.languages.LogMarkdownAndCultureInfoNames();
    }

    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(Section, GenerateGroup)]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsGenerateLocalizationJsonHiddenDisabled))]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsGenerateLocalizationJsonHiddenDisabled))]
    [SettingsUIDirectoryPicker]
    public string GenerateDirectory { get; set; }

    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(Section, GenerateGroup)]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsGenerateLocalizationJsonHiddenDisabled))]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsGenerateLocalizationJsonHiddenDisabled))]
    [SettingsUIConfirmation]
    [SettingsUIButton]
    public bool GenerateLocalizationJson {
        set {
            if (this.SettingsLocale is null) {
                return;
            }
            try {
                string path = Path.Combine(this.GenerateDirectory, ModConstants.ModExportKeyValueJsonName);
                JsonHelper.Write(this.SettingsLocale.ExportableEntries, path);
            } catch (Exception ex) {
                this.runtimeContainer.ErrorMessages.DisplayErrorMessageFailedToGenerateJson();
                this.runtimeContainer.Logger.LogError(this.GetType(),
                                                      LoggingConstants.FailedTo,
                                                      [nameof(this.GenerateLocalizationJson), ex]);
            }
        }
    }

    public bool IsGenerateLocalizationJsonHiddenDisabled() {
        return this.SettingsLocale is null;
    }
}
