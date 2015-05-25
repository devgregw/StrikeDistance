using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StrikeDistance_WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                if (args.Uri.AbsoluteUri.Contains("wunderground"))
                {
                    raindrop.IsEnabled = true;
                    progressBar.Visibility = Visibility.Collapsed;
                    await new MessageDialog("The raindrop referral was sent successfully.", "Thanks!").ShowAsync();
                    web.Navigate(new Uri("http://localhost", UriKind.Absolute));
                }
            }
            else
            {
                if (args.Uri.AbsoluteUri.Contains("wunderground"))
                    await new MessageDialog(string.Format("The referral could not be sent.\n\n{0}\n\nTry again later.", args.WebErrorStatus.ToString()), "Raindrop Referal").ShowAsync();
                raindrop.IsEnabled = true;
                progressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void raindrop_Click(object sender, RoutedEventArgs e)
        {
            web.Navigate(new Uri("http://www.wunderground.com/?apiref=ddc7474baa78cc4d", UriKind.Absolute));
            raindrop.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
        }
    }
}
