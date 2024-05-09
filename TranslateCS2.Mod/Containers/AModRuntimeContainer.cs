using Colossal.Localization;
using Colossal.Logging;

using Game.Settings;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;

namespace TranslateCS2.Mod.Containers;
public abstract class AModRuntimeContainer : IModRuntimeContainer {
    public ILog? Logger { get; }
    public Paths Paths { get; }
    public ErrorMessages ErrorMessages { get; }
    public Locales Locales { get; }
    public MyLanguages Languages { get; }
    public DropDownItems DropDownItems { get; }


    public virtual LocalizationManager? LocManager { get; }
    public virtual InterfaceSettings? IntSetting { get; }


    protected AModRuntimeContainer(ILog? logger, Paths paths) {
        this.Logger = logger;
        this.DropDownItems = new DropDownItems();
        this.Paths = paths;
        // the following need the Paths to be initialized!
        this.Locales = new Locales(this);
        this.ErrorMessages = new ErrorMessages(this);
        this.Languages = new MyLanguages(this);
    }
    protected AModRuntimeContainer(Paths paths) {
        this.DropDownItems = new DropDownItems();
        this.Paths = paths;
        // the following need the Paths to be initialized!
        this.Locales = new Locales(this);
        this.ErrorMessages = new ErrorMessages(this);
        this.Languages = new MyLanguages(this);
    }
}
