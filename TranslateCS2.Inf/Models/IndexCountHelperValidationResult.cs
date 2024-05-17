namespace TranslateCS2.Inf.Models;
public class IndexCountHelperValidationResult {
    public bool IsValid => StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Erroneous);
    public string? Erroneous { get; }
    public int NextFreeIndex { get; }
    private IndexCountHelperValidationResult(string? erroneous, int nextFreeIndex) {
        this.Erroneous = erroneous;
        this.NextFreeIndex = nextFreeIndex;
    }

    public static IndexCountHelperValidationResult Valid() {
        return new IndexCountHelperValidationResult(null, 0);
    }
    public static IndexCountHelperValidationResult InValid(string erroneous, int nextFreeIndex) {
        return new IndexCountHelperValidationResult(erroneous, nextFreeIndex);
    }
}
