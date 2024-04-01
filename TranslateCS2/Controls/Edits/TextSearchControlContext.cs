using System;
using System.Windows;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;

namespace TranslateCS2.Controls.Edits;
internal class TextSearchControlContext : BindableBase {
    public event Action? OnSearch;
    public bool IsKeyVisible { get; }


    private string? _SearchString;
    public string? SearchString {
        get => this._SearchString;
        set => this.SetProperty(ref this._SearchString, value);
    }


    private bool _IsKey;
    public bool IsKey {
        get => this._IsKey;
        set => this.SetProperty(ref this._IsKey, value, this.Invoke);
    }


    private bool _IsEnglishValue = true;
    public bool IsEnglishValue {
        get => this._IsEnglishValue;
        set => this.SetProperty(ref this._IsEnglishValue, value, this.Invoke);
    }


    private bool _IsMergeValue = true;
    public bool IsMergeValue {
        get => this._IsMergeValue;
        set => this.SetProperty(ref this._IsMergeValue, value, this.Invoke);
    }


    private bool _IsTranslation;
    public bool IsTranslation {
        get => this._IsTranslation;
        set => this.SetProperty(ref this._IsTranslation, value, this.Invoke);
    }


    public DelegateCommand<RoutedEventArgs> SearchCommand { get; }

    public TextSearchControlContext(bool isKeyVisible) {
        this.IsKeyVisible = isKeyVisible;
        this._IsKey = this.IsKeyVisible;
        this.SearchCommand = new DelegateCommand<RoutedEventArgs>(this.CommandAction);
    }

    private void CommandAction(RoutedEventArgs args) {
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

    private void Invoke() {
        OnSearch?.Invoke();
    }
}
