using System.Collections.Generic;

using Colossal;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;

using UnityEngine;

namespace TranslateCS2.ZZZModTestLib.TestHelpers.Containers.Items.Unitys;
/// <summary>
///     imitates <see cref="Colossal.Localization.LocalizationManager"/>s behaviour for testing purposes
/// </summary>
internal class TestLocManager : ILocManager {
    public IDictionary<string, SystemLanguage> Locales { get; } = new Dictionary<string, SystemLanguage>();
    public IDictionary<string, string> LocaleNames { get; } = new Dictionary<string, string>();
    public IDictionary<string, IList<IDictionarySource>> Sources { get; } = new Dictionary<string, IList<IDictionarySource>>();
    public string ActiveLocaleId { get; private set; }
    public string FallbackLocaleId => "en-US";
    public TestLocManager() {
        this.ActiveLocaleId = this.FallbackLocaleId;
    }
    public void AddLocale(string localeId, SystemLanguage systemLanguage, string localeName) {
        this.Locales[localeId] = systemLanguage;
        this.LocaleNames[localeId] = localeName;
    }

    public void AddSource(string localeId, IDictionarySource source) {
        if (this.SupportsLocale(localeId)) {
            if (!this.Sources.ContainsKey(localeId)) {
                this.Sources[localeId] = [];
            }
            this.Sources[localeId].Add(source);
        }
    }

    public void RemoveLocale(string localeId) {
        this.Locales.Remove(localeId);
        this.LocaleNames.Remove(localeId);
        this.Sources.Remove(localeId);
    }

    public void RemoveSource(string localeId, TranslationFile source) {
        if (this.Sources.ContainsKey(localeId)) {
            this.Sources[localeId].Remove(source);
        }
    }

    public void SetActiveLocale(string localeId) {
        this.ActiveLocaleId = localeId;
    }

    public bool SupportsLocale(string localeId) {
        return
            this.Locales.ContainsKey(localeId)
            && this.LocaleNames.ContainsKey(localeId);
    }
}
