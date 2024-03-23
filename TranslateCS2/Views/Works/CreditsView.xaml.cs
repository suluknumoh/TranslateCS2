using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace TranslateCS2.Views.Works;

public partial class CreditsView : ContentControl {
    public CreditsView() {
        this.InitializeComponent();
    }
    public void OpenURLCommandAction(object? sender, ExecutedRoutedEventArgs args) {
        if (args.Parameter is string url) {
            Process.Start(new ProcessStartInfo(url) {
                UseShellExecute = true
            });
        }
    }
}
