namespace TranslateCS2.Core.Models.Localizations;
public static class AppLocFileEntryFactory {
    public static IAppLocFileEntry Create(string key,
                                          string value) {
        return new AppLocFileEntry(key,
                                   value);
    }
    public static IAppLocFileEntry Create(string key,
                                          string? value,
                                          string? valueMerge,
                                          string? translation,
                                          bool isDeleteAble) {
        return new AppLocFileEntry(key,
                                   value,
                                   valueMerge,
                                   translation,
                                   isDeleteAble);
    }

    public static IAppLocFileEntry CreateEmpty() {
        return AppLocFileEntryFactory.Create(null,
                                             null,
                                             null,
                                             null,
                                             true);
    }
}
