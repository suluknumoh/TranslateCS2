using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Configurations;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties.I18N;

namespace TranslateCS2.Controls.Sessions;
internal class NewEditSessionControlContext : BindableBase, INavigationAware {
    private readonly IRegionManager _regionManager;
    private bool _isLoaded = false;
    private bool _isEdit = false;

    public delegate void CallBackAfter();

    private CallBackAfter _callbackEnd;


    private string? _ActionString;
    public string? ActionString {
        get => this._ActionString;
        set => this.SetProperty(ref this._ActionString, value);
    }


    public ObservableCollection<string> Merges { get; } = [];

    public ObservableCollection<string> Overwrites { get; } = [];

    private BindingGroup? _newSessionBindingGroup;


    private TranslationSession? _NewSession;
    public TranslationSession? NewSession {
        get => this._NewSession;
        set => this.SetProperty(ref this._NewSession, value);
    }


    public DelegateCommand<RoutedEventArgs> CreateNewTranslationSessionGridLoaded { get; }
    public DelegateCommand Save { get; }
    public DelegateCommand Cancel { get; }
    public TranslationSessionManager SessionManager { get; }

    public NewEditSessionControlContext(IRegionManager regionManager,
                                        TranslationSessionManager translationSessionManager) {
        this._regionManager = regionManager;
        this.CreateNewTranslationSessionGridLoaded = new DelegateCommand<RoutedEventArgs>(this.CreateNewTranslationSessionGridLoadedAction);
        this.Save = new DelegateCommand(this.SaveAction);
        this.Cancel = new DelegateCommand(this.CancelAction);
        this.SessionManager = translationSessionManager;
        this.InitMergesOverwrites();
    }

    private void CancelAction() {
        this._callbackEnd?.Invoke();
        string? regionName = AppConfigurationManager.AppNewEditSessionRegion;
        this._regionManager.Regions[regionName].RemoveAll();
    }

    private void InitMergesOverwrites() {
        IEnumerable<FileInfo> localizationFiles = this.SessionManager.LocalizationFiles;
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
        if (this._newSessionBindingGroup.CommitEdit()) {
            if (this._isEdit) {
                this.SessionManager.Update(this.NewSession);
            } else {
                this.SessionManager.Insert(this.NewSession);
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
        this._isEdit = SessionActions.Edit == action;
        this._callbackEnd = navigationContext.Parameters.GetValue<CallBackAfter>(nameof(CallBackAfter));
        if (!this._isLoaded) {
            return;
        }
        this.InitNewSession();
    }

    private void InitNewSession() {
        this._newSessionBindingGroup.CancelEdit();
        if (this._isEdit) {
            this.NewSession = this.SessionManager.CurrentTranslationSession;
            this.ActionString = I18NSessions.DoEdit.Replace("\r\n", " ");
        } else {
            this.NewSession = new TranslationSession();
            this.ActionString = I18NSessions.DoCreate.Replace("\r\n", " ");
        }
        this._newSessionBindingGroup.BeginEdit();
    }
}
