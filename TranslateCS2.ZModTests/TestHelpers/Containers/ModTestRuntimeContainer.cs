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
    public TestLocManagerProvider TestLocManagerProvider => (TestLocManagerProvider) this.LocManager.Provider;
    public TestIntSettingsProvider TestIntSettings => (TestIntSettingsProvider) this.IntSettings.Provider;
    private ModTestRuntimeContainer(IMyLogProvider logProvider,
                                    IMod mod,
                                    ILocManagerProvider locManagerProvider,
                                    IIntSettingsProvider intSettings,
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
        IIntSettingsProvider intSettings = new TestIntSettingsProvider();
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
