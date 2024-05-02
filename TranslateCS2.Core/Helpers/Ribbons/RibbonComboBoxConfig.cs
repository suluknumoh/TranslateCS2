using System.Collections.Generic;
using System.Windows;

namespace TranslateCS2.Core.Helpers.Ribbons;
public class RibbonComboBoxConfig {
    public string Label { get; }
    public RoutedPropertyChangedEventHandler<object>? OnSelectionChanged { get; set; }
    public object? SelectedItem { get; set; }
    public string? DisplayMemberPath { get; set; }
    public IList<RibbonComboBoxItems> ItemsList { get; } = [];
    public RibbonComboBoxConfig(string label) {
        this.Label = label;
    }
}
