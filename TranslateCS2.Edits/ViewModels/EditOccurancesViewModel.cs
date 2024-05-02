using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

using Prism.Ioc;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Edits.ViewModels;

internal class EditOccurancesViewModel : AEditViewModel<EditOccurancesViewModel> {
    private readonly List<ILocalizationDictionaryEntry> _entries = [];
    public EditOccurancesViewModel(IContainerProvider containerProvider,
                                   IViewConfigurations viewConfigurations,
                                   ITranslationSessionManager translationSessionManager,
                                   IDialogService dialogService) : base(containerProvider,
                                                                        viewConfigurations,
                                                                        translationSessionManager,
                                                                        dialogService) {
        this.AddToolsGroup();
        this.AddCountGroup();
        this.AddAffectedSessionGroup();
        this.TextSearchContext.Columns.AddRange(this._columnsSearchAble.Skip(1));
        this.TextSearchContext.OnSearch += this.RefreshViewList;
    }

    protected override void CellEditEndingCommandAction(DataGridCellEditEndingEventArgs args) {
        if (args.Row.Item is not ILocalizationDictionaryEntry edited) {
            return;
        }
        if (args.EditingElement is not TextBox textBox) {
            return;
        }
        this.Save(textBox.Text.Trim(), edited);
    }

    protected override void Save(string? translation, ILocalizationDictionaryEntry edited) {
        if (this.CurrentSession is null) {
            return;
        }
        SetNewValue(this.CurrentSession.LocalizationDictionary, translation, edited);
        this.SetNewValue(translation, edited);
        this.SessionManager.SaveCurrentTranslationSessionsTranslations();
        if (this.SessionManager.HasDatabaseError) {
            // see xaml-code
        }
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
        IEnumerable<IGrouping<string, ILocalizationDictionaryEntry>> groups = this.CurrentSession.LocalizationDictionary.GroupBy(entry => entry.Value);
        foreach (IGrouping<string, ILocalizationDictionaryEntry> group in groups) {
            ILocalizationDictionaryEntry entry = new LocalizationDictionaryEntry(null, group.First().Value, null, false);
            this._entries.Add(entry);
            foreach (ILocalizationDictionaryEntry groupItem in group) {
                entry.AddKey(groupItem.Key);
                entry.ValueMerge = groupItem.ValueMerge;
                entry.ValueMergeLanguageCode = groupItem.ValueMergeLanguageCode;
                entry.Translation = groupItem.Translation;
            }
        }

        foreach (ILocalizationDictionaryEntry entry in this._entries) {
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
            if (!this.TextSearchContext.IsTextSearchMatch(entry)) {
                continue;
            }
            this.Mapping.Add(entry);
        }
        base.RefreshViewList();
    }
    private void SetNewValue(string? translation, ILocalizationDictionaryEntry edited) {
        foreach (ILocalizationDictionaryEntry entry in this._entries) {
            if (entry.Value == edited.Value
                || entry.Key == edited.Key) {
                entry.Translation = translation;
            }
        }
    }
}
