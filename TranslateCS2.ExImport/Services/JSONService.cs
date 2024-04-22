using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Sessions;
using TranslateCS2.ModBridge;

namespace TranslateCS2.ExImport.Services;
internal class JSONService {
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions {
        WriteIndented = true,
        IgnoreReadOnlyProperties = false,
        IgnoreReadOnlyFields = false,
        AllowTrailingCommas = true,
    };
    public async Task WriteLocalizationFileJson(ILocalizationFile localizationFile,
                                                string file) {
        await Task.Factory.StartNew(() => {
            List<ILocalizationDictionaryEntry> exp = localizationFile.LocalizationDictionary.Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation)).ToList();
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(exp, this._jsonSerializerOptions);
            File.WriteAllBytes(file, bytes);
        });
    }

    public async Task WriteLocalizationFileForMod(ILocalizationFile localizationFile,
                                                  string file,
                                                  bool addKey) {
        await Task.Factory.StartNew(() => {
            Func<ILocalizationDictionaryEntry, string> keySelector = (item) => item.Key;
            Func<ILocalizationDictionaryEntry, string> valueSelector = (item) => item.Translation;
            Dictionary<string, string> exp = localizationFile.LocalizationDictionary.Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation)).ToDictionary<ILocalizationDictionaryEntry, string, string>(keySelector, valueSelector);
            if (addKey) {
                exp.Add(ModConstants.LocaleNameLocalizedKey, localizationFile.LocaleNameLocalized);
            }
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(exp, this._jsonSerializerOptions);
            File.WriteAllBytes(file, bytes);
        });
    }

    public async Task<List<ILocalizationDictionaryEntry>?> ReadLocalizationFileJson(string file) {
        try {
            using Stream stream = File.OpenRead(file);
            List<LocalizationDictionaryEntry>? deserialized = await JsonSerializer.DeserializeAsync<List<LocalizationDictionaryEntry>?>(stream, this._jsonSerializerOptions);
            return deserialized?.ToList<ILocalizationDictionaryEntry>();
        } catch {
            using Stream stream = File.OpenRead(file);
            Dictionary<string, string>? deserialized = await JsonSerializer.DeserializeAsync<Dictionary<string, string>?>(stream, this._jsonSerializerOptions);
            ArgumentNullException.ThrowIfNull(deserialized);
            List<ILocalizationDictionaryEntry> localizationDictionaryEntries = [];
            foreach (KeyValuePair<string, string> entry in deserialized) {
                if (entry.Key == ModConstants.LocaleNameLocalizedKey) {
                    continue;
                }
                localizationDictionaryEntries.Add(new LocalizationDictionaryEntry(entry.Key, entry.Value));
            }
            return localizationDictionaryEntries;
        }
    }
}
