using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using TranslateCS2.Core.Models.Localizations;

namespace TranslateCS2.Core.Sessions;
public interface ITranslationSession {
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
    string? MergeLocalizationId => this.MergeLocalizationFileName?.Split('.')[0];
    string? LocNameEnglish { get; set; }
    string? LocName { get; set; }
    bool IsAutoSave => true;
    string? DisplayName { get; set; }
    ObservableCollection<KeyValuePair<string, IAppLocFileEntry>> Localizations { get; }
    bool HasTranslationForKey(string key) {
        return
            this.Localizations
                .Where(item => item.Value.Key.Key == key && item.Value.IsTranslated)
                .Any();
    }
    void UpdateWith(ITranslationSession session);
}
