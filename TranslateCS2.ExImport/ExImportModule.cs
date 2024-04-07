using Prism.Ioc;
using Prism.Modularity;

using TranslateCS2.Core;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Properties;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Sessions;
using TranslateCS2.ExImport.Controls.Exports;
using TranslateCS2.ExImport.Controls.Imports;
using TranslateCS2.ExImport.Services;
using TranslateCS2.ExImport.ViewModels;
using TranslateCS2.ExImport.ViewModels.Dialogs;
using TranslateCS2.ExImport.Views;
using TranslateCS2.ExImport.Views.Dialogs;
using TranslateCS2.TextSearch;

namespace TranslateCS2.ExImport;
[ModuleDependency(nameof(CoreModule))]
[ModuleDependency(nameof(TextSearchModule))]
public class ExImportModule : IModule {
    public void OnInitialized(IContainerProvider containerProvider) {
        IViewConfigurations viewConfigurations = containerProvider.Resolve<IViewConfigurations>();
        ITranslationSessionManager translationSessionManager = containerProvider.Resolve<ITranslationSessionManager>();
        viewConfigurations.Add(IViewConfiguration.Create<ExImPortView, ExImPortViewModel>(I18NRibbon.ExImport, ImageResources.database_multiple, false, false, translationSessionManager));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {
        containerRegistry.Register<JSONService>();
        containerRegistry.Register<ExImportService>();
        {
            // configure controls
            containerRegistry.RegisterForNavigation<ExportControl, ExportControlContext>(nameof(ExportControl));
            containerRegistry.RegisterForNavigation<ImportControl, ImportControlContext>(nameof(ImportControl));
            containerRegistry.RegisterForNavigation<ComparisonDataGrid>(nameof(ComparisonDataGrid));
        }
        {
            // configure dialogs
            containerRegistry.RegisterDialog<ImportComparisonView, ImportComparisonViewModel>(nameof(ImportComparisonView));
        }
    }
}
