using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SDEngine;
using Windows.UI.Popups;
using System.Net.Http;
using SDEngine.Memory;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StrikeDistance_WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage : Page
    {
        public const string WUNDERGROUND_API_KEY = "19b9a5738bb4a7f4";

        public LoadingPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var sb = StatusBar.GetForCurrentView();
            sb.ForegroundColor = (Windows.UI.Xaml.Application.Current.Resources["StrikeDistanceForegroundBrush"] as SolidColorBrush).Color;
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            try
            {
                Manager.csource = (await Main.GetWeatherInformation(WUNDERGROUND_API_KEY)).xmlSource;
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(InvalidOperationException))
                    new MessageDialog(
                        string.Format(
                            "StrikeDistance could not connect to the weather server.\n\nStrikeDistance can and will continue, but some features that require Internet access may not work.\n\n{0}\n{1}",
                            ex.GetType().FullName,
                            ex.Message),
                        "Error").ShowAsync();
                else if (ex.GetType() == typeof(HttpRequestException))
                    new MessageDialog(
                        string.Format(
                            "StrikeDistance encountered an error.\n\nStrikeDistance may be able to continue, but some features that require Internet access may not work.\n\n{0}\n{1}",
                            ex.GetType().FullName,
                            ex.Message),
                        "Error").ShowAsync();
                else
                    new MessageDialog(
                        string.Format(
                            "StrikeDistance encountered an error.\n\nStrikeDistance may be able to continue.\n\n{0}\n{1}",
                            ex.GetType().FullName,
                            ex.Message),
                        "Unexpected Error").ShowAsync();
            }
            finally
            {
                Frame.Navigate(typeof(CalculatorPage));
            }
        }
    }
}
