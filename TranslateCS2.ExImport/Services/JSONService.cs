using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Sessions;

namespace TranslateCS2.ExImport.Services;
internal class JSONService {
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions {
        WriteIndented = true,
        IgnoreReadOnlyProperties = false,
        IgnoreReadOnlyFields = false,
        AllowTrailingCommas = true,
    };
    public void WriteLocalizationFileJson(ILocalizationFile localizationFile,
                                          string file) {
        List<ILocalizationDictionaryEntry> exp = localizationFile.LocalizationDictionary.Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation)).ToList();
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(exp, this._jsonSerializerOptions);
        File.WriteAllBytes(file, bytes);
    }

    public List<ILocalizationDictionaryEntry>? ReadLocalizationFileJson(string file) {
        using Stream stream = File.OpenRead(file);
        List<LocalizationDictionaryEntry>? deserialized = JsonSerializer.Deserialize<List<LocalizationDictionaryEntry>?>(stream, this._jsonSerializerOptions);
        return deserialized?.ToList<ILocalizationDictionaryEntry>();
    }
}
