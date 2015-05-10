using SDEngine;
using SDEngine.Memory;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



#pragma warning disable CS4014
namespace StrikeDistance_WindowsPhone {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalculatorPage : Page
    {
        WeatherInformation info;
        public const string WUNDERGROUND_API_KEY = "19b9a5738bb4a7f4";
        bool GettingLocation, UsingStopwatch, EditingField;
        StatusBar statusBar = StatusBar.GetForCurrentView();
        DispatcherTimer Stopwatch = new DispatcherTimer() { Interval = new TimeSpan(TimeSpan.TicksPerMillisecond) };
        DateTime Start, End;

        public CalculatorPage()
        {
            InitializeComponent();
            Stopwatch.Tick += (s, e) =>
            {
                End = DateTime.Now;
                TimeBox.Text = Math.Round(End.Subtract(Start).TotalSeconds, 2).ToString();
            };
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async Task Rate() {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private async Task SendFeedback() {
            var m = new EmailMessage() {
                Subject = "Feedback Submission (StrikeDistance)",
                Body = ""
            };
            m.To.Add(new EmailRecipient("devgregw@outlook.com"));
            await EmailManager.ShowComposeNewEmailAsync(m);
        }

        private void UpdateWeather() {
            if (string.IsNullOrEmpty(Manager.csource)) {
                TempBox.Text = Manager.Temp.ToString();
                wLat.Text = "Unavailable";
                wLon.Text = "Unavailable";
                wEle.Text = "Unavailable";
                wCnd.Text = "Unavailable";
                wTmp.Text = "Unavailable";
                wFlk.Text = "Unavailable";
                wHmd.Text = "Unavailable";
                wWdr.Text = "Unavailable";
                wWsp.Text = "Unavailable";
                wWgs.Text = "Unavailable";
                wPsr.Text = "Unavailable";
                wViewOnWU.IsEnabled = false;
                wViewHistoric.IsEnabled = false;
                return;
            }
            else if (info == null)
                info = new WeatherInformation(Manager.csource, Manager.TempUnit, Manager.SpeedUnit, Manager.PressureUnit);
            info.Update(Manager.TempUnit, Manager.SpeedUnit, Manager.PressureUnit);
            wViewOnWU.IsEnabled = true;
            wViewHistoric.IsEnabled = true;
            TempBox.Text = info.Temperature.ToString();
            wLat.Text = info.Latitude + "°";
            wLon.Text = info.Longitude + "°";
            wEle.Text = info.Elevation.ToString();
            wCnd.Text = info.ConditionString;
            wTmp.Text = info.Temperature + ((Manager.TempUnit == 0) ? " °F" : (Manager.TempUnit == 1) ? " °C" : " K");
            wFlk.Text = info.FeelsLike + ((Manager.TempUnit == 0) ? " °F" : (Manager.TempUnit == 1) ? " °C" : " K");
            wHmd.Text = info.Humidity;
            wWdr.Text = info.WindDirection;
            wWsp.Text = info.WindSpeed + ((Manager.SpeedUnit == 0) ? " MPH" : " KPH");
            wWgs.Text = info.WindGustSpeed + ((Manager.SpeedUnit == 0) ? " MPH" : " KPH");
            wPsr.Text = info.Pressure + ((Manager.PressureUnit == 0) ? " in" : " mb");
            wViewOnWU.NavigateUri = info.ForecastUrl;
            wViewHistoric.NavigateUri = info.HistoryUrl;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs args)
        {
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseVisible);
            statusBar.BackgroundOpacity = 1.0;
            statusBar.BackgroundColor = (Application.Current.Resources["StrikeDistanceThemeBrush"] as SolidColorBrush).Color;
            statusBar.ForegroundColor = (Application.Current.Resources["StrikeDistanceForegroundBrush"] as SolidColorBrush).Color;
            TimeBox.Text = Manager.Time.ToString();
            UpdateWeather();
            if (args.Parameter != null && args.NavigationMode == NavigationMode.New) {
                if ((bool)args.Parameter == true) {
                    var dlg = new ContentDialog() {
                        Title = "Help Improve StrikeDistance",
                        Content = new TextBlock() {
                            Text = "You've used StrikeDistance for awhile now, and I'd like to ask if you could rate my app and/or send feedback to me.  Reviews and feedback will help StrikeDistance improve.",
                            TextWrapping = TextWrapping.WrapWholeWords
                        },
                        PrimaryButtonText = "Sure!",
                        SecondaryButtonText = "No thanks"
                    };
                    dlg.PrimaryButtonClick += async (s, e) => {
                        await Rate();
                        var dlg2 = new ContentDialog() {
                            Title = "Help Improve StrikeDistance",
                            Content = new TextBlock() {
                                Text = "Would you like to send feedback too?  This is done via email.",
                                TextWrapping = TextWrapping.WrapWholeWords
                            },
                            PrimaryButtonText = "Okay",
                            SecondaryButtonText = "No thanks"
                        };
                        dlg2.PrimaryButtonClick += async (s2, e2) => {
                            await SendFeedback();
                        };
                        await dlg2.ShowAsync();
                    };
                    dlg.SecondaryButtonClick += (s, e) => {
                        var dlg2 = new ContentDialog() {
                            Title = "Help Improve StrikeDistance",
                            Content = new TextBlock() {
                                Text = "Would you like to send feedback instead?  This is done via email.",
                                TextWrapping = TextWrapping.WrapWholeWords
                            },
                            PrimaryButtonText = "Okay",
                            SecondaryButtonText = "No thanks"
                        };
                        dlg2.PrimaryButtonClick += async (s2, e2) => {
                            await SendFeedback();
                        };
                    };
                    await dlg.ShowAsync();
                }
            }
            #region Events
            #region TempBox
            TempBox.GotFocus += (s, e) =>
            {
                EditingField = true;
                ButtonBar.Visibility = Visibility.Collapsed;
                TempBox.SelectAll();
            };
            TempBox.LostFocus += (s, e) =>
            {
                if (!string.IsNullOrEmpty(TempBox.Text))
                    TempBox.Text = Manager.Temp.ToString();
                if (UsingStopwatch == false && GettingLocation == false)
                    ButtonBar.Visibility = Visibility.Visible;
                EditingField = false;
            };
            TempBox.TextChanged += (s, e) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(TempBox.Text))
                        Manager.Temp = Math.Round(Convert.ToDouble(TempBox.Text), 2);
                }
                catch (Exception) { }
            };
            #endregion
            #region TimeBox
            TimeBox.GotFocus += (s, e) =>
            {
                EditingField = true;
                ButtonBar.Visibility = Visibility.Collapsed;
                TimeBox.SelectAll();
            };
            TimeBox.LostFocus += (s, e) =>
            {
                if (!string.IsNullOrEmpty(TimeBox.Text))
                    TimeBox.Text = Manager.Time.ToString();
                if (UsingStopwatch == false && GettingLocation == false)
                    ButtonBar.Visibility = Visibility.Visible;
                EditingField = false;
            };
            TimeBox.TextChanged += (s, e) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(TimeBox.Text))
                        Manager.Time = Math.Round(Convert.ToDouble(TimeBox.Text), 2);
                }
                catch (Exception) { }
            };
            #endregion
            #endregion
        }

        private void InvertButton_Click(object sender, RoutedEventArgs e)
        {
            TempBox.Text = (Convert.ToDecimal(TempBox.Text) * -1).ToString();
        }

        private async void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                TempBox.IsTabStop = false;
                TempBox.IsEnabled = false;
                GettingLocation = true;
                await statusBar.ProgressIndicator.ShowAsync();
                TempBox.Text = "";
                ButtonBar.Visibility = Visibility.Collapsed;
                InvertButton.IsEnabled = false;
                LocationButton.IsEnabled = false;
                statusBar.ProgressIndicator.Text = "Getting weather data...";
                weatherbar.Visibility = Visibility.Visible;
                wViewOnWU.IsEnabled = false;
                wViewHistoric.IsEnabled = false;
                info = await Main.GetWeatherInformation(WUNDERGROUND_API_KEY, Manager.TempUnit, Manager.SpeedUnit, Manager.PressureUnit);
                weatherbar.Visibility = Visibility.Collapsed;
                UpdateWeather();
                await statusBar.ProgressIndicator.HideAsync();
                LocationButton.IsEnabled = true;
                TempBox.IsTabStop = true;
                TempBox.IsEnabled = true;
                if (UsingStopwatch == false && EditingField == false)
                    ButtonBar.Visibility = Visibility.Visible;
                InvertButton.IsEnabled = true;
                GettingLocation = false;
            }
            catch (Exception ex) {
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
        }

        private void StopwatchClearButton_Click(object sender, RoutedEventArgs e)
        {
            TimeBox.Text = "0";
        }

        private void StopwatchButton_Checked(object sender, RoutedEventArgs e)
        {
            UsingStopwatch = true;
            TimeBox.IsEnabled = false;
            StopwatchClearButton.IsEnabled = false;
            ButtonBar.Visibility = Visibility.Collapsed;
            Stopwatch.Start();
            Start = DateTime.Now;
            End = DateTime.Now;
        }

        private void StopwatchButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Stopwatch.Stop();
            End = DateTime.Now;
            TimeBox.Text = Math.Round(End.Subtract(Start).TotalSeconds, 2).ToString();
            TimeBox.IsEnabled = true;
            StopwatchClearButton.IsEnabled = true;
            if (GettingLocation == false && EditingField == false)
                ButtonBar.Visibility = Visibility.Visible;
            UsingStopwatch = false;
        }

        private async void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            await new MessageDialog(
                Main.Calculate(
                    Convert.ToDouble(TempBox.Text),
                    Convert.ToDouble(TimeBox.Text)),
                    "Result").ShowAsync();
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            TempBox.Text = "0";
            TimeBox.Text = "0";
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(SettingsPage)))
                throw new Exception("Failed to create initial page");
        }

        private async void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(AboutPage)));
        }

        private async void AppsButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("zune:search?publisher=Greg Whatley"));
        }

        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            await Rate();
        }

        private async void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            await SendFeedback();
        }
    }
}
