using System;
using System.Collections.Generic;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Keyz;

namespace TranslateCS2.Inf.Models.Localizations;
public class MyLocalizationEntry : IMyKeyProvider, IEquatable<MyLocalizationEntry?> {
    public IMyKey Key { get; }
    public string? Value { get; set; }
    public MyLocalizationEntry(string key, string? value) {
        this.Key = new MyKey(key);
        this.Value = value;
    }

    [MyExcludeMethodFromCoverage]
    public override bool Equals(object? obj) {
        return this.Equals(obj as MyLocalizationEntry);
    }

    [MyExcludeMethodFromCoverage]
    public bool Equals(MyLocalizationEntry? other) {
        return other is not null &&
               EqualityComparer<IMyKey>.Default.Equals(this.Key, other.Key);
    }

    [MyExcludeMethodFromCoverage]
    public override int GetHashCode() {
        return 990326508 + EqualityComparer<IMyKey>.Default.GetHashCode(this.Key);
    }
}
