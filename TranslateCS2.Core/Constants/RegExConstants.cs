﻿using System.Text.RegularExpressions;

namespace TranslateCS2.Core.Constants;
public static class RegExConstants {
    public static Regex IsOnlyAZCaseInSensitive { get; } = new Regex("^[a-zA-Z]+$");
    public static Regex ContainsNonBasicLatinCharacters { get; } = new Regex("\\P{IsBasicLatin}");
}