using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Media;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Ribbons.Sessions;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.ViewModels;
using TranslateCS2.Edits.ViewModels.Dialogs;
using TranslateCS2.Edits.Views.Dialogs;
using TranslateCS2.Inf;
using TranslateCS2.TextSearch.Models;
using TranslateCS2.TextSearch.ViewModels;

namespace TranslateCS2.Edits.ViewModels;
internal abstract class AEditViewModel<T> : ABaseViewModel {
    private readonly IContainerProvider containerProvider;
    private readonly IViewConfigurations viewConfigurations;
    private readonly IDialogService dialogService;
    protected readonly List<ColumnSearchAble<AppLocFileEntry>> columnsSearchAble = [];


    public ITranslationSessionManager SessionManager { get; }


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


    public TextSearchControlContext<AppLocFileEntry> TextSearchContext { get; protected set; }

    public ITranslationSession? CurrentSession => this.SessionManager.CurrentTranslationSession;
    public ObservableCollection<AppLocFileEntry> Mapping { get; } = [];
    public DelegateCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand { get; }
    public DelegateCommand<RoutedEventArgs> EditInNewWindowCommand { get; }
    protected AEditViewModel(IContainerProvider containerProvider,
                             IViewConfigurations viewConfigurations,
                             ITranslationSessionManager translationSessionManager,
                             IDialogService dialogService) {
        this.containerProvider = containerProvider;
        this.viewConfigurations = viewConfigurations;
        this.SessionManager = translationSessionManager;
        this.dialogService = dialogService;
        this.InitColumnsSearchAble();
        this.TextSearchContext = new TextSearchControlContext<AppLocFileEntry>();
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
        if (dataGrid.SelectedCells[0].Item is not AppLocFileEntry entry) {
            return;
        }
        AppLocFileEntry clone = AppLocFileEntry.Clone(entry);
        bool isCount = typeof(T) == typeof(EditOccurancesViewModel);
        IDialogParameters parameters = new DialogParameters {
            { nameof(AppLocFileEntry), clone },
            { nameof(EditEntryLargeViewModel.IsCount), isCount }
        };
        this.OpenEditEntryLarge(parameters);
    }

    protected void OpenEditEntryLarge(IDialogParameters parameters) {
        this.dialogService.ShowDialog(nameof(EditEntryLargeView), parameters, this.EditEntryLargeCallBack);
    }

