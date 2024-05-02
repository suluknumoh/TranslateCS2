using System.Windows.Controls;
using System.Windows.Input;

using TranslateCS2.Core.Helpers;

namespace TranslateCS2.Views.Works;

public partial class CreditsView : ContentControl {
    public CreditsView() {
        this.InitializeComponent();
    }
    public void OpenURLCommandAction(object? sender, ExecutedRoutedEventArgs args) {
        if (args.Parameter is string url) {
            URLHelper.Open(url);
        }
    }
}
