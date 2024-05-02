using System.Collections.Generic;

using Prism.Modularity;

namespace TranslateCS2.Core.Translators.Modules;

/// <summary>
///     marker <see langword="interface"/>
///     <br/>
///     <br/>
///     extend <see cref="ATranslatorModule"/>!!!
///     <br/>
///     <br/>
///     <seealso href="https://docs.prismlibrary.com/docs/modularity/index.html"/>
///     <br/>
///     <seealso href="https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Code"/>
///     <br/>
///     <seealso href="https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Directory"/>
/// </summary>
public interface ITranslatorModule : IModule {
    /// <summary>
    ///     collects <see cref="ITranslator"/>s
    /// </summary>
    List<ITranslator> Translators { get; }
}
