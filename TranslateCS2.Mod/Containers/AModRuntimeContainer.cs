using Colossal.Localization;
using Colossal.Logging;

using Game.SceneFlow;
using Game.Settings;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;

namespace TranslateCS2.Mod.Containers;
public abstract class AModRuntimeContainer : IModRuntimeContainer {
    private readonly GameManager? gameManager;
    public ILog? Logger { get; }
    public Paths Paths { get; }
    public ErrorMessages ErrorMessages { get; }
    public Locales Locales { get; }
    public MyLanguages Languages { get; }
    public DropDownItems DropDownItems { get; }


    public virtual LocalizationManager? LocManager => this.gameManager?.localizationManager;
    public virtual InterfaceSettings? IntSetting => this.gameManager?.settings.userInterface;


    protected AModRuntimeContainer(GameManager gameManager,
                                   ILog? logger,
                                   Paths paths) {
        this.gameManager = gameManager;
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
