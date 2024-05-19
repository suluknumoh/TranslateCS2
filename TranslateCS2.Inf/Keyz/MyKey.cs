using System;
using System.Collections.Generic;

namespace TranslateCS2.Inf.Keyz;
public class MyKey : IMyKey, IEquatable<MyKey?> {
    public string Key { get; set; }
    public bool IsIndexed => IndexCountHelper.IndexMatcher.IsMatch(this.Key);
    public string CountKey => IndexCountHelper.GetCountKeyFromKey(this.Key);
    public int Index => IndexCountHelper.GetIndexFromKey(this.Key);
    public MyKey(string key) {
        this.Key = key;
    }

    public override string ToString() {
        return this.Key;
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as MyKey);
    }

    public bool Equals(MyKey? other) {
        return other is not null &&
               this.Key == other.Key;
    }

    public override int GetHashCode() {
        return 990326508 + EqualityComparer<string>.Default.GetHashCode(this.Key);
    }

}
