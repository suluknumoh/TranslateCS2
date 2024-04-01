using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using TranslateCS2.Helpers;
using TranslateCS2.Models;
using TranslateCS2.Models.LocDictionary;

namespace TranslateCS2.Services;
internal class JSONService {
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions {
        WriteIndented = true,
        IgnoreReadOnlyProperties = false,
        IgnoreReadOnlyFields = false,
        AllowTrailingCommas = true
    };
    public void WriteLocalizationFileJson(LocalizationFile localizationFile,
                                          string file) {
        List<LocalizationDictionaryEntry> exp = localizationFile.LocalizationDictionary.Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation)).ToList();
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(exp, this._jsonSerializerOptions);
        File.WriteAllBytes(file, bytes);
    }

    public List<LocalizationDictionaryEntry>? ReadLocalizationFileJson(string file) {
        using Stream stream = File.OpenRead(file);
        return JsonSerializer.Deserialize<List<LocalizationDictionaryEntry>?>(stream, this._jsonSerializerOptions);
    }
}
