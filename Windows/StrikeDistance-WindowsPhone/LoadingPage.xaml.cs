using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SDEngine;
using Windows.UI.Popups;
using System.Net.Http;
using SDEngine.Exceptions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StrikeDistance_WindowsPhone {
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
                await Main.GetWeatherInformation(WUNDERGROUND_API_KEY);
                Frame.Navigate(typeof(CalculatorPage));
            }
            catch (NoConnectionException) {
                new MessageDialog(
                    "StrikeDistance could not connect to the weather server.\n\nStrikeDistance can and will continue, but some features that require Internet access may not work.\nStrikeDistance will use the most recent data instead.",
                    "Error").ShowAsync();
                Frame.Navigate(typeof(CalculatorPage));
            }
            catch (TooSoonException ex) {
                new MessageDialog(
                    string.Format("Because of the Weather Retrieval Frequency policy (read about it in Settings), StrikeDistance will not retrieve updated weather information at this time.\nWeather data was already retrieved {0} minutes ago, and the policy states weather data can be retrieved every 10 minutes.\nTry again in {1} minutes.\n\nStrikeDistance will use the most recent data instead for now.", ex.CurrentTime.Subtract(ex.PreviousTime).Minutes, 10 - ex.CurrentTime.Subtract(ex.PreviousTime).Minutes),
                    "Weather Retrieval Frequency Policy").ShowAsync();
                Frame.Navigate(typeof(CalculatorPage));
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
                Frame.Navigate(typeof(CalculatorPage));
            }
        }
    }
}
