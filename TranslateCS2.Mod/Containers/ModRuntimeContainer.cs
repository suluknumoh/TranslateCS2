using Colossal.Logging;
using Colossal.PSI.Environment;

using Game.SceneFlow;

using TranslateCS2.Inf;

namespace TranslateCS2.Mod.Containers;
internal class ModRuntimeContainer : AModRuntimeContainer {
    public ModRuntimeContainer(GameManager gameManager, ILog logger) : base(gameManager,
                                                                            logger,
                                                                            new Paths(true,
                                                                                            EnvPath.kStreamingDataPath,
                                                                                            EnvPath.kUserDataPath)) { }
}
