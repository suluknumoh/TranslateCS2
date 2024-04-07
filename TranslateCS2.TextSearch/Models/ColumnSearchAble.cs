using System;

using Prism.Mvvm;

namespace TranslateCS2.TextSearch.Models;
public class ColumnSearchAble<T> : BindableBase {
    public delegate bool MatchesColumn(T parameter);

    public event Action? OnIsCheckedChange;
    public MatchesColumn? Matcher { get; set; }

    private bool _IsChecked;
    public bool IsChecked {
        get => this._IsChecked;
        set => this.SetProperty(ref this._IsChecked, value, OnIsCheckedChange);
    }


    public string Name { get; }

    public string ToolTip { get; }

    public ColumnSearchAble(string name, string tooltip) {
        this.Name = name;
        this.ToolTip = tooltip;
    }

    public bool IsMatch(T param) {
        if (this.Matcher is null) {
            return false;
        }
        return this.IsChecked && this.Matcher.Invoke(param);
    }
}
