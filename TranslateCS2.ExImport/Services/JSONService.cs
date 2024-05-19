using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Inf;

namespace TranslateCS2.ExImport.Services;
internal class JSONService {
    private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions {
        WriteIndented = true,
        IgnoreReadOnlyProperties = false,
        IgnoreReadOnlyFields = false,
        AllowTrailingCommas = true,
    };

    public async Task WriteLocalizationFileJson(IDictionary<string, string> localizationDictionary,
                                                string file) {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(localizationDictionary, this.jsonSerializerOptions);
        await File.WriteAllBytesAsync(file, bytes);
    }

    public async Task<List<AppLocFileEntry>?> ReadLocalizationFileJson(string file) {
        using Stream stream = File.OpenRead(file);
        Dictionary<string, string>? deserialized = await JsonSerializer.DeserializeAsync<Dictionary<string, string>?>(stream, this.jsonSerializerOptions);
        ArgumentNullException.ThrowIfNull(deserialized);
        List<AppLocFileEntry> localizationDictionaryEntries = [];
        foreach (KeyValuePair<string, string> entry in deserialized) {
            if (entry.Key == ModConstants.LocaleNameLocalizedKey
                || StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Key)) {
                continue;
            }
            localizationDictionaryEntries.Add(new AppLocFileEntry(entry.Key,
                                                                  null,
                                                                  null,
                                                                  entry.Value,
                                                                  false));
        }
        return localizationDictionaryEntries;
    }
}

