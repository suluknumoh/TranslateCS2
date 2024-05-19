using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Prism.Mvvm;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Models.Localizations;

namespace TranslateCS2.Core.Sessions;
// https://learn.microsoft.com/en-us/archive/msdn-magazine/2010/june/msdn-magazine-input-validation-enforcing-complex-business-data-rules-with-wpf
// https://blog.magnusmontin.net/2013/08/26/data-validation-in-wpf/
internal class TranslationSession : BindableBase, ITranslationSession, IEquatable<ITranslationSession?> {
    private long _ID;
    public long ID {
        get => this._ID;
        set => this.SetProperty(ref this._ID, value, this.ChangeRefresh);
    }
    private string? _Name;
    public string? Name {
        get => this._Name;
        set => this.SetProperty(ref this._Name, value, this.ChangeRefresh);
    }
    /// <summary>
    ///     local <see cref="DateTime"/> !!!
    ///     <br/>
    ///     gets converted to universal time within <see cref="TranslationsDB" />
    /// </summary>
    public DateTime Started { get; set; }


    private DateTime _LastEdited;
    /// <summary>
    ///     local <see cref="DateTime"/>!!!
    ///     <br/>
    ///     gets converted to universal time within <see cref="TranslationsDB"/>
    /// </summary>
    public DateTime LastEdited {
        get => this._LastEdited;
        set => this.SetProperty(ref this._LastEdited, value, this.ChangeRefresh);
    }


    private string? _MergeLocalizationFileName = AppConfigurationManager.LeadingLocFileName;
    public string? MergeLocalizationFileName {
        get => this._MergeLocalizationFileName;
        set => this.SetProperty(ref this._MergeLocalizationFileName, value);
    }



    private string? _LocNameEnglish;
    public string? LocNameEnglish {
        get => this._LocNameEnglish;
        set => this.SetProperty(ref this._LocNameEnglish, value);
    }


    private string? _LocName;
    public string? LocName {
        get => this._LocName;
        set => this.SetProperty(ref this._LocName, value);
    }
    public ObservableCollection<AppLocFileEntry> Localizations { get; } = [];


    private string? _DisplayName;
    public string? DisplayName {
        get => this._DisplayName;
        set => this.SetProperty(ref this._DisplayName, value);
    }


    public TranslationSession() {
        this.Started = DateTime.Now;
        this.LastEdited = this.Started;
    }

    private void ChangeRefresh() {
        this.DisplayName = $"{nameof(this.ID)}: {this.ID} - {nameof(this.Name)}: {this.Name} - {nameof(this.Started)}: {this.Started} - {nameof(this.LastEdited)}: {this.LastEdited}";
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as TranslationSession);
    }

    public bool Equals(ITranslationSession? other) {
        return other is not null &&
               this.ID == other.ID;
    }

    public override int GetHashCode() {
        return HashCode.Combine(this.ID);
    }

    public void UpdateWith(ITranslationSession session) {
        this.Name = session.Name;
        this.Started = session.Started;
        this.LastEdited = session.LastEdited;
        this.MergeLocalizationFileName = session.MergeLocalizationFileName;
        this.LocNameEnglish = session.LocNameEnglish;
        this.LocName = session.LocName;
        this.DisplayName = session.DisplayName;
    }

    public static bool operator ==(TranslationSession? left, TranslationSession? right) {
        return EqualityComparer<TranslationSession>.Default.Equals(left, right);
    }

    public static bool operator !=(TranslationSession? left, TranslationSession? right) {
        return !(left == right);
    }
}
