﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;

using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

using TranslateCS2.Configurations.Views;
using TranslateCS2.Controls.Searchs;
using TranslateCS2.Helpers;
using TranslateCS2.Models;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Searchs;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties.I18N;
using TranslateCS2.Views.Dialogs;

namespace TranslateCS2.ViewModels.Works;
internal abstract class AEditViewModel<T> : ABaseViewModel {
    private readonly ViewConfigurations _viewConfigurations;
    private readonly IDialogService _dialogService;
    protected readonly List<ColumnSearchAble<LocalizationDictionaryEntry>> _columnsSearchAble = [];


    public TranslationSessionManager SessionManager { get; }


    private bool _HideTranslated;
    public bool HideTranslated {
        get => this._HideTranslated;
        set => this.SetProperty(ref this._HideTranslated, value, this.RefreshViewList);
    }


    private bool _OnlyTranslated;
    public bool OnlyTranslated {
        get => this._OnlyTranslated;
        set => this.SetProperty(ref this._OnlyTranslated, value, this.RefreshViewList);
    }


    private int _ElementCount;
    public int ElementCount {
        get => this._ElementCount;
        set => this.SetProperty(ref this._ElementCount, value);
    }


    public TextSearchControlContext<LocalizationDictionaryEntry> TextSearchContext { get; protected set; }

    public TranslationSession? CurrentSession => this.SessionManager.CurrentTranslationSession;
    public ObservableCollection<LocalizationDictionaryEntry> Mapping { get; } = [];
    public DelegateCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand { get; }
    public DelegateCommand<RoutedEventArgs> EditInNewWindowCommand { get; }
    protected AEditViewModel(ViewConfigurations viewConfigurations,
                             TranslationSessionManager translationSessionManager,
                             IDialogService dialogService) {
        this._viewConfigurations = viewConfigurations;
        this.SessionManager = translationSessionManager;
        this._dialogService = dialogService;
        this.InitColumnsSearchAble();
        this.TextSearchContext = new TextSearchControlContext<LocalizationDictionaryEntry>();
        this.CellEditEndingCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(this.CellEditEndingCommandAction);
        this.EditInNewWindowCommand = new DelegateCommand<RoutedEventArgs>(this.EditInNewWindowCommandAction);
    }

    private void EditInNewWindowCommandAction(RoutedEventArgs args) {
        if (args.Source is not MenuItem menuItem) {
            return;
        }
        if (menuItem.Parent is not ContextMenu contextMenu) {
            return;
        }
        if (contextMenu.PlacementTarget is not DataGrid dataGrid) {
            return;
        }
        if (dataGrid.SelectedCells[0].Item is not LocalizationDictionaryEntry entry) {
            return;
        }
        bool isCount = typeof(T) == typeof(EditOccurancesViewModel);
        IDialogParameters parameters = new DialogParameters {
            { nameof(LocalizationDictionaryEntry), entry },
            { nameof(isCount), isCount },

        };
        this._dialogService.ShowDialog(nameof(EditEntryLargeView), parameters, this.EditEntryLargeCallBack);
    }

    private void EditEntryLargeCallBack(IDialogResult dialogResult) {
        bool gotEdited = dialogResult.Parameters.TryGetValue(nameof(LocalizationDictionaryEntry), out LocalizationDictionaryEntry edited);
        if (dialogResult.Result == ButtonResult.OK && gotEdited) {
            this.Save(edited.Translation, edited);
        }
    }

    private void InitColumnsSearchAble() {
        this._columnsSearchAble.Add(
        new ColumnSearchAble<LocalizationDictionaryEntry>(I18NEdits.ColumnKey, I18NEdits.ColumnKeyTip) {
            IsChecked = true,
            Matcher = this.MatchesKeyColumn
        });
        this._columnsSearchAble.Add(
        new ColumnSearchAble<LocalizationDictionaryEntry>(I18NEdits.ColumnEnglish, I18NEdits.ColumnEnglishTip) {
            IsChecked = true,
            Matcher = this.MatchesEnglishColumn
        });
        this._columnsSearchAble.Add(
        new ColumnSearchAble<LocalizationDictionaryEntry>(I18NEdits.ColumnMerge, I18NEdits.ColumnMergeTip) {
            IsChecked = true,
            Matcher = this.MatchesMergeColumn
        });
        this._columnsSearchAble.Add(
        new ColumnSearchAble<LocalizationDictionaryEntry>(I18NEdits.ColumnTranslation, I18NEdits.ColumnTranslationTip) {
            Matcher = this.MatchesTranslationColumn
        });
        foreach (ColumnSearchAble<LocalizationDictionaryEntry> columnSearchAble in this._columnsSearchAble) {
            columnSearchAble.OnIsCheckedChange += this.RefreshViewList;
        }
    }

