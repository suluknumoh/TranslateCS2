using System;

using Colossal.IO.AssetDatabase;

using Game.Modding;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Containers;
internal class ModRuntimeContainer : IModRuntimeContainer {
    public Paths Paths { get; }
    public IMyLogger Logger { get; }
    public ErrorMessages ErrorMessages { get; }
    public Locales Locales { get; }
    public MyLanguages Languages { get; }
    public LocManager LocManager { get; }
    public IntSettings IntSettings { get; }
    public IIndexCountsProvider IndexCountsProvider { get; }
    public IMod Mod { get; }
    public ModSettings Settings { get; }
    public ModSettingsLocale SettingsLocale { get; }
    public ModManager? ModManager { get; set; }
    public ExecutableAsset? ModAsset { get; set; }
    public IBuiltInLocaleIdProvider BuiltInLocaleIdProvider { get; }

    public ModRuntimeContainer(IMyLogProvider logProvider,
                               IMod mod,
                               ILocManagerProvider locManagerProvider,
                               IIntSettingsProvider intSettingsProvider,
                               IIndexCountsProvider indexCountsProvider,
                               IBuiltInLocaleIdProvider builtInLocaleIdProvider,
                               Paths paths) {
        // dont change init-order!!!
        this.Logger = new ModLogger(logProvider);
        this.Mod = mod;
        this.LocManager = new LocManager(this.Logger, locManagerProvider);
        this.IntSettings = new IntSettings(intSettingsProvider);
        this.IndexCountsProvider = indexCountsProvider;
        this.BuiltInLocaleIdProvider = builtInLocaleIdProvider;
        this.Paths = paths;
        // the following need the Paths to be initialized!
        this.Locales = new Locales(this);
        this.ErrorMessages = new ErrorMessages(this);
        this.Languages = new MyLanguages(this);
        this.Settings = new ModSettings(this);
        this.SettingsLocale = new ModSettingsLocale(this);
    }

    /// <summary>
    ///     inits this container
    /// </summary>
    /// <param name="loadSettings">
    ///     <see cref="Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings"/>
    ///     <br/>
    /// </param>
    /// <param name="register">
    ///     do not register in tests!!!
    ///     <br/>
    ///     it does not work
    /// </param>
    public void Init(Action<string, object, object?>? loadSettings = null,
                     bool register = false) {
        // since the whole logic changed,
        // settings no longer have to loaded after Languages are initialized
        // now they have to be loaded before Languages get initialized
        loadSettings?.Invoke(ModConstants.Name,
                             this.Settings,
                             null);
        this.Languages.Init();
        this.SettingsLocale.Init();
        if (register) {
            this.Settings.RegisterInOptionsUI();
        }
        this.LocManager.Provider.AddSource(this.LocManager.FallbackLocaleId,
                                           this.SettingsLocale);
        this.Settings.HandleLocaleOnLoad();
    }

    public void Dispose(bool unregister = false) {
        if (unregister) {
            this.Settings.UnregisterInOptionsUI();
        }
        this.Settings.HandleLocaleOnUnLoad();
    }
}
