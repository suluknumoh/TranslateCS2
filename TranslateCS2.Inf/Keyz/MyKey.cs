namespace TranslateCS2.Inf.Keyz;
public class MyKey : IMyKey {
    public string Key { get; set; }
    public bool IsIndexed => IndexCountHelper.IndexMatcher.IsMatch(this.Key);
    public string CountKey => IndexCountHelper.GetCountKeyFromKey(this.Key);
    public int Index => IndexCountHelper.GetIndexFromKey(this.Key);
    public MyKey(string key) {
        this.Key = key;
    }
}
