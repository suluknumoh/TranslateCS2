using System;

namespace TranslateCS2.Helpers;
internal static class StringHelper {
    public static string? GetNullForEmpty(string? s) {
        if (IsNullOrWhiteSpaceOrEmpty(s)) {
            return null;
        }
        return s;
    }

    public static bool IsNullOrWhiteSpaceOrEmpty(string? s) {
        return String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s);
    }
}
