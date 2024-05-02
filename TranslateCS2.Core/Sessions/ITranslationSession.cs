using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace TranslateCS2.Core.Sessions;
public interface ITranslationSession : IDataErrorInfo {
    long ID { get; set; }
    string? Name { get; set; }
    /// <summary>
    ///     local <see cref="DateTime"/> !!!
    ///     <br/>
    ///     gets converted to universal time within <see cref="TranslationsDB" />
    /// </summary>
    DateTime Started { get; set; }
    /// <summary>
    ///     local <see cref="DateTime"/>!!!
    ///     <br/>
    ///     gets converted to universal time within <see cref="TranslationsDB"/>
    /// </summary>
    DateTime LastEdited { get; set; }
    string? MergeLocalizationFileName { get; set; }
    string? MergeLanguageCode { get; }
    string? OverwriteLocalizationFileName { get; set; }
    string? OverwriteLocalizationLocaleID => this.OverwriteLocalizationFileName?.Split(".")[0];
    string? OverwriteLocalizationNameEN { get; set; }
    string? OverwriteLocalizationNameLocalized { get; set; }
    bool IsAutoSave => true;
    bool AreBaseAndMergeLocalizationFilesDifferent => this.OverwriteLocalizationFileName != this.MergeLocalizationFileName;
    ObservableCollection<ILocalizationDictionaryEntry> LocalizationDictionary { get; }
    string? DisplayName { get; set; }
    bool HasTranslationForKey(string key) {
        return this.LocalizationDictionary.Where(item => item.Key == key && item.IsTranslated).Any();
    }

    void UpdateWith(ITranslationSession session);
}
