using System;
using System.Collections.Generic;
using System.IO;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Helpers;

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
        // flavor cannot be null
        Flavor flavor = language.GetFlavor(localeId);

        MyLocalizationSource<string> source = this.CreateNewSource(fileInfo);
        MyLocalization<string> locFile = this.CreateNewFile(flavor.Id,
                                                            flavor.NameEnglish,
                                                            flavor.Name,
                                                            source);
        using Stream stream = File.OpenRead(fileInfo.FullName);
        this.ReadContent(source, stream);
        return locFile;
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
            IDictionary<string, string>? temporary = JsonHelper.DeSerializeFromStream<Dictionary<string, string>>(stream);
            HandleRead(temporary, source);
            return true;
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(ReadContent), ex, source]);
        }
        return false;
    }

    private static void HandleRead(IDictionary<string, string> from, MyLocalizationSource<string> to) {
        IDictionary<string, string> local = DictionaryHelper.GetNonEmpty(from);
        to.Localizations.Clear();
        DictionaryHelper.AddAll(local, to.Localizations);
    }
}
