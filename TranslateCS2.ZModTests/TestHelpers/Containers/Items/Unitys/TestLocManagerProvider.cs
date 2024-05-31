using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal;

using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Services.Localizations;

using UnityEngine;

using Xunit;

namespace TranslateCS2.ZModTests.TestHelpers.Containers.Items.Unitys;
/// <summary>
///     imitates <see cref="Colossal.Localization.LocalizationManager"/>s behaviour for testing purposes
/// </summary>
internal class TestLocManagerProvider : ILocManagerProvider {
    public IDictionary<string, SystemLanguage> Locales { get; } = new Dictionary<string, SystemLanguage>();
    public IDictionary<string, string> LocaleNames { get; } = new Dictionary<string, string>();
    public IDictionary<string, IList<IDictionarySource>> Sources { get; } = new Dictionary<string, IList<IDictionarySource>>();
    public string ActiveLocaleId { get; private set; }
    public string FallbackLocaleId => "en-US";
    public TestLocManagerProvider() {
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

    public void AddBuiltIn() {
        TestLocFileServiceStrategy strategy = new TestLocFileServiceStrategy();
        LocFileService<string> locFileService = new LocFileService<string>(strategy);
        IEnumerable<FileInfo> builtInLocFiles = locFileService.GetLocalizationFiles();
        Assert.Equal(ModTestConstants.ExpectedBuiltInLocFileCount, builtInLocFiles.Count());
        foreach (FileInfo builtInLocFile in builtInLocFiles) {
            MyLocalization<string> locFile = locFileService.GetLocalizationFile(builtInLocFile);
            bool parsed = Enum.TryParse(locFile.NameEnglish, true, out SystemLanguage systemLanguage);
            Assert.True(parsed);
            if (parsed) {
                this.AddLocale(locFile.Id, systemLanguage, locFile.Name);
                IDictionarySource source = TestDictionarySource.FromMyLocalization(locFile);
                this.AddSource(locFile.Id, source);
            }
        }
    }
}
