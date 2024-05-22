using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Inf;

[MyExcludeFromCoverage]
public static class ModConstants {

    /// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
    /// <seealso cref="TranslateCS2.Mod.Helpers.FileSystemHelper"/>
    public const string ModsData = nameof(ModsData);

    /// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
    /// <seealso cref="TranslateCS2.Mod.Helpers.FileSystemHelper"/>
    public const string ModsSettings = nameof(ModsSettings);
    //
    //
    //
    public const string NameSimple = nameof(TranslateCS2);
    public const string Name = $"{NameSimple}.Mod";
    //
    //
    //
    public static string ModId { get; } = "79187";
    public static string LocaleNameLocalizedKey => $"{nameof(TranslateCS2)}.{nameof(LocaleNameLocalizedKey)}";
    public static string JsonExtension => ".json";
    public static string JsonSearchPattern => $"*{JsonExtension}";
    public static string LocExtension => ".loc";
    public static string LocSearchPattern => $"*{LocExtension}";
    public static int MaxDisplayNameLength => 31;
    public static int MaxErroneous => 5;
    public static string ModExportKeyValueJsonName { get; } = $"_{Name}{JsonExtension}";
}
