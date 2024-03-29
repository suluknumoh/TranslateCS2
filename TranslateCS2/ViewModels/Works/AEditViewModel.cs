using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Configurations.Views;
using TranslateCS2.Controls.Edits;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties;

namespace TranslateCS2.ViewModels.Works;
internal abstract class AEditViewModel<T> : BindableBase, INavigationAware {
    private readonly ViewConfigurations _viewConfigurations;

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


    public TextSearchControlContext TextSearchContext { get; protected set; }

    public TranslationSession? CurrentSession => this.SessionManager.CurrentTranslationSession;
    public ObservableCollection<LocalizationDictionaryEntry> Mapping { get; } = [];
    public DelegateCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand { get; }
    protected AEditViewModel(ViewConfigurations viewConfigurations,
                             TranslationSessionManager translationSessionManager) {
        this._viewConfigurations = viewConfigurations;
        this.SessionManager = translationSessionManager;
        this.CellEditEndingCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(this.CellEditEndingCommandAction);
    }

    protected void AddToolsGroup() {
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<T>();
        if (viewConfiguration != null) {
            RibbonGroup ribbonGroup = new RibbonGroup {
                Header = I18N.StringToolsCap,
                IsEnabled = false
            };
            IEnumerable<object> toolsGroupItems = this.CreateToolsGroupItems();
            foreach (object toolGroupItem in toolsGroupItems) {
                ribbonGroup.Items.Add(toolGroupItem);
            }
            {

                RibbonCheckBox ribbonCheckBox = new RibbonCheckBox {
                    Label = I18N.StringHideTranslated,
                    Cursor = Cursors.Hand
                };
                ribbonCheckBox.SetBinding(RibbonCheckBox.IsCheckedProperty, new Binding(nameof(this.HideTranslated)) { Source = this });
                ribbonGroup.Items.Add(ribbonCheckBox);
            }
            {

                RibbonCheckBox ribbonCheckBox = new RibbonCheckBox {
                    Label = I18N.StringShowOnlyTranslated,
                    Cursor = Cursors.Hand
                };
                ribbonCheckBox.SetBinding(RibbonCheckBox.IsCheckedProperty, new Binding(nameof(this.OnlyTranslated)) { Source = this });
                ribbonGroup.Items.Add(ribbonCheckBox);
            }

            viewConfiguration.Tab.Items.Add(ribbonGroup);
        }
    }

    protected void AddCountGroup() {
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<T>();
        if (viewConfiguration != null) {
            RibbonGroup ribbonGroup = new RibbonGroup {
                Header = I18N.StringRowsShownCap,
                IsEnabled = false,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            {
                RibbonTextBox ribbonTextBox = new RibbonTextBox {
                    IsEnabled = false,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                };
                ribbonTextBox.SetBinding(RibbonTextBox.TextProperty, new Binding(nameof(this.ElementCount)) { Source = this, Mode = BindingMode.OneWay, StringFormat = "{0:N0}" });
                ribbonGroup.Items.Add(ribbonTextBox);
            }

            viewConfiguration.Tab.Items.Add(ribbonGroup);
        }
    }

    protected bool IsTextSearchMatch(LocalizationDictionaryEntry entry) {
        if (this.TextSearchContext == null) {
            return true;
        }
        if (String.IsNullOrEmpty(this.TextSearchContext.SearchString)
            || String.IsNullOrWhiteSpace(this.TextSearchContext.SearchString)) {
            return true;
        }
        if (this.TextSearchContext.IsKey
            && entry.Key != null
            && entry.Key.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase)) {
            return true;
        }
        if (this.TextSearchContext.IsEnglishValue
            && entry.Value != null
            && entry.Value.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase)) {
            return true;
        }
        if (this.TextSearchContext.IsMergeValue
            && entry.ValueMerge != null
            && entry.ValueMerge.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase)) {
            return true;
        }
        if (this.TextSearchContext.IsTranslation
            && entry.Translation != null
            && entry.Translation.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase)) {
            return true;
        }
        return false;
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        //
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        this.RefreshViewList();
    }

    protected virtual void RefreshViewList() {
        this.ElementCount = this.Mapping.Count;
    }

    protected abstract void CellEditEndingCommandAction(DataGridCellEditEndingEventArgs args);

    protected abstract IEnumerable<object> CreateToolsGroupItems();

    protected static void SetNewValue(ObservableCollection<LocalizationDictionaryEntry> list, TextBox textBox, LocalizationDictionaryEntry edited) {
        foreach (LocalizationDictionaryEntry entry in list) {
            if (entry.Value == edited.Value) {
                entry.Translation = textBox.Text.Trim();
            }
        }
    }
}
