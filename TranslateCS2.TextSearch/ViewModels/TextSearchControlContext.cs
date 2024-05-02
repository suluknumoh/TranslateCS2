using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;

using TranslateCS2.Inf;
using TranslateCS2.TextSearch.Models;

namespace TranslateCS2.TextSearch.ViewModels;
public class TextSearchControlContext<T> : BindableBase {
    public event Action? OnSearch;


    private string? _SearchString;
    public string? SearchString {
        get => this._SearchString;
        set => this.SetProperty(ref this._SearchString, value);
    }


    public ObservableCollection<ColumnSearchAble<T>> Columns { get; } = [];


    public DelegateCommand<RoutedEventArgs> SearchCommand { get; }
    public DelegateCommand ClearCommand { get; }

    public TextSearchControlContext() {
        this.SearchCommand = new DelegateCommand<RoutedEventArgs>(this.SearchCommandAction);
        this.ClearCommand = new DelegateCommand(this.ClearCommandAction);
    }

    private void SearchCommandAction(RoutedEventArgs args) {
        if (args is KeyEventArgs keyEventArgs) {
            switch (keyEventArgs.Key) {
                case Key.Enter:
                // Key is Integer enum and Key.Enter and Key.Return both have the same Integer!
                //case Key.Return:
                case Key.Execute:
                    break;
                default:
                    return;
            }
        }
        this.Invoke();
    }

    private void ClearCommandAction() {
        this.SearchString = null;
        this.Invoke();
    }

    private void Invoke() {
        OnSearch?.Invoke();
    }

    public bool IsTextSearchMatch(T entry) {
        if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.SearchString)) {
            return true;
        }
        foreach (ColumnSearchAble<T> column in this.Columns) {
            if (column.IsMatch(entry)) {
                return true;
            }
        }
        return false;
    }
}
