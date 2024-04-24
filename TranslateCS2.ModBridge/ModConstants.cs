namespace TranslateCS2.ModBridge;

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
    public static string FailedToLoad => "failed to load:";
    public static string FailedToUnLoad => "failed to unload:";
    public static string StrangerThings => "failed to load the entire mod:";
    public static string StrangerThingsDispose => "failed to dispose:";
}