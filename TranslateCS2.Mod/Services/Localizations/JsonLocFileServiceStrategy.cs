using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Newtonsoft.Json;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;

using UnityEngine;

namespace TranslateCS2.Mod.Services.Localizations;
internal class JsonLocFileServiceStrategy : LocFileServiceStrategy<string> {
    private readonly IModRuntimeContainer runtimeContainer;
    public override string LocFileDirectory => this.runtimeContainer.Paths.ModsDataPathSpecific;
    public override string SearchPattern { get; } = ModConstants.JsonSearchPattern;
    public JsonLocFileServiceStrategy(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
    }
    public override MyLocalization<string> GetFile(FileInfo fileInfo) {
        string localeId = this.GetLocaleIdFromFileInfo(fileInfo);
        MyLanguage? language = this.runtimeContainer.Languages.GetLanguage(localeId);
        if (language is null) {
            throw new ArgumentNullException(nameof(language));
        }
        // if language is not null,
        // cultureInfo cannot be null
        CultureInfo cultureInfo = language.GetCultureInfo(localeId);
        string localeName = GetLocaleNameNative(language.SystemLanguage,
                                                cultureInfo);

        MyLocalizationSource<string> source = this.CreateNewSource(fileInfo);
        MyLocalization<string> locFile = this.CreateNewFile(localeId,
                                                            cultureInfo.EnglishName,
                                                            localeName,
                                                            source);
        using Stream stream = File.OpenRead(fileInfo.FullName);
        this.ReadContent(source, stream);
        return locFile;
    }

    private static string GetLocaleNameNative(SystemLanguage systemLanguage,
                                              CultureInfo cultureInfo) {
        string localeName = cultureInfo.NativeName;
        if (systemLanguage == SystemLanguage.SerboCroatian) {
            if (cultureInfo.EnglishName.Contains(LangConstants.Latin)) {
                localeName += $" ({LangConstants.Latin})";
            } else if (cultureInfo.EnglishName.Contains(LangConstants.Cyrillic)) {
                localeName += $" ({LangConstants.Cyrillic})";
            }
        }
        return localeName;
    }

    private string GetLocaleIdFromFileInfo(FileInfo fileInfo) {
        string localeIdPre = this.runtimeContainer.Paths.ExtractLocaleIdFromPath(fileInfo.Name);
        string localeId = this.runtimeContainer.Locales.CorrectLocaleId(localeIdPre);
        return localeId;
    }

    protected override string CreateEntryValue(string key, string value) {
        return value;
    }
    public override bool ReadContent(MyLocalizationSource<string> source,
                                     Stream? streamParameter = null) {
        try {
            //string json = File.ReadAllText(source.File.FullName, Encoding.UTF8);
            //IDictionary<string, string>? temporary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //
            //
            using Stream stream = streamParameter ?? source.File.OpenRead();
            JsonSerializer serializer = JsonSerializer.CreateDefault();
            using TextReader textReader = new StreamReader(stream);
            using JsonReader jsonReader = new JsonTextReader(textReader);
            IDictionary<string, string>? temporary = serializer.Deserialize<Dictionary<string, string>>(jsonReader);
            return HandleRead(source, temporary);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(ReadContent), ex, this]);
        }
        return false;
    }

    private static bool HandleRead(MyLocalizationSource<string> source, IDictionary<string, string>? temporary) {
        if (temporary is null) {
            return false;
        }
        IDictionary<string, string> local = DictionaryHelper.GetNonEmpty(temporary);
        source.Localizations.Clear();
        DictionaryHelper.AddAll(local, source.Localizations);
        return true;
    }
}
