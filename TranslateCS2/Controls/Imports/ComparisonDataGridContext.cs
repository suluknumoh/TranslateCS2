using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Controls.Searchs;
using TranslateCS2.Models.Imports;
using TranslateCS2.Models.Searchs;
using TranslateCS2.Properties.I18N;

namespace TranslateCS2.Controls.Imports;
internal class ComparisonDataGridContext : BindableBase, INavigationAware {
    public TextSearchControlContext<CompareExistingImportedTranslation> TextSearchContext { get; }


    private ImportModes _ImportMode;
    public ImportModes ImportMode {
        get => this._ImportMode;
        set => this.SetProperty(ref this._ImportMode, value);
    }

    private readonly List<CompareExistingImportedTranslation> Backing = [];

    public ObservableCollection<CompareExistingImportedTranslation> Preview { get; } = [];
    public bool IsPreviewAvailable => this.Preview.Any();

    public ComparisonDataGridContext() {
        this.TextSearchContext = new TextSearchControlContext<CompareExistingImportedTranslation>();
        this.InitTextSearchContext();
    }

    private void InitTextSearchContext() {
        this.TextSearchContext.OnSearch += this.OnSearch;
        this.TextSearchContext.Columns.Add(
        new ColumnSearchAble<CompareExistingImportedTranslation>(I18NImport.ColumnKey, I18NImport.ColumnKeyTip) {
            IsChecked = true,
            Matcher = this.MatchesKeyColumn
        });
        this.TextSearchContext.Columns.Add(
        new ColumnSearchAble<CompareExistingImportedTranslation>(I18NImport.ColumnExisting, I18NImport.ColumnExistingTip) {
            IsChecked = true,
            Matcher = this.MatchesExistingColumn
        });
        this.TextSearchContext.Columns.Add(
        new ColumnSearchAble<CompareExistingImportedTranslation>(I18NImport.ColumnRead, I18NImport.ColumnReadTip) {
            IsChecked = true,
            Matcher = this.MatchesImportedColumn
        });
        foreach (ColumnSearchAble<CompareExistingImportedTranslation> column in this.TextSearchContext.Columns) {
            column.OnIsCheckedChange += this.OnSearch;
        }
    }


    private bool MatchesKeyColumn(CompareExistingImportedTranslation parameter) {
        return this.TextSearchContext.SearchString != null && parameter.Key != null && parameter.Key.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }
    private bool MatchesExistingColumn(CompareExistingImportedTranslation parameter) {
        return this.TextSearchContext.SearchString != null && parameter.TranslationExisting != null && parameter.TranslationExisting.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }
    private bool MatchesImportedColumn(CompareExistingImportedTranslation parameter) {
        return this.TextSearchContext.SearchString != null && parameter.TranslationImported != null && parameter.TranslationImported.Contains(this.TextSearchContext.SearchString, StringComparison.OrdinalIgnoreCase);
    }


    private void OnSearch() {
        Application.Current.Dispatcher.Invoke(() => {
            this.Preview.Clear();
            foreach (CompareExistingImportedTranslation item in this.Backing) {
                if (this.TextSearchContext.IsTextSearchMatch(item)) {
                    this.Preview.Add(item);
                }
            }
        });
    }


    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        this.Preview.Clear();
        this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        this.Preview.Clear();
        this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
    }

    public void Raiser() {
        this.RaisePropertyChanged(nameof(this.Preview));
        this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
        this.RaisePropertyChanged(nameof(this.TextSearchContext));
    }

    public void Clear() {
        this.Backing.Clear();
        Application.Current.Dispatcher.Invoke(this.Preview.Clear);
        this.Raiser();
    }

    public void SetItems(IList<CompareExistingImportedTranslation> preview) {
        this.Backing.AddRange(preview);
        this.OnSearch();
    }

    internal IList<CompareExistingImportedTranslation> GetItems() {
        return this.Backing;
    }
}
