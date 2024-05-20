using Colossal.Localization;

using Game.SceneFlow;
using Game.Settings;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Containers;
public abstract class AModRuntimeContainer : IModRuntimeContainer {
    private readonly GameManager? gameManager;
    public Paths Paths { get; }
    public IMyLogger Logger { get; }
    public ErrorMessages ErrorMessages { get; }
    public Locales Locales { get; }
    public MyLanguages Languages { get; }
    public DropDownItems DropDownItems { get; }


    public LocalizationManager? LocManager => this.gameManager?.localizationManager;
    public InterfaceSettings? IntSetting => this.gameManager?.settings.userInterface;


    protected AModRuntimeContainer(GameManager gameManager,
                                   IMyLogProvider logProvider,
                                   Paths paths) {
        this.gameManager = gameManager;
        this.Logger = new ModLogger(logProvider);
        this.DropDownItems = new DropDownItems();
        this.Paths = paths;
        // the following need the Paths to be initialized!
        this.Locales = new Locales(this);
        this.ErrorMessages = new ErrorMessages(this);
        this.Languages = new MyLanguages(this);
    }
    protected AModRuntimeContainer(IMyLogProvider logProvider, Paths paths) {
        this.Logger = new ModLogger(logProvider);
        this.DropDownItems = new DropDownItems();
        this.Paths = paths;
        // the following need the Paths to be initialized!
        this.Locales = new Locales(this);
        this.ErrorMessages = new ErrorMessages(this);
        this.Languages = new MyLanguages(this);
    }
}
