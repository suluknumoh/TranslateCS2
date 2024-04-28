using System.Text.RegularExpressions;

namespace TranslateCS2.ModBridge;
public static class RegExConstants {
    public static Regex IsOnlyAZCaseInSensitive { get; } = new Regex("^[a-zA-Z]+$");
}
