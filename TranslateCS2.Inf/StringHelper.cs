using System;

namespace TranslateCS2.Inf;
public static class StringHelper {
    public static string? GetNullForEmpty(string? s) {
        if (IsNullOrWhiteSpaceOrEmpty(s)) {
            return null;
        }
        return s;
    }

    public static bool IsNullOrWhiteSpaceOrEmpty(string? s) {
        return String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s);
    }

    public static string CutStringAfterMaxLengthAndAddThreeDots(string s, int maxLength) {
        string result = s;
        if (maxLength >= 0
            && result.Length > maxLength) {
            result = result.Substring(0, maxLength);
            result += StringConstants.ThreeDots;
        }
        return result;
    }
}
