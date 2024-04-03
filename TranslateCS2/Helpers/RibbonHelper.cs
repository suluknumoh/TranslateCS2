using System.Collections;
using System.Drawing;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace TranslateCS2.Helpers;
internal class RibbonHelper {
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
}
