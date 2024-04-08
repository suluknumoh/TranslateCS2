using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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
    public async Task WriteLocalizationFileJson(ILocalizationFile localizationFile,
                                                string file) {
        await Task.Factory.StartNew(() => {
            List<ILocalizationDictionaryEntry> exp = localizationFile.LocalizationDictionary.Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation)).ToList();
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(exp, this._jsonSerializerOptions);
            File.WriteAllBytes(file, bytes);
        });
    }

    public async Task<List<ILocalizationDictionaryEntry>?> ReadLocalizationFileJson(string file) {
        using Stream stream = File.OpenRead(file);
        List<LocalizationDictionaryEntry>? deserialized = await JsonSerializer.DeserializeAsync<List<LocalizationDictionaryEntry>?>(stream, this._jsonSerializerOptions);
        return deserialized?.ToList<ILocalizationDictionaryEntry>();
    }
}
