using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers;
using TranslateCS2.ZZZModTestLib.Containers.Items.Unitys;
using TranslateCS2.ZZZTestLib;

namespace TranslateCS2.ZZZModTestLib.Containers;
internal class ModTestRuntimeContainer : ModRuntimeContainer {
    public TestLocManager TestLocManager => (TestLocManager) this.LocManager;
    public TestIntSettings TestIntSettings => (TestIntSettings) this.IntSettings;
    public ModTestRuntimeContainer(IMyLogProvider logProvider,
                                   IIndexCountsProvider? indexCountsProvider = null,
                                   string? userDataPath = null) : base(logProvider,
                                                                      new TestMod(),
                                                                      new TestLocManager(),
                                                                      new TestIntSettings(),
                                                                      indexCountsProvider ?? TestIndexCountsProvider.WithEmptyContent(),
                                                                      new Paths(true,
                                                                                      CS2PathsHelper.GetStreamingDataPathFromProps(),
                                                                                      userDataPath)) { }
}
