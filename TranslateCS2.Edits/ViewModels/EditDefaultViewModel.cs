﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Prism.Services.Dialogs;

using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Services.Filters;
using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Edits.ViewModels;

internal class EditDefaultViewModel : AEditViewModel<EditDefaultViewModel> {
    private readonly IFiltersService _filtersService;
    private ILocalizationKeyFilter _selectedFilter;

    public ObservableCollection<ILocalizationKeyFilter> Filters { get; private set; }
    public EditDefaultViewModel(IViewConfigurations viewConfigurations,
                                ITranslationSessionManager translationSessionManager,
                                IFiltersService filtersService,
                                IDialogService dialogService) : base(viewConfigurations,
                                                                     translationSessionManager,
                                                                     dialogService) {
        this._filtersService = filtersService;
        this.Filters = this._filtersService.GetFilters();
        this._selectedFilter = this.Filters.First();
        this.AddToolsGroup();
        this.AddCountGroup();
        this.TextSearchContext.Columns.AddRange(this._columnsSearchAble);
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
        this.SessionManager.SaveCurrentTranslationSessionsTranslations();
        if (this.SessionManager.HasDatabaseError) {
            // see xaml-code
        }
        this.RefreshViewList();
    }

    protected override void RefreshViewList() {
        this.Mapping.Clear();
        if (this.CurrentSession == null || this.CurrentSession.LocalizationDictionary == null) {
            return;
        }
        foreach (ILocalizationDictionaryEntry entry in this.CurrentSession.LocalizationDictionary) {
            if (this._selectedFilter.Matches(entry)) {
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
        }
        base.RefreshViewList();
    }

    protected override IEnumerable<object> CreateToolsGroupItems() {
        // dont translate displayMemberPath
        return [RibbonHelper.CreateComboBox(I18NRibbon.FilterKeys, this.Filters, this.FilterChanged, this._selectedFilter, "Name")];
    }


    private void FilterChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        if (e.NewValue is ILocalizationKeyFilter newValue) {
            this._selectedFilter = newValue;
            this.RefreshViewList();
        }
    }
}