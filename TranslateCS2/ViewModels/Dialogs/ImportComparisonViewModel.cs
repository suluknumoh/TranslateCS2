using System;

using Prism.Mvvm;
using Prism.Services.Dialogs;

using TranslateCS2.Controls.Imports;
using TranslateCS2.Properties.I18N;

namespace TranslateCS2.ViewModels.Dialogs;
internal class ImportComparisonViewModel : BindableBase, IDialogAware {
    public static string ContextName { get; } = "context";

    public string Title => I18NImport.ComparisonDialogTitle;

    public event Action<IDialogResult>? RequestClose;
    public ComparisonDataGridContext? CDGContext { get; private set; }

    public bool CanCloseDialog() {
        return true;
    }

    public void OnDialogClosed() {
        this.CDGContext = null;
    }

    public void OnDialogOpened(IDialogParameters parameters) {
        parameters.TryGetValue(ContextName, out ComparisonDataGridContext context);
        this.CDGContext = context;
        this.RaisePropertyChanged(nameof(this.CDGContext));
        this.CDGContext.Raiser();
    }
}
