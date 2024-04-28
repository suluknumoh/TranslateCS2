using System;
using System.IO;
using System.Reflection;

using Markdig;
using Markdig.Wpf;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using TranslateCS2.ExImport.Properties.I18N;

namespace TranslateCS2.ExImport.ViewModels.Dialogs;
internal class ModReadMeViewModel : BindableBase, IDialogAware {
    public string? Doc { get; private set; }
    public MarkdownPipeline? Pipeline { get; private set; }
    public DelegateCommand OnLoadedCommand { get; }

    public string Title => I18NExport.CaptionAdditionalInformation;

    public ModReadMeViewModel() {
        this.OnLoadedCommand = new DelegateCommand(this.OnLoadedCommandAction);
    }

    public event Action<IDialogResult>? RequestClose;

    private void OnLoadedCommandAction() {
        if (this.Doc == null) {
            this.Doc = GetReadMe();
            this.Pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
            this.RaisePropertyChanged(nameof(this.Doc));
            this.RaisePropertyChanged(nameof(this.Pipeline));
        }
    }
    private static string GetReadMe() {
        try {
            Assembly assembly = Assembly.GetCallingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream("TranslateCS2.ExImport.Assets.README.MOD.md");
            if (stream != null) {
                using StreamReader sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
        } catch {
            //
        }
        return String.Empty;
    }

    public bool CanCloseDialog() {
        return true;
    }

    public void OnDialogClosed() {
        // nothing to do
    }

    public void OnDialogOpened(IDialogParameters parameters) {
        // nothing to do
    }
}
