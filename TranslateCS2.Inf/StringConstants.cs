namespace TranslateCS2.Inf;
public static class StringConstants {
    public static string Underscore { get; } = "_";
    public static string Space { get; } = " ";
    public static string Low { get; } = nameof(Low);
    public static string Colossal_Order { get; } = nameof(Colossal_Order).Replace(Underscore, Space);
    public static string Cities_Skylines_II { get; } = nameof(Cities_Skylines_II).Replace(Underscore, Space);
    public static string ForwardSlash { get; } = "/";
    public static string BackSlash { get; } = "\\";
    public static string Dot { get; } = ".";
    public static string ThreeDots { get; } = "...";
    public static string Dash { get; } = "-";
    public static string DataTilde { get; } = "Data~";
    public static string None { get; } = nameof(None);
    public static string NoneLower { get; } = nameof(None).ToLower();
    public static string CarriageReturn { get; } = "\r";
    public static string LineFeed { get; } = "\n";
}
