using System;

using Microsoft.Win32;

using TranslateCS2.Configurations;
using TranslateCS2.Properties;

namespace TranslateCS2.Helpers;

internal static class ImExportDialogHelper {
    public static string? ShowSaveFileDialog(string? path) {
        SaveFileDialog dialog = new SaveFileDialog {
            Title = I18N.StringExportDialogTitle,
            CheckPathExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = AppConfigurationManager.ImExportDefaultFileName,
            Filter = AppConfigurationManager.ImExportFilter
        };
        if (path is not null) {
            dialog.InitialDirectory = path;
        }
        bool ok = dialog.ShowDialog() ?? true;
        if (ok) {
            return dialog.FileName;
        }
        return null;
    }
    public static string? OpenFileDialog(string? path) {
        OpenFileDialog dialog = new OpenFileDialog {
            Title = I18N.StringImportDialogTitle,
            Multiselect = false,
            CheckPathExists = true,
            CheckFileExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = AppConfigurationManager.ImExportDefaultFileName,
            Filter = AppConfigurationManager.ImExportFilter
        };
        if (path is not null) {
            dialog.InitialDirectory = path;
        }
        bool ok = dialog.ShowDialog() ?? true;
        if (ok) {
            return dialog.FileName;
        }
        return null;
    }
}
