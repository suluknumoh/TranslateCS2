using System.Collections.Generic;

using Prism.Modularity;

namespace TranslateCS2.Core.Translators.Modules;

/// <summary>
///     marker <see langword="interface"/>
///     <br/>
///     <br/>
///     extend <see cref="ATranslatorModule"/>!!!
/// </summary>
public interface ITranslatorModule : IModule {
    /// <summary>
    ///     collects <see cref="ITranslator"/>s
    /// </summary>
    List<ITranslator> Translators { get; }
}
