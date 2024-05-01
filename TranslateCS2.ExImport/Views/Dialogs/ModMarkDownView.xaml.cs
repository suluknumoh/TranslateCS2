using System.Windows.Controls;
using System.Windows.Input;

using TranslateCS2.Core.Helpers;

namespace TranslateCS2.ExImport.Views.Dialogs;
/// <summary>
///     Interaktionslogik für ImportComparisonView.xaml
/// </summary>
public partial class ModMarkDownView : ContentControl {
    public ModMarkDownView() {
        this.InitializeComponent();
    }
    public void OpenURLCommandAction(object? sender, ExecutedRoutedEventArgs args) {
        if (args.Parameter is string url) {
            URLHelper.Open(url);
        }
    }
}
