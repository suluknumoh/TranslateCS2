using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;

using Prism.Ioc;

using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Core.Configurations.Views;
internal class ViewConfiguration<V, VM> : IViewConfiguration {
    public string Label { get; }
    public Type View { get; }

    public Type ViewModel { get; }

    public string Name => this.View.FullName ?? this.View.Name;

    public RibbonTab Tab { get; }

    public RibbonToggleButton NavToggleButton { get; }

    public string NavigationTarget => this.Name;

    public ViewConfiguration(string label,
                              Bitmap image,
                              bool isUseableWithoutSessions,
                              bool isUseableWithDatabaseErrors,
                              ITranslationSessionManager? translationSessionManager = null) {
        this.View = typeof(V);
        this.ViewModel = typeof(VM);
        this.Label = label;
        this.NavToggleButton = RibbonHelper.CreateRibbonToggleButton(label,
                                                                     image,
                                                                     false);
        this.Tab = new RibbonTab {
            Header = label
        };
        if (isUseableWithoutSessions && isUseableWithDatabaseErrors) {
            this.Tab.IsEnabled = true;
        } else if (translationSessionManager != null) {
            MultiBinding multiBinding = new MultiBinding {
                NotifyOnSourceUpdated = true,
                Converter = new MyMultiValueConverter()
            };
            if (!isUseableWithDatabaseErrors) {
                Binding binding = new Binding(nameof(translationSessionManager.HasNoDatabaseError)) {
                    Source = translationSessionManager,
                    NotifyOnSourceUpdated = true
                };
                multiBinding.Bindings.Add(binding);
                this.Tab.IsEnabled = !translationSessionManager.HasNoDatabaseError;
            }
            if (!isUseableWithoutSessions) {
                Binding binding = new Binding(nameof(translationSessionManager.HasTranslationSessions)) {
                    Source = translationSessionManager,
                    NotifyOnSourceUpdated = true
                };
                multiBinding.Bindings.Add(binding);
                this.Tab.IsEnabled = translationSessionManager.HasTranslationSessions;
            }
            this.Tab.SetBinding(UIElement.IsEnabledProperty, multiBinding);
        } else {
            this.Tab.IsEnabled = false;
        }
        RibbonGroup navGroup = new RibbonGroup {
            Header = I18NRibbon.Navigation,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
        };
        this.Tab.Items.Add(navGroup);
        navGroup.Items.Add(this.NavToggleButton);
    }

    public void RegisterForNavigation(IContainerRegistry containerRegistry) {
        containerRegistry.RegisterForNavigation<V, VM>(this.Name);
    }

    private class MyMultiValueConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            bool[] bools = Array.ConvertAll(values, val => (bool) val);
            return bools.All(b => b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            // unsed
            return [];
        }
    }
}
