using TranslateCS2.Inf;

namespace TranslateCS2.Core.Translators;
public class TranslatorResult {
    public string? Translation { get; set; }
    public string? Error { get; set; }
    public bool IsError => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Error);
}
