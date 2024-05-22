using System.Collections.Generic;

using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Inf.Keyz;
public class MyKey : IMyKey {
    public string Key { get; set; }
    public bool IsIndexed => IndexCountHelper.IndexMatcher.IsMatch(this.Key);
    public string CountKey => IndexCountHelper.GetCountKeyFromKey(this.Key);
    public int Index => IndexCountHelper.GetIndexFromKey(this.Key);
    public MyKey(string key) {
        this.Key = key;
    }

    [MyExcludeMethodFromCoverage]
    public override string ToString() {
        return this.Key;
    }

    [MyExcludeMethodFromCoverage]
    public override bool Equals(object? obj) {
        return this.Equals(obj as IMyKey);
    }

    [MyExcludeMethodFromCoverage]
    public bool Equals(IMyKey? other) {
        return other is not null &&
               this.Key == other.Key;
    }

    [MyExcludeMethodFromCoverage]
    public override int GetHashCode() {
        return 990326508 + EqualityComparer<string>.Default.GetHashCode(this.Key);
    }
}
