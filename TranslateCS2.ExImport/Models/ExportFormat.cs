using System;
using System.Collections.Generic;

using TranslateCS2.ExImport.Properties.I18N;

namespace TranslateCS2.ExImport.Models;
internal class ExportFormat : IEquatable<ExportFormat?> {
    public ExportFormats Format { get; }
    public string Name { get; }
    public bool RequiresDestinationSelection { get; }
    public string ToolTip { get; }
    public ExportFormat(string name,
                        ExportFormats format,
                        bool requiresDestinationSelection,
                        string toolTip) {
        this.Name = name;
        this.Format = format;
        this.RequiresDestinationSelection = requiresDestinationSelection;
        this.ToolTip = toolTip;
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as ExportFormat);
    }

    public bool Equals(ExportFormat? other) {
        return other is not null &&
               this.Name == other.Name;
    }

    public override int GetHashCode() {
        return HashCode.Combine(this.Name);
    }

    public static bool operator ==(ExportFormat? left, ExportFormat? right) {
        return EqualityComparer<ExportFormat>.Default.Equals(left, right);
    }

    public static bool operator !=(ExportFormat? left, ExportFormat? right) {
        return !(left == right);
    }

    public static ExportFormat DirectOverwrite() {
        return new ExportFormat("direct-overwrite",
                                ExportFormats.Direct,
                                false,
                                I18NExport.ToolTipExportFormatDirectOverwrite);
    }
}
