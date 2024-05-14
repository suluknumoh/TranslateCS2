using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Colossal;

using Newtonsoft.Json;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Containers.Items;
internal class TranslationFile : IDictionarySource, IEquatable<TranslationFile?> {
    private readonly IModRuntimeContainer runtimeContainer;
    private string Uniquer { get; } = ModConstants.Name;
    private MyLanguage Language { get; }
    private Dictionary<string, string>? dictionary;
    public string LocaleId { get; private set; }
    public string LocaleName { get; private set; }
    public bool IsOK => this.EntryCount > 0;
    public int EntryCount => this.dictionary?.Count ?? 0;
    public string Path { get; }
    public TranslationFile(IModRuntimeContainer runtimeContainer,
                           string localeId,
                           string localeName,
                           string path,
                           MyLanguage language) {
        this.runtimeContainer = runtimeContainer;
        this.LocaleId = localeId;
        this.Path = path;
        this.Language = language;
        this.ReadJson();
        if (this.dictionary != null
            && this.dictionary.TryGetValue(ModConstants.LocaleNameLocalizedKey, out string? outLocaleName)
            && outLocaleName != null
            && !StringHelper.IsNullOrWhiteSpaceOrEmpty(outLocaleName)) {
            this.LocaleName = outLocaleName;
        }
        this.LocaleName ??= localeName;
    }
    public bool ReInit() {
        return this.ReadJson();
    }
    private bool ReadJson() {
        try {
            string json = File.ReadAllText(this.Path, Encoding.UTF8);
            Dictionary<string, string>? temporary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (temporary != null) {
                this.dictionary = temporary;
                return true;
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger?.LogError(this.GetType(),
                                                   LoggingConstants.FailedTo,
                                                   [nameof(ReadJson), ex, this]);
        }
        return false;
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors,
                                                                 Dictionary<string, int> indexCounts) {
        indexCounts.Clear();
        // has to be added!!! if dictionary is null, fallback-language is used!!!
        this.Language.AddIndexCounts(indexCounts);
        if (this.dictionary == null) {
            return [];
        }
        IEnumerable<KeyValuePair<string, string>> localDictionary = this.dictionary
            .Where(item =>
                !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Key)
                && !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Value));
        // TODO: indexed values???
        // what if it is incorrect?
        // for example:
        // Key: "Chirper.BIRTH_RATE_DOWN:0"
        // Chirper.BIRTH_RATE_DOWN has only this one indexed value
        // someone added
        // "Chirper.BIRTH_RATE_DOWN:2"
        // but the correct next indexed value would be
        // "Chirper.BIRTH_RATE_DOWN:1"
        // is the file incorrect?
        /// <see cref="IsOK"/>
        // autocorrect? - how and when? property IndexCounts for this class that is initialized after json is read?
        /// <see cref="ReadJson"/>
        return localDictionary;
    }

    public void Unload() {
        //
    }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(nameof(TranslationFile));
        builder.AppendLine($"{nameof(this.LocaleId)}: {this.LocaleId}");
        builder.AppendLine($"{nameof(this.LocaleName)}: {this.LocaleName}");
        builder.AppendLine($"{nameof(this.Path)}: {this.Path}");
        builder.AppendLine($"{nameof(this.IsOK)}: {this.IsOK}");
        return builder.ToString();
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as TranslationFile);
    }

    public bool Equals(TranslationFile? other) {
        return other is not null &&
               this.Uniquer == other.Uniquer &&
               this.LocaleId == other.LocaleId &&
               this.LocaleName == other.LocaleName &&
               this.Path == other.Path;
    }

    public override int GetHashCode() {
        int hashCode = 1111822720;
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Uniquer);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.LocaleId);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.LocaleName);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Path);
        return hashCode;
    }

    public static bool operator ==(TranslationFile? left, TranslationFile? right) {
        return EqualityComparer<TranslationFile>.Default.Equals(left, right);
    }

    public static bool operator !=(TranslationFile? left, TranslationFile? right) {
        return !(left == right);
    }
}
