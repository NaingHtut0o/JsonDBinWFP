using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SmartHealthTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BrushConverter brushConverter = new BrushConverter();
#region template one
            // Create a ControlTemplate for the Button
            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));

            // Create the Border
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.Name = "alterBorder";
            border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));

            // Create ContentPresenter
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Add ContentPresenter inside the Border
            border.AppendChild(contentPresenter);

            // Set the Visual Tree of the Template
            buttonTemplate.VisualTree = border;

            // Create the Trigger for MouseOver
            Trigger hoverTrigger = new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };
            //hoverTrigger.Setters.Add(new Setter(Border.BackgroundProperty, (Brush)brushConverter.ConvertFromString("#55441E"), "alterBorder"));
            hoverTrigger.Setters.Add(new Setter
            {
                Property = Border.EffectProperty,
                Value = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 320,
                    BlurRadius = 10,
                    ShadowDepth = 3
                }
            });

            // Add the Trigger to the Template
            buttonTemplate.Triggers.Add(hoverTrigger);

            // Store the template in Application Resources
            Resources["CustomButtonTemplate"] = buttonTemplate;
            #endregion template one
        }

    }

}
