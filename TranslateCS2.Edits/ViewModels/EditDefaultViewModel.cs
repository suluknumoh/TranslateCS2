﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Shapes;

using Prism.Ioc;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Helpers.Ribbons;
using TranslateCS2.Core.Properties;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Services.Filters;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Edits.Properties.I18N;
using TranslateCS2.Edits.ViewModels.Dialogs;

namespace TranslateCS2.Edits.ViewModels;

internal class EditDefaultViewModel : AEditViewModel<EditDefaultViewModel> {
    private readonly IFiltersService _filtersService;
    private ILocalizationKeyFilter _selectedFilter;

    public ObservableCollection<ILocalizationKeyFilter> FiltersCustom { get; private set; }
    public ObservableCollection<ILocalizationKeyFilter> FiltersAutogenerated { get; private set; }
    public EditDefaultViewModel(IContainerProvider containerProvider,
                                IViewConfigurations viewConfigurations,
                                ITranslationSessionManager translationSessionManager,
                                IFiltersService filtersService,
                                IDialogService dialogService) : base(containerProvider,
                                                                     viewConfigurations,
                                                                     translationSessionManager,
                                                                     dialogService) {
        this._filtersService = filtersService;
        this.FiltersCustom = this._filtersService.GetFiltersCustom();
        this.FiltersAutogenerated = this._filtersService.GetFiltersAutogenerated();
        this._selectedFilter = this._filtersService.GetFilterNone();
        this.AddToolsGroup();
        this.AddCountGroup();
        this.AddAffectedSessionGroup();
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
        if (edited.KeyOrigin != null && edited.KeyOrigin != edited.Key) {
            // key has changed
            // set translation to null to delete from database
            IEnumerable<ILocalizationDictionaryEntry> existings = this.CurrentSession.LocalizationDictionary.Where(item => item.Key == edited.KeyOrigin);
            if (existings.Any()) {
                ILocalizationDictionaryEntry existing = existings.First();
                existing.Translation = null;
                this.SessionManager.SaveCurrentTranslationSessionsTranslations();
                this.CurrentSession.LocalizationDictionary.Remove(existing);
            }
        }
        if (!this.CurrentSession.LocalizationDictionary.Contains(edited)) {
            this.CurrentSession.LocalizationDictionary.Add(edited);
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
        IList<object> items = [];
        {
            RibbonButton button = RibbonHelper.CreateRibbonButton(I18NEdits.DoAdd,
                                                                  ImageResources.add,
                                                                  this.AddEntry);
            items.Add(button);
        }
        {
            Rectangle rectangle = RibbonHelper.CreateInGroupSeparator();
            items.Add(rectangle);
        }
        {
            RibbonComboBoxConfig config = new RibbonComboBoxConfig(I18NRibbon.FilterKeys) {
                OnSelectionChanged = this.FilterChanged,
                SelectedItem = this._selectedFilter,
                DisplayMemberPath = nameof(ILocalizationKeyFilter.Name)
            };
            config.ItemsList.Add(new RibbonComboBoxItems(new List<ILocalizationKeyFilter>([this._selectedFilter])));
            config.ItemsList.Add(new RibbonComboBoxItems(this.FiltersCustom) { SeparatorLabel = I18NRibbon.FiltersCustomLabel });
            config.ItemsList.Add(new RibbonComboBoxItems(this.FiltersAutogenerated) { SeparatorLabel = I18NRibbon.FiltersAutogeneratedLabel });
            items.Add(RibbonHelper.CreateComboBox(config));
        }
        return items;
    }

    private void FilterChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        if (e.NewValue is ILocalizationKeyFilter newValue) {
            this._selectedFilter = newValue;
            this.RefreshViewList();
        }
    }

    private void AddEntry(object sender, RoutedEventArgs e) {
        ILocalizationDictionaryEntry entry = new LocalizationDictionaryEntry(null, null, true);
        IDialogParameters parameters = new DialogParameters {
            { nameof(ILocalizationDictionaryEntry), entry },
            { nameof(EditEntryLargeViewModel.IsCount), false }
        };
        this.OpenEditEntryLarge(parameters);
    }
}
