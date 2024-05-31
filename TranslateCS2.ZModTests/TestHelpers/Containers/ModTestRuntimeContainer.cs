using Game.Modding;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.ZModTests.TestHelpers.Containers.Items.Unitys;
using TranslateCS2.ZZZTestLib;

namespace TranslateCS2.ZModTests.TestHelpers.Containers;
internal class ModTestRuntimeContainer : ModRuntimeContainer {
    public TestLocManagerProvider TestLocManager => (TestLocManagerProvider) this.LocManager.Provider;
    public TestIntSettings TestIntSettings => (TestIntSettings) this.IntSettings;
    private ModTestRuntimeContainer(IMyLogProvider logProvider,
                                    IMod mod,
                                    ILocManagerProvider locManagerProvider,
                                    IIntSettings intSettings,
                                    IIndexCountsProvider indexCountsProvider,
                                    Paths paths) : base(logProvider,
                                                        mod,
                                                        locManagerProvider,
                                                        intSettings,
                                                        indexCountsProvider,
                                                        paths) { }
    public static ModTestRuntimeContainer Create(IMyLogProvider logProvider,
                                                 IIndexCountsProvider? indexCountsProvider = null,
                                                 string? userDataPath = null) {
        IMod mod = new TestMod();
        ILocManagerProvider locManager = new TestLocManagerProvider();
        IIntSettings intSettings = new TestIntSettings();
        IIndexCountsProvider indexCountsProviderLocal = indexCountsProvider ?? TestIndexCountsProvider.WithEmptyContent();
        Paths paths = new Paths(true,
                                CS2PathsHelper.GetStreamingDataPathFromProps(),
                                userDataPath);
        return new ModTestRuntimeContainer(logProvider,
                                           mod,
                                           locManager,
                                           intSettings,
                                           indexCountsProviderLocal,
                                           paths);
    }
}
