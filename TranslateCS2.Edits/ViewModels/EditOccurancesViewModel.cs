using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

using Prism.Ioc;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Inf;

namespace TranslateCS2.Edits.ViewModels;

internal class EditOccurancesViewModel : AEditViewModel<EditOccurancesViewModel> {
    private readonly List<AppLocFileEntry> entries = [];
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
        this.TextSearchContext.Columns.AddRange(this.columnsSearchAble.Skip(1));
        this.TextSearchContext.OnSearch += this.RefreshViewList;
    }

    protected override void CellEditEndingCommandAction(DataGridCellEditEndingEventArgs args) {
        if (args.Row.Item is not AppLocFileEntry edited) {
            return;
        }
        if (args.EditingElement is not TextBox textBox) {
            return;
        }
        this.Save(textBox.Text.Trim(), edited);
    }

    protected override void Save(string? translation, AppLocFileEntry edited) {
        if (this.CurrentSession is null) {
            return;
        }
        SetNewValue(this.CurrentSession.Localizations, translation, edited);
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
        this.entries.Clear();
        if (this.CurrentSession == null
            || this.CurrentSession.Localizations == null) {
            return;
        }
        IEnumerable<IGrouping<string, AppLocFileEntry>> groups =
            this.CurrentSession.Localizations
                .Select(item => item.Value)
                .GroupBy(entry => entry.Value);
        foreach (IGrouping<string, AppLocFileEntry> group in groups) {
            AppLocFileEntry entry = new AppLocFileEntry(null,
                                                        group.Key,
                                                        null,
                                                        null,
                                                        false);
            this.entries.Add(entry);
            foreach (AppLocFileEntry groupItem in group) {
                entry.AddKey(groupItem.Key.Key);
                entry.ValueMerge = groupItem.ValueMerge;
                entry.Translation = groupItem.Translation;
            }
        }

        foreach (AppLocFileEntry entry in this.entries) {
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
    private void SetNewValue(string? translation, AppLocFileEntry edited) {
        foreach (AppLocFileEntry entry in this.entries) {
            if ((!StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Key.Key) && entry.Key == edited.Key)
                || (!StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Value) && entry.Value == edited.Value)) {
                entry.Translation = translation;
            }
        }
    }
}
