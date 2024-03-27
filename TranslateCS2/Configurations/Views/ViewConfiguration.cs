using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;

using Prism.Ioc;

using TranslateCS2.Helpers;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties;

namespace TranslateCS2.Configurations.Views;
internal class ViewConfiguration<V, M> : IViewConfiguration {
    public string Label { get; }
    public Type View { get; }

    public Type ViewModel { get; }

    public string Name => this.View.FullName ?? this.View.Name;

    public RibbonTab Tab { get; }

    public RibbonToggleButton NavToggleButton { get; }

    public string NavigationTarget => this.Name;

    public ViewConfiguration(string label, Bitmap image, bool isViewUseAble, TranslationSessionManager? translationSessionManager = null) {
        this.View = typeof(V);
        this.ViewModel = typeof(M);
        this.Label = label;
        this.NavToggleButton = new RibbonToggleButton {
            Label = label,
            Cursor = Cursors.Hand,
            LargeImageSource = ImageHelper.GetBitmapImage(image)
        };
        this.Tab = new RibbonTab {
            Header = label
        };
        if (isViewUseAble && translationSessionManager != null) {
            Binding binding = new Binding(nameof(translationSessionManager.HasTranslationSessions)) {
                Source = translationSessionManager
            };
            this.Tab.SetBinding(UIElement.IsEnabledProperty, binding);
        } else if (!isViewUseAble) {
            this.Tab.IsEnabled = false;
        }
        RibbonGroup navGroup = new RibbonGroup {
            Header = I18N.StringNavigationCap
        };
        this.Tab.Items.Add(navGroup);
        navGroup.Items.Add(this.NavToggleButton);
    }

    public void RegisterForNavigation(IContainerRegistry containerRegistry) {
        containerRegistry.RegisterForNavigation<V, M>(this.Name);
    }
}
