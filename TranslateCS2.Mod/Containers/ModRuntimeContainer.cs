using System;

using Game.Modding;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Containers;
internal class ModRuntimeContainer : IModRuntimeContainer {
    private readonly PerformanceMeasurement performanceMeasurement;
    public Paths Paths { get; }
    public IMyLogger Logger { get; }
    public ErrorMessages ErrorMessages { get; }
    public Locales Locales { get; }
    public MyLanguages Languages { get; }
    public DropDownItems DropDownItems { get; }
    public LocManager LocManager { get; }
    public IIntSettings IntSettings { get; }
    public IIndexCountsProvider IndexCountsProvider { get; }
    public IMod Mod { get; }
    public ModSettings Settings { get; }
    public ModSettingsLocale SettingsLocale { get; }

    public ModRuntimeContainer(IMyLogProvider logProvider,
                               IMod mod,
                               ILocManagerProvider locManagerProvider,
                               IIntSettings intSettings,
                               IIndexCountsProvider indexCountsProvider,
                               Paths paths) {
        // dont change init-order!!!
        this.Logger = new ModLogger(logProvider);
        this.performanceMeasurement = new PerformanceMeasurement(this);
        this.performanceMeasurement.Start();
        this.Mod = mod;
        this.LocManager = new LocManager(locManagerProvider, this);
        this.IntSettings = intSettings;
        this.IndexCountsProvider = indexCountsProvider;
        this.DropDownItems = new DropDownItems();
        this.Paths = paths;
        // the following need the Paths to be initialized!
        this.Locales = new Locales(this);
        this.ErrorMessages = new ErrorMessages(this);
        this.Languages = new MyLanguages(this);
        this.Settings = new ModSettings(this);
        this.SettingsLocale = new ModSettingsLocale(this);
    }

    public void Init(Action<string, object, object?>? loadSettings = null,
                     bool register = false) {
        this.Languages.Init();
        this.SettingsLocale.Init();
        if (register) {
            this.Settings.RegisterInOptionsUI();
        }
        // settings have to be loaded after files are read and loaded
        loadSettings?.Invoke(ModConstants.Name,
                             this.Settings,
                             null);
        this.LocManager.Provider.AddSource(this.LocManager.FallbackLocaleId,
                                           this.SettingsLocale);
        this.Settings.HandleLocaleOnLoad();
        this.performanceMeasurement.Stop();
    }

    public void Dispose(bool unregister = false) {
        this.Logger.LogInfo(this.GetType(), nameof(Dispose));
        if (unregister) {
            this.Settings.UnregisterInOptionsUI();
        }
        this.Settings.HandleLocaleOnUnLoad();
    }
}
