using Prism.Ioc;
using Prism.Modularity;

using TranslateCS2.Core;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Properties;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Edits.ViewModels;
using TranslateCS2.Edits.ViewModels.Dialogs;
using TranslateCS2.Edits.Views;
using TranslateCS2.Edits.Views.Dialogs;
using TranslateCS2.TextSearch;

namespace TranslateCS2.Edits;
[ModuleDependency(nameof(CoreModule))]
[ModuleDependency(nameof(TextSearchModule))]
public class EditsModule : IModule {
    public void OnInitialized(IContainerProvider containerProvider) {
        IViewConfigurations viewConfigurations = containerProvider.Resolve<IViewConfigurations>();
        ITranslationSessionManager translationSessionManager = containerProvider.Resolve<ITranslationSessionManager>();
        viewConfigurations.Add(IViewConfiguration.Create<EditDefaultView, EditDefaultViewModel>(I18NRibbon.Edit, ImageResources.translate, false, false, translationSessionManager));
        viewConfigurations.Add(IViewConfiguration.Create<EditOccurancesView, EditOccurancesViewModel>(I18NRibbon.EditByOccurances, ImageResources.translate, false, false, translationSessionManager));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {
        {
            // configure dialogs
            containerRegistry.RegisterDialog<EditEntryLargeView, EditEntryLargeViewModel>(nameof(EditEntryLargeView));
        }
    }
}
