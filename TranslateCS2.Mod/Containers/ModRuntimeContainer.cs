using Game.Modding;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Containers;
public class ModRuntimeContainer : IModRuntimeContainer {
    public Paths Paths { get; }
    public IMyLogger Logger { get; }
    public ErrorMessages ErrorMessages { get; }
    public Locales Locales { get; }
    public MyLanguages Languages { get; }
    public DropDownItems DropDownItems { get; }
    public ILocManager LocManager { get; }
    public IIntSettings IntSettings { get; }
    public IIndexCountsProvider IndexCountsProvider { get; }
    public IMod Mod { get; }

    public ModRuntimeContainer(IMyLogProvider logProvider,
                               IMod mod,
                               ILocManager locManager,
                               IIntSettings intSettings,
                               IIndexCountsProvider indexCountsProvider,
                               Paths paths) {
        this.Logger = new ModLogger(logProvider);
        this.Mod = mod;
        this.LocManager = locManager;
        this.IntSettings = intSettings;
        this.IndexCountsProvider = indexCountsProvider;
        this.DropDownItems = new DropDownItems();
        this.Paths = paths;
        // the following need the Paths to be initialized!
        this.Locales = new Locales(this);
        this.ErrorMessages = new ErrorMessages(this);
        this.Languages = new MyLanguages(this);
    }
}
