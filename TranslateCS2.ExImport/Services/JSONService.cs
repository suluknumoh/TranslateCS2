using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using TranslateCS2.Core.Sessions;
using TranslateCS2.Inf;

namespace TranslateCS2.ExImport.Services;
internal class JSONService {
    private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions {
        WriteIndented = true,
        IgnoreReadOnlyProperties = false,
        IgnoreReadOnlyFields = false,
        AllowTrailingCommas = true,
    };

    public async Task WriteLocalizationFileJson(ILocalizationFile localizationFile,
                                                string file,
                                                bool addKey,
                                                bool addMergeValues) {

        IDictionary<string, string> exp = localizationFile.GetLocalizationsAsDictionary(addMergeValues);
        if (addKey) {
            exp.Add(ModConstants.LocaleNameLocalizedKey, localizationFile.LocaleNameLocalized);
        }
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(exp, this.jsonSerializerOptions);
        File.WriteAllBytes(file, bytes);
    }

    public async Task<List<ILocalizationEntry>?> ReadLocalizationFileJson(string file) {
        try {
            using Stream stream = File.OpenRead(file);
            List<LocalizationEntry>? deserialized = await JsonSerializer.DeserializeAsync<List<LocalizationEntry>?>(stream, this.jsonSerializerOptions);
            return deserialized?.ToList<ILocalizationEntry>();
        } catch {
            using Stream stream = File.OpenRead(file);
            Dictionary<string, string>? deserialized = await JsonSerializer.DeserializeAsync<Dictionary<string, string>?>(stream, this.jsonSerializerOptions);
            ArgumentNullException.ThrowIfNull(deserialized);
            List<ILocalizationEntry> localizationDictionaryEntries = [];
            foreach (KeyValuePair<string, string> entry in deserialized) {
                if (entry.Key == ModConstants.LocaleNameLocalizedKey
                    || StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Key)) {
                    continue;
                }
                localizationDictionaryEntries.Add(new LocalizationEntry(entry.Key, entry.Value, false));
            }
            return localizationDictionaryEntries;
        }
    }
}
