using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SDEngine;
using Windows.UI.Popups;
using System.Threading.Tasks;
using System.Net.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StrikeDistance_WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage : Page
    {
        public LoadingPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var sb = StatusBar.GetForCurrentView();
            sb.ForegroundColor = (App.Current.Resources["StrikeDistanceForegroundBrush"] as SolidColorBrush).Color;
            (ApplicationView.GetForCurrentView()).SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            try
            {
                Engine.Memory.Temp = await Engine.GetTemperature();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(InvalidOperationException))
                    (new MessageDialog(string.Format("StrikeDistance could not connect to the weather server.\n\nStrikeDistance can and will continue, but some features that require Internet access may not work.\n\n{0}\n{1}", ex.GetType().FullName, ex.Message), "Error")).ShowAsync();
                else if (ex.GetType() == typeof(HttpRequestException))
                    (new MessageDialog(string.Format("StrikeDistance encountered an error.\n\nStrikeDistance may be able to continue, but some features that require Internet access may not work.\n\n{0}\n{1}", ex.GetType().FullName, ex.Message), "Error")).ShowAsync();
                else
                    (new MessageDialog(string.Format("StrikeDistance encountered an error.\n\nStrikeDistance may be able to continue.\n\n{0}\n{1}", ex.GetType().FullName, ex.Message), "Unexpected Error")).ShowAsync();
            }
            finally
            {
                try
                {
                    if (!Frame.Navigate(typeof(CalculatorPage)))
                        throw new Exception("Failed to create initial page");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
