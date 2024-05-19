using System;
using System.Collections.Generic;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Core.Models.Localizations;
internal class AppLocFileEntry : MyLocalizationEntry, IAppLocFileEntry {
    public HashSet<string> Keys { get; } = [];
    public string? KeyOrigin { get; }
    public int Count => this.Keys.Count;
    public string? ValueMerge { get; set; }
    public string? Translation { get; set; }
    public bool IsDeleteAble { get; }
    public bool IsTranslated => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation);
    public AppLocFileEntry(string key, string value) : base(key, value) { }
    public AppLocFileEntry(string key,
                           string? value,
                           string? valueMerge,
                           string? translation,
                           bool isDeleteAble) : base(key, value) {
        this.AddKey(key);
        this.ValueMerge = valueMerge;
        this.Translation = translation;
        this.IsDeleteAble = isDeleteAble;
    }
    public void AddKey(string keyParameter) {
        if (keyParameter == null) {
            return;
        }
        string key =
            keyParameter
                .Replace("\r", String.Empty)
                .Replace("\n", String.Empty)
                .Trim();
        this.Keys.Add(key);
    }

    public IAppLocFileEntry Clone() {
        AppLocFileEntry copy = new AppLocFileEntry(this.Key.Key,
                                                   this.Value,
                                                   this.ValueMerge,
                                                   this.Translation,
                                                   this.IsDeleteAble);
        foreach (string key in this.Keys) {
            copy.AddKey(key);
        }
        return copy;
    }
}
