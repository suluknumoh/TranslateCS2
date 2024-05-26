using System;
using System.Collections.Generic;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Keyz;

namespace TranslateCS2.Core.Models.Localizations;
internal class AppLocFileEntry : IAppLocFileEntry {
    public IMyKey Key { get; }
    public string? Value { get; set; }
    public HashSet<string> Keys { get; } = [];
    public string? KeyOrigin { get; }
    public int Count => this.Keys.Count;
    public string? ValueMerge { get; set; }
    public string? Translation { get; set; }
    public bool IsDeleteAble { get; }
    public bool IsTranslated => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation);
    public AppLocFileEntry(string key, string? value) {
        this.Key = new MyKey(key);
        this.Value = value;
    }
    public AppLocFileEntry(string key,
                           string? value,
                           string? valueMerge,
                           string? translation,
                           bool isDeleteAble) : this(key, value) {
        this.AddKey(key);
        this.ValueMerge = valueMerge;
        this.Translation = translation;
        this.IsDeleteAble = isDeleteAble;
    }
    public void AddKey(string keyParameter) {
        if (keyParameter is null) {
            return;
        }
        string key =
            keyParameter
                .Replace(StringConstants.CarriageReturn, String.Empty)
                .Replace(StringConstants.LineFeed, String.Empty)
                .Trim();
        this.Keys.Add(key);
    }

    public IAppLocFileEntry Clone() {
        IAppLocFileEntry copy = AppLocFileEntryFactory.Create(this.Key.Key,
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
