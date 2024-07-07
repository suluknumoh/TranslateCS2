using System;
using System.Collections.Generic;

using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items;
internal class ModSettingsLocales {
    private readonly ModSettings modSettings;
    private readonly Dictionary<string, ModSettingsLocaleSource> _LocaleSources = [];
    private readonly string fallbackLocaleId;
    public IReadOnlyDictionary<string, ModSettingsLocaleSource> LocaleSources => this._LocaleSources;
    public IReadOnlyDictionary<string, string> ExportableEntries => this._LocaleSources[this.fallbackLocaleId].ExportableEntries;
    public IReadOnlyDictionary<string, string> AllEntries => this._LocaleSources[this.fallbackLocaleId].AllEntries;

    public ModSettingsLocales(ModSettings modSettings, string fallbackLocaleId) {
        this.modSettings = modSettings;
        this.fallbackLocaleId = fallbackLocaleId;
    }

    public void Init(IEnumerable<MyLanguage> builtInLanguages) {
        foreach (MyLanguage language in builtInLanguages) {
            string locale = language.Id;
            bool isFallBackLocale = this.fallbackLocaleId.Equals(locale,
                                                                 StringComparison.OrdinalIgnoreCase);
            ModSettingsLocaleSource localeSource = new ModSettingsLocaleSource(this.modSettings,
                                                                               locale,
                                                                               isFallBackLocale);
            this._LocaleSources.Add(locale, localeSource);
        }

    }

    public void AddSources(ILocManagerProvider provider) {
        foreach (KeyValuePair<string, ModSettingsLocaleSource> entry in this.LocaleSources) {
            string localeId = entry.Key;
            ModSettingsLocaleSource source = entry.Value;
            provider.AddSource(localeId, source);
        }
    }
}
