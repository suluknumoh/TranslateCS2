namespace TranslateCS2.Inf.Keyz;
public interface IMyKey {
    string Key { get; set; }
    bool IsIndexed { get; }
    string CountKey { get; }
    int Index { get; }
}
