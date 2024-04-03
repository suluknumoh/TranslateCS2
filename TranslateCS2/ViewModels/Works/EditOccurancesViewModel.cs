﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

using Prism.Services.Dialogs;

using TranslateCS2.Configurations.Views;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;

namespace TranslateCS2.ViewModels.Works;

internal class EditOccurancesViewModel : AEditViewModel<EditOccurancesViewModel> {
    private readonly List<LocalizationDictionaryEntry> _entries = [];
    public EditOccurancesViewModel(ViewConfigurations viewConfigurations,
                                   TranslationSessionManager translationSessionManager,
                                   IDialogService dialogService) : base(viewConfigurations,
                                                                        translationSessionManager,
                                                                        dialogService) {
        this.AddToolsGroup();
        this.AddCountGroup();
        this.TextSearchContext.Columns.AddRange(this._columnsSearchAble.Skip(1));
        this.TextSearchContext.OnSearch += this.RefreshViewList;
    }

    protected override void CellEditEndingCommandAction(DataGridCellEditEndingEventArgs args) {
        if (args.Row.Item is not LocalizationDictionaryEntry edited) {
            return;
        }
        if (args.EditingElement is not TextBox textBox) {
            return;
        }
        this.Save(textBox.Text.Trim(), edited);
    }

    protected override void Save(string? translation, LocalizationDictionaryEntry edited) {
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
        IEnumerable<IGrouping<string, LocalizationDictionaryEntry>> groups = this.CurrentSession.LocalizationDictionary.GroupBy(entry => entry.Value);
        foreach (IGrouping<string, LocalizationDictionaryEntry> group in groups) {
            LocalizationDictionaryEntry entry = new LocalizationDictionaryEntry(null, group.First().Value, null);
            this._entries.Add(entry);
            foreach (LocalizationDictionaryEntry groupItem in group) {
                entry.AddKey(groupItem.Key);
                entry.ValueMerge = groupItem.ValueMerge;
                entry.Translation = groupItem.Translation;
            }
        }

        foreach (LocalizationDictionaryEntry entry in this._entries) {
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
    private void SetNewValue(string? translation, LocalizationDictionaryEntry edited) {
        foreach (LocalizationDictionaryEntry entry in this._entries) {
            if (entry.Value == edited.Value) {
                entry.Translation = translation;
            }
        }
    }
}
