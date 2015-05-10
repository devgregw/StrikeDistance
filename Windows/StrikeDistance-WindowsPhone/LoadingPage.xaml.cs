using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SDEngine;
using Windows.UI.Popups;
using System.Net.Http;
using SDEngine.Exceptions;
using System.Threading.Tasks;
using SDEngine.Memory;



#pragma warning disable CS4014
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
            int lc = SDEngine.Memory.Utility.UtilityMethods.Get("LaunchCount", 0);
            SDEngine.Memory.Utility.UtilityMethods.Set("LaunchCount", lc + 1);
            var con = lc == 5 || lc == 10;
            try {
                await Main.GetWeatherInformation(WUNDERGROUND_API_KEY, Manager.TempUnit, Manager.SpeedUnit, Manager.PressureUnit);
                Frame.Navigate(typeof(CalculatorPage), con);
            }
            catch (NoConnectionException) {
                var t = new Task(() => {
                    new MessageDialog(
                        "StrikeDistance could not connect to the weather server.\n\nStrikeDistance can and will continue, but some features that require Internet access may not work.\nStrikeDistance will use the most recent data instead.",
                        "Error").ShowAsync();
                });
                t.ContinueWith((continuewith) => {
                    Frame.Navigate(typeof(CalculatorPage), con);
                });
                t.Start();
            }
            catch (Exception ex) {
                if (ex.GetType() == typeof(InvalidOperationException)) {
                    var t = new Task(() => {
                        new MessageDialog(
                            string.Format(
                                "StrikeDistance could not connect to the weather server.\n\nStrikeDistance can and will continue, but some features that require Internet access may not work.\n\n{0}\n{1}",
                                ex.GetType().FullName,
                                ex.Message),
                            "Error").ShowAsync();
                    });
                    t.ContinueWith((continuewith) => {
                        Frame.Navigate(typeof(CalculatorPage), con);
                    });
                    t.Start();
                }
                else if (ex.GetType() == typeof(HttpRequestException)) {
                    var t = new Task(() => {
                        new MessageDialog(
                            string.Format(
                                "StrikeDistance encountered an error.\n\nStrikeDistance may be able to continue, but some features that require Internet access may not work.\n\n{0}\n{1}",
                                ex.GetType().FullName,
                                ex.Message),
                            "Error").ShowAsync();
                    });
                    t.ContinueWith((continuewith) => {
                        Frame.Navigate(typeof(CalculatorPage), con);
                    });
                    t.Start();
                }
                else {
                    var t = new Task(() => {
                        new MessageDialog(
                            string.Format(
                                "StrikeDistance encountered an error.\n\nStrikeDistance may be able to continue.\n\n{0}\n{1}",
                                ex.GetType().FullName,
                                ex.Message),
                            "Unexpected Error").ShowAsync();
                    });
                    t.ContinueWith((continuewith) => {
                        Frame.Navigate(typeof(CalculatorPage), con);
                    });
                    t.Start();
                }
            }
        }
    }
}