    protected void AddToolsGroup() {
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<T>();
        if (viewConfiguration != null) {
            RibbonGroup ribbonGroup = RibbonHelper.CreateRibbonGroup(I18NRibbon.Tools, false);
            IEnumerable<object> toolsGroupItems = this.CreateToolsGroupItems();
            foreach (object toolGroupItem in toolsGroupItems) {
                ribbonGroup.Items.Add(toolGroupItem);
            }
            {
                Binding isCheckedBinding = new Binding(nameof(this.HideTranslated)) { Source = this };
                RibbonCheckBox ribbonCheckBox = RibbonHelper.CreateRibbonCheckBox(I18NRibbon.HideTranslated, isCheckedBinding);
                ribbonGroup.Items.Add(ribbonCheckBox);
            }
            {
                Binding isCheckedBinding = new Binding(nameof(this.OnlyTranslated)) { Source = this };
                RibbonCheckBox ribbonCheckBox = RibbonHelper.CreateRibbonCheckBox(I18NRibbon.ShowOnlyTranslated, isCheckedBinding);
                ribbonGroup.Items.Add(ribbonCheckBox);
            }

            viewConfiguration.Tab.Items.Add(ribbonGroup);
        }
    }

    protected void AddCountGroup() {
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<T>();
        if (viewConfiguration != null) {
            RibbonGroup ribbonGroup = RibbonHelper.CreateRibbonGroup(I18NRibbon.RowsShown, false);
            {
                Binding textBinding = new Binding(nameof(this.ElementCount)) { Source = this, Mode = BindingMode.OneWay, StringFormat = "{0:N0}" };
                RibbonTextBox ribbonTextBox = RibbonHelper.CreateRibbonTextBox(null, false, textBinding);
                ribbonGroup.Items.Add(ribbonTextBox);
            }
            viewConfiguration.Tab.Items.Add(ribbonGroup);
        }
    }

    protected bool MatchesKeyColumn(LocalizationDictionaryEntry parameter) {
        return this.TextSearchContext.SearchString != null && parameter.Key != null && parameter.Key.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }
    protected bool MatchesEnglishColumn(LocalizationDictionaryEntry parameter) {
        return this.TextSearchContext.SearchString != null && parameter.Value != null && parameter.Value.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }
    protected bool MatchesMergeColumn(LocalizationDictionaryEntry parameter) {
        return this.TextSearchContext.SearchString != null && parameter.ValueMerge != null && parameter.ValueMerge.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }
    protected bool MatchesTranslationColumn(LocalizationDictionaryEntry parameter) {
        return this.TextSearchContext.SearchString != null && parameter.Translation != null && parameter.Translation.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }

    public override void OnNavigatedTo(NavigationContext navigationContext) {
        this.RefreshViewList();
    }

    protected virtual void RefreshViewList() {
        this.ElementCount = this.Mapping.Count;
    }

    protected abstract void CellEditEndingCommandAction(DataGridCellEditEndingEventArgs args);

    protected abstract void Save(string? translation, LocalizationDictionaryEntry edited);

    protected abstract IEnumerable<object> CreateToolsGroupItems();

    protected static void SetNewValue(ObservableCollection<LocalizationDictionaryEntry> list, string? translation, LocalizationDictionaryEntry edited) {
        foreach (LocalizationDictionaryEntry entry in list) {
            if (entry.Value == edited.Value) {
                entry.Translation = translation;
            }
        }
    }

    protected override void OnLoadedCommandAction() { }
}
