using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Sessions.Models;
using TranslateCS2.Sessions.Properties.I18N;

namespace TranslateCS2.Sessions.Controls;
internal class NewEditSessionControlContext : BindableBase, INavigationAware {
    private readonly IRegionManager _regionManager;
    private bool _isLoaded = false;


    private bool _isEdit = false;
    public bool IsEdit {
        get => this._isEdit;
        set => this.SetProperty(ref this._isEdit, value);
    }


    public delegate void CallBackAfter();

    private CallBackAfter? _callbackEnd;


    private string? _ActionString;
    public string? ActionString {
        get => this._ActionString;
        set => this.SetProperty(ref this._ActionString, value);
    }


    public ObservableCollection<string> Merges { get; } = [];

    public ObservableCollection<string> Overwrites { get; } = [];

    private BindingGroup? _newSessionBindingGroup;


    private ITranslationSession? _NewSession;
    public ITranslationSession? Session {
        get => this._NewSession;
        set => this.SetProperty(ref this._NewSession, value);
    }


    public DelegateCommand<RoutedEventArgs> CreateNewTranslationSessionGridLoaded { get; }
    public DelegateCommand FileComboBoxSelectionChangedCommand { get; }
    public DelegateCommand Save { get; }
    public DelegateCommand Cancel { get; }
    public ITranslationSessionManager SessionManager { get; }

    public NewEditSessionControlContext(IRegionManager regionManager,
                                        ITranslationSessionManager translationSessionManager) {
        this._regionManager = regionManager;
        this.CreateNewTranslationSessionGridLoaded = new DelegateCommand<RoutedEventArgs>(this.CreateNewTranslationSessionGridLoadedAction);
        this.FileComboBoxSelectionChangedCommand = new DelegateCommand(this.FileComboBoxSelectionChangedCommandAction);
        this.Save = new DelegateCommand(this.SaveAction);
        this.Cancel = new DelegateCommand(this.CancelAction);
        this.SessionManager = translationSessionManager;
        this.InitMergesOverwrites();
    }

    private void FileComboBoxSelectionChangedCommandAction() {
        this._newSessionBindingGroup?.UpdateSources();
    }

    private void CancelAction() {
        this._callbackEnd?.Invoke();
        string? regionName = AppConfigurationManager.AppNewEditSessionRegion;
        this._regionManager.Regions[regionName].RemoveAll();
    }

    private void InitMergesOverwrites() {
        IEnumerable<FileInfo> localizationFiles = this.SessionManager.LocalizationFiles;
        this.Overwrites.Add(AppConfigurationManager.NoneOverwrite);
        foreach (FileInfo file in localizationFiles) {
            this.Merges.Add(file.Name);
            if (file.Name != AppConfigurationManager.LeadingLocFileName) {
                this.Overwrites.Add(file.Name);
            }
        }
    }

    private void CreateNewTranslationSessionGridLoadedAction(RoutedEventArgs e) {
        if (e.Source is Grid grid) {
            this._newSessionBindingGroup = grid.BindingGroup;
            this.InitNewSession();
            this._isLoaded = true;
        }
    }
    private void SaveAction() {
        if (this._newSessionBindingGroup != null && this._newSessionBindingGroup.CommitEdit()) {
            if (this.IsEdit) {
                this.SessionManager.Update(this.Session);
            } else {
                this.SessionManager.Insert(this.Session);
            }
            if (!this.SessionManager.HasDatabaseError) {
                this.CancelAction();
            } else {
                // see xaml-code
            }
        }
    }
    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        //
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        SessionActions? action = navigationContext.Parameters.GetValue<SessionActions?>(nameof(SessionActions));
        this.IsEdit = SessionActions.Edit == action;
        this._callbackEnd = navigationContext.Parameters.GetValue<CallBackAfter>(nameof(CallBackAfter));
        if (!this._isLoaded) {
            return;
        }
        this.InitNewSession();
    }

    private void InitNewSession() {
        if (this._newSessionBindingGroup != null) {
            this._newSessionBindingGroup.CancelEdit();
            if (this.IsEdit) {
                this.Session = this.SessionManager.CurrentTranslationSession;
                this.ActionString = I18NSessions.DoEdit.Replace("\r\n", " ");
            } else {
                this.Session = this.SessionManager.GetNewTranslationSession();
                this.Session.OverwriteLocalizationFileName = AppConfigurationManager.NoneOverwrite;
                this.ActionString = I18NSessions.DoCreate.Replace("\r\n", " ");
            }
            this._newSessionBindingGroup.BeginEdit();
        }
    }
}
