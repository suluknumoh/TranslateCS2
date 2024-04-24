using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Colossal;

using Newtonsoft.Json;

using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Models;
internal class TranslationFile : IDictionarySource, IEquatable<TranslationFile?> {
    private Dictionary<string, string>? dictionary;
    public string LocaleId { get; private set; }
    public string LocaleName { get; private set; }
    public bool IsOK {
        get {
            if (String.IsNullOrEmpty(this.LocaleId)
                || String.IsNullOrWhiteSpace(this.LocaleId)) {
                return false;
            }
            if (String.IsNullOrEmpty(this.LocaleName)
                || String.IsNullOrWhiteSpace(this.LocaleName)) {
                return false;
            }
            if (this.dictionary == null
                || this.dictionary.Count == 0) {
                return false;
            }
            return true;
        }
    }
    public string Name { get; }
    public string Path { get; }
    public TranslationFile(string name, string path) {
        this.Name = name;
        this.LocaleId = this.Name;
        this.Path = path;
        string json = this.ReadJson();
        this.dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        bool got = this.dictionary.TryGetValue(ModConstants.LocaleNameLocalizedKey, out string localeName);
        if (got) {
            this.LocaleName = localeName;
        } else {
            // use this as fallback
            // dont use native name! - does not work!
            //this.LocaleName = this.LanguageCulture.Culture?.EnglishName;
            // TODO:
        }
    }
    public void ReInit() {
        try {
            string json = this.ReadJson();
            Dictionary<string, string>? temporary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (temporary != null) {
                this.dictionary = temporary;
            }
        } catch {
            //
        }
    }
    private string ReadJson() {
        return File.ReadAllText(this.Path, Encoding.UTF8);
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        return this.dictionary
            .Where(item =>
                !String.IsNullOrEmpty(item.Key)
                && !String.IsNullOrWhiteSpace(item.Key)
                && !String.IsNullOrEmpty(item.Value)
                && !String.IsNullOrWhiteSpace(item.Value));
    }

    public void Unload() {
        //
    }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(nameof(TranslationFile));
        builder.AppendLine($"{nameof(this.Name)}: {this.Name}");
        builder.AppendLine($"{nameof(this.Path)}: {this.Path}");
        builder.AppendLine($"{nameof(this.LocaleId)}: {this.LocaleId}");
        builder.AppendLine($"{nameof(this.LocaleName)}: {this.LocaleName}");
        builder.AppendLine($"{nameof(this.IsOK)}: {this.IsOK}");
        return builder.ToString();
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as TranslationFile);
    }

    public bool Equals(TranslationFile? other) {
        return other is not null &&
               this.LocaleId == other.LocaleId &&
               this.LocaleName == other.LocaleName &&
               this.Name == other.Name;
    }

    public override int GetHashCode() {
        int hashCode = 1646257719;
        hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(this.LocaleId);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(this.LocaleName);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
        return hashCode;
    }

    public static bool operator ==(TranslationFile? left, TranslationFile? right) {
        return EqualityComparer<TranslationFile>.Default.Equals(left, right);
    }

    public static bool operator !=(TranslationFile? left, TranslationFile? right) {
        return !(left == right);
    }
}
