using System.Collections;
using System.Drawing;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;

using TranslateCS2.Core.Helpers.Ribbons;
using TranslateCS2.Inf;

namespace TranslateCS2.Core.Helpers;
public class RibbonHelper {
    public static RibbonComboBox CreateComboBox(string label,
                                                IEnumerable itemsSource,
                                                RoutedPropertyChangedEventHandler<object> onSelectionChanged,
                                                object? selectedItem,
                                                string? displayMemberPath = null) {
        RibbonComboBox comboBox = new RibbonComboBox {
            Label = label,
            Cursor = Cursors.Hand
        };
        RibbonGallery ribbonGallery = new RibbonGallery();
        comboBox.Items.Add(ribbonGallery);
        RibbonGalleryCategory ribbonGalleryCategory = new RibbonGalleryCategory();
        ribbonGallery.Items.Add(ribbonGalleryCategory);
        ribbonGalleryCategory.ItemsSource = itemsSource;
        ribbonGallery.SelectionChanged += onSelectionChanged;
        if (selectedItem != null) {
            ribbonGallery.SelectedItem = selectedItem;
        }
        if (displayMemberPath != null) {
            ribbonGalleryCategory.DisplayMemberPath = displayMemberPath;
        }
        return comboBox;
    }

    public static RibbonComboBox CreateComboBox(RibbonComboBoxConfig config) {
        RibbonComboBox comboBox = new RibbonComboBox {
            Label = config.Label,
            Cursor = Cursors.Hand
        };
        RibbonGallery ribbonGallery = new RibbonGallery();
        comboBox.Items.Add(ribbonGallery);


        foreach (RibbonComboBoxItems items in config.ItemsList) {
            if (items.Items.Count == 0) {
                continue;
            }
            if (!StringHelper.IsNullOrWhiteSpaceOrEmpty(items.SeparatorLabel)) {
                RibbonSeparator ribbonSeparator = new RibbonSeparator {
                    Label = items.SeparatorLabel
                };
                ribbonGallery.Items.Add(ribbonSeparator);
            }
            RibbonGalleryCategory ribbonGalleryCategory = new RibbonGalleryCategory();
            if (config.DisplayMemberPath != null) {
                ribbonGalleryCategory.DisplayMemberPath = config.DisplayMemberPath;
            }
            ribbonGallery.Items.Add(ribbonGalleryCategory);
            foreach (object? item in items.Items) {
                ribbonGalleryCategory.Items.Add(item);
            }
        }
        if (config.OnSelectionChanged != null) {
            ribbonGallery.SelectionChanged += config.OnSelectionChanged;
        }
        if (config.SelectedItem != null) {
            ribbonGallery.SelectedItem = config.SelectedItem;
        }
        return comboBox;
    }

    public static RibbonToggleButton CreateRibbonToggleButton(string label,
                                                              Bitmap largeImageSource,
                                                              bool isChecked,
                                                              RoutedEventHandler? clickAction = null) {
        RibbonToggleButton button = new RibbonToggleButton {
            Label = label,
            LargeImageSource = ImageHelper.GetBitmapImage(largeImageSource),
            IsChecked = isChecked,
            Cursor = Cursors.Hand,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            MinWidth = 75,
            MaxWidth = 75,
            Width = 75,
        };
        if (clickAction is not null) {
            button.Click += clickAction;
        }
        return button;
    }

    public static RibbonGroup CreateRibbonGroup(string header, bool isEnabled) {
        RibbonGroup ribbonGroup = new RibbonGroup {
            Header = header,
            IsEnabled = isEnabled,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
        };
        return ribbonGroup;
    }

    public static RibbonTextBox CreateRibbonTextBox(string? label, bool isEnabled, Binding textBinding) {
        RibbonTextBox ribbonTextBox = new RibbonTextBox {
            IsEnabled = isEnabled,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        if (label is not null) {
            ribbonTextBox.Label = label;
        }
        ribbonTextBox.SetBinding(System.Windows.Controls.TextBox.TextProperty, textBinding);
        return ribbonTextBox;
    }

    public static RibbonCheckBox CreateRibbonCheckBox(string label, Binding isCheckedBinding) {
        RibbonCheckBox ribbonCheckBox = new RibbonCheckBox {
            Label = label,
            Cursor = Cursors.Hand
        };
        ribbonCheckBox.SetBinding(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, isCheckedBinding);
        return ribbonCheckBox;
    }

    public static RibbonButton CreateRibbonButton(string label,
                                                  Bitmap largeImageSource,
                                                  RoutedEventHandler? clickAction = null,
                                                  int width = 75) {
        RibbonButton button = new RibbonButton {
            Label = label,
            LargeImageSource = ImageHelper.GetBitmapImage(largeImageSource),
            Cursor = Cursors.Hand,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            MinWidth = width,
            MaxWidth = width,
            Width = width,
        };
        if (clickAction is not null) {
            button.Click += clickAction;
        }
        return button;
    }

    public static System.Windows.Shapes.Rectangle CreateInGroupSeparator() {
        System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle {
            Width = 2,
            Height = 65,
            Stroke = System.Windows.Media.Brushes.Silver,
            Margin = new Thickness(5, 0, 5, 0)
        };
        return rectangle;
    }
}
