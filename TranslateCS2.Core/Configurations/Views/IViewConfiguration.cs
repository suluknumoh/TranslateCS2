using System;
using System.Drawing;
using System.Windows.Controls.Ribbon;

using Prism.Ioc;

using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Core.Configurations.Views;
public interface IViewConfiguration {
    RibbonTab Tab { get; }
    RibbonToggleButton NavToggleButton { get; }
    string NavigationTarget { get; }
    Type View { get; }
    Type ViewModel { get; }
    string Name { get; }
    void RegisterForNavigation(IContainerRegistry containerRegistry);
    public static IViewConfiguration Create<V, VM>(string label,
                                                   Bitmap image,
                                                   bool isUseableWithoutSessions,
                                                   bool isUseableWithDatabaseErrors,
                                                   ITranslationSessionManager? translationSessionManager = null) {
        return new ViewConfiguration<V, VM>(label,
                                            image,
                                            isUseableWithoutSessions,
                                            isUseableWithDatabaseErrors,
                                            translationSessionManager);
    }
}
