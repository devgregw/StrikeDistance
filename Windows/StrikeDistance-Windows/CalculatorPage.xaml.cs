using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StrikeDistance_Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalculatorPage : Page
    {
        public CalculatorPage() {
            this.InitializeComponent();
            Window.Current.SizeChanged += Page_SizeChanged;
        }

        internal void ReplaceElement(UIElement what, UIElement with) {
            HideElement(what);
            ShowElement(with);
        }

        internal void HideElement(UIElement e) {
            e.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        internal void ShowElement(UIElement e) {
            e.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        internal void UpdateSize() {
            Debug.WriteLine("Updating size");
            double fullWidth = Window.Current.Bounds.Width;
            double titleBarWidthWithTitle = 1100.0;
            double titleBarWidthWithImage = 800.0;
            double widthToCollapseButtons = 750.0;
            if (fullWidth <= titleBarWidthWithImage) {
                LeftPane.Width = fullWidth;
                LeftPane.Margin = new Thickness(LeftPane.Margin.Left, LeftPane.Margin.Top, 0, LeftPane.Margin.Bottom);
                if (RightPane.Visibility == Windows.UI.Xaml.Visibility.Visible)
                    RightPane.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                TemperatureBox.Width = (fullWidth) - 300.0;
                TimeBox.Width = (fullWidth) - 237.0;
            }
            else {
                LeftPane.Width = fullWidth / 2.0;
                LeftPane.Margin = new Thickness(LeftPane.Margin.Left, LeftPane.Margin.Top, (double)fullWidth / 2.0, LeftPane.Margin.Bottom);
                RightPane.Width = fullWidth / 2.0;
                RightPane.Margin = new Thickness(fullWidth / 2.0, RightPane.Margin.Top, RightPane.Margin.Right, RightPane.Margin.Bottom);
                if (RightPane.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
                    RightPane.Visibility = Windows.UI.Xaml.Visibility.Visible;
                TemperatureBox.Width = (fullWidth / 2.0) - 300.0;
                TimeBox.Width = (fullWidth / 2.0) - 237.0;
            }
            if (fullWidth < titleBarWidthWithTitle) {
                ReplaceElement(TitleBarT, TitleBarI);
            }
            else if (fullWidth >= titleBarWidthWithTitle) {
                ReplaceElement(TitleBarI, TitleBarT);
            }
            if (fullWidth < widthToCollapseButtons) {
                ShowElement(MoreButton);
                HideElement(ClearButton);
                HideElement(RefreshButton);
                HideElement(SettingsButton);
            }
            else {
                HideElement(MoreButton);
                ShowElement(ClearButton);
                ShowElement(RefreshButton);
                ShowElement(SettingsButton);
            }
            ContentPane.Width = Window.Current.Bounds.Width;
            this.UpdateLayout();
            Debug.WriteLine("Size updated");
        }

        private void Page_SizeChanged(object sender, WindowSizeChangedEventArgs e) {
            e.Handled = true;
            UpdateSize();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            UpdateSize();
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e) {
            FlyoutBase.ShowAttachedFlyout(MoreButton);
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e) {

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e) {

        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) {

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(SettingsPage));
        }
    }
}
