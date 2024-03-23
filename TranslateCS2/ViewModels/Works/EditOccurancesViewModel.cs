using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

using TranslateCS2.Configurations.Views;
using TranslateCS2.Controls.Edits;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;

namespace TranslateCS2.ViewModels.Works;

internal class EditOccurancesViewModel : AEditViewModel<EditOccurancesViewModel, LocalizationDictionaryOccuranceEntry> {
    private readonly List<LocalizationDictionaryOccuranceEntry> _entries = [];
    public EditOccurancesViewModel(ViewConfigurations viewConfigurations, TranslationSessionManager translationSessionManager) : base(viewConfigurations, translationSessionManager) {
        this.AddToolsGroup();
        this.TextSearchContext = new TextSearchControlContext(this.RefreshViewList, false);
    }

    protected override void CellEditEndingCommandAction(DataGridCellEditEndingEventArgs args) {
        if (this.CurrentSession is null) {
            return;
        }
        if (args.Row.Item is not LocalizationDictionaryOccuranceEntry edited) {
            return;
        }
        if (args.EditingElement is not TextBox textBox) {
            return;
        }
        this.SetNewValue(this.CurrentSession.LocalizationDictionary, textBox, edited);
        this.SetNewValue(textBox, edited);
        this.SessionManager.SaveCurrentTranslationSessionsTranslations();
        this.RefreshViewList();
    }

    protected override IEnumerable<object> CreateToolsGroupItems() {
        return [];
    }

    protected override void RefreshViewList() {
        this.Mapping.Clear();
        this._entries.Clear();
        if (this.CurrentSession == null || this.CurrentSession.LocalizationDictionary == null) {
            return;
        }
        IEnumerable<IGrouping<string, LocalizationDictionaryEditEntry>> groups = this.CurrentSession.LocalizationDictionary.GroupBy(entry => entry.Value);
        foreach (IGrouping<string, LocalizationDictionaryEditEntry> group in groups) {
            LocalizationDictionaryOccuranceEntry entry = new LocalizationDictionaryOccuranceEntry(group.Key);
            this._entries.Add(entry);
            foreach (LocalizationDictionaryEditEntry groupItem in group) {
                entry.Keys.Add(groupItem.Key);
                entry.ValueMerge = groupItem.ValueMerge;
                entry.Translation = groupItem.Translation;
            }
        }

        foreach (LocalizationDictionaryOccuranceEntry entry in this._entries) {
            bool add = false;
            if (this.OnlyTranslated
                && !this.HideTranslated
                && entry.IsTranslated
                ) {
                add = true;
            } else if (!this.OnlyTranslated
                && !this.HideTranslated
                ) {
                add = true;
            } else if (this.HideTranslated && !this.OnlyTranslated && !entry.IsTranslated) {
                add = true;
            }
            if (!add) {
                continue;
            }
            if (!this.IsTextSearchMatch(entry)) {
                continue;
            }
            this.Mapping.Add(entry);
        }
    }

    private void SetNewValue(ObservableCollection<LocalizationDictionaryEditEntry> list, TextBox textBox, LocalizationDictionaryOccuranceEntry edited) {
        foreach (LocalizationDictionaryEditEntry entry in list) {
            if (entry.Value == edited.Value) {
                entry.Translation = textBox.Text.Trim();
            }
        }
    }
    private void SetNewValue(TextBox textBox, LocalizationDictionaryOccuranceEntry edited) {
        foreach (LocalizationDictionaryOccuranceEntry entry in this._entries) {
            if (entry.Value == edited.Value) {
                entry.Translation = textBox.Text.Trim();
            }
        }
    }
}