    private void EditEntryLargeCallBack(IDialogResult dialogResult) {
        bool gotEdited = dialogResult.Parameters.TryGetValue(nameof(AppLocFileEntry), out AppLocFileEntry edited);
        if (!gotEdited) {
            return;
        }
        switch (dialogResult.Result) {
            case ButtonResult.OK:
                // save
                this.Save(edited.Translation, edited);
                break;
            case ButtonResult.Yes:
                // delete
                IEnumerable<KeyValuePair<string, AppLocFileEntry>> existings =
                    this.SessionManager.CurrentTranslationSession.Localizations
                    .Where(item => item.Key == edited.Key.Key)
                    .Where(item => StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Value.Value));
                if (existings.Any()) {
                    KeyValuePair<string, AppLocFileEntry> existing = existings.First();
                    existing.Value.Translation = null;
                    this.SessionManager.SaveCurrentTranslationSessionsTranslations();
                    this.SessionManager.CurrentTranslationSession.Localizations.Remove(existing);
                }
                this.RefreshViewList();
                break;
        }
    }

    private void InitColumnsSearchAble() {
        this.columnsSearchAble.Add(
        new ColumnSearchAble<AppLocFileEntry>(Properties.I18N.I18NEdits.ColumnKey, Properties.I18N.I18NEdits.ColumnKeyTip) {
            IsChecked = true,
            Matcher = this.MatchesKeyColumn
        });
        this.columnsSearchAble.Add(
        new ColumnSearchAble<AppLocFileEntry>(Properties.I18N.I18NEdits.ColumnEnglish, Properties.I18N.I18NEdits.ColumnEnglishTip) {
            IsChecked = true,
            Matcher = this.MatchesEnglishColumn
        });
        this.columnsSearchAble.Add(
        new ColumnSearchAble<AppLocFileEntry>(Properties.I18N.I18NEdits.ColumnMerge, Properties.I18N.I18NEdits.ColumnMergeTip) {
            IsChecked = true,
            Matcher = this.MatchesMergeColumn
        });
        this.columnsSearchAble.Add(
        new ColumnSearchAble<AppLocFileEntry>(Properties.I18N.I18NEdits.ColumnTranslation, Properties.I18N.I18NEdits.ColumnTranslationTip) {
            Matcher = this.MatchesTranslationColumn
        });
        foreach (ColumnSearchAble<AppLocFileEntry> columnSearchAble in this.columnsSearchAble) {
            columnSearchAble.OnIsCheckedChange += this.RefreshViewList;
        }
    }

    protected void AddToolsGroup() {
        IViewConfiguration? viewConfiguration = this.viewConfigurations.GetViewConfiguration<T>();
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
        IViewConfiguration? viewConfiguration = this.viewConfigurations.GetViewConfiguration<T>();
        if (viewConfiguration != null) {
            RibbonGroup ribbonGroup = RibbonHelper.CreateRibbonGroup(I18NRibbon.RowsShown, false);
            {
                Binding textBinding = new Binding(nameof(this.ElementCount)) { Source = this, Mode = BindingMode.OneWay, StringFormat = "{0:N0}" };
                RibbonTextBox ribbonTextBox = RibbonHelper.CreateRibbonTextBox(null, false, textBinding);
                ribbonTextBox.Background = Brushes.Transparent;
                ribbonGroup.Items.Add(ribbonTextBox);
            }
            viewConfiguration.Tab.Items.Add(ribbonGroup);
        }
    }

    protected void AddAffectedSessionGroup() {
        IViewConfiguration? viewConfiguration = this.viewConfigurations.GetViewConfiguration<T>();
        if (viewConfiguration != null) {
            CurrentSessionInfo selectedSessionInfoGroup = this.containerProvider.Resolve<CurrentSessionInfo>();
            viewConfiguration.Tab.Items.Add(selectedSessionInfoGroup);
        }
    }

    protected bool MatchesKeyColumn(AppLocFileEntry parameter) {
        return this.TextSearchContext.SearchString != null && parameter.Key != null && parameter.Key.Key.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }
    protected bool MatchesEnglishColumn(AppLocFileEntry parameter) {
        return this.TextSearchContext.SearchString != null && parameter.Value != null && parameter.Value.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }
    protected bool MatchesMergeColumn(AppLocFileEntry parameter) {
        return this.TextSearchContext.SearchString != null && parameter.ValueMerge != null && parameter.ValueMerge.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }
    protected bool MatchesTranslationColumn(AppLocFileEntry parameter) {
        return this.TextSearchContext.SearchString != null && parameter.Translation != null && parameter.Translation.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }

    public override void OnNavigatedTo(NavigationContext navigationContext) {
        this.RefreshViewList();
    }

    protected virtual void RefreshViewList() {
        this.ElementCount = this.Mapping.Count;
    }

    protected abstract void CellEditEndingCommandAction(DataGridCellEditEndingEventArgs args);

    protected abstract void Save(string? translation, AppLocFileEntry edited);

    protected abstract IEnumerable<object> CreateToolsGroupItems();

    protected static void SetNewValue(ObservableCollection<KeyValuePair<string, AppLocFileEntry>> list, string? translation, AppLocFileEntry edited) {
        foreach (KeyValuePair<string, AppLocFileEntry> entry in list) {
            if ((!StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Key) && entry.Key == edited.Key.Key)
                || (!StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Value.Value) && entry.Value.Value == edited.Value)) {
                entry.Value.Translation = translation;
            }
        }
    }

    protected override void OnLoadedCommandAction() { }
}
