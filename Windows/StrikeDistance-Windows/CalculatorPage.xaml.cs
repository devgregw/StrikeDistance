using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StrikeDistance_Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalculatorPage : Page
    {
        int barview = 2;
        int paneview = 1;

        TextBlock tbt {
            get {
                return GetResource<TextBlock>("TitleBarT");
            }
        }

        Image tbi {
            get {
                return GetResource<Image>("TitleBarI");
            }
        }
        Button cb {
            get {
                var x = GetResource<Button>("CalculateButton");
                x.Click += CalculateButton_Click;
                return x;
            }
        }

        Button clb {
            get {
                var x = GetResource<Button>("ClearButton");
                x.Click += ClearButton_Click;
                return x;
            }
        }

        Button rb {
            get {
                var x = GetResource<Button>("RefreshButton");
                x.Click += RefreshButton_Click;
                return x;
            }
        }

        Button sb {
            get {
                var x = GetResource<Button>("SettingsButton");
                x.Click += SettingsButton_Click;
                return x;
            }
        }

        Button mb {
            get {
                var x = GetResource<Button>("MoreButton");
                x.Click += MoreButton_Click;
                foreach (object o in (FlyoutBase.GetAttachedFlyout(x) as MenuFlyout).Items) {
                    if (o is MenuFlyoutItem) {
                        MenuFlyoutItem obj = o as MenuFlyoutItem;
                        if (obj.Text == "Clear All")
                            obj.Click += ClearButton_Click;
                        else if (obj.Text == "Refresh")
                            obj.Click += RefreshButton_Click;
                        else
                            obj.Click += SettingsButton_Click;
                    }
                    else
                        continue;
                }
                return x;
            }
        }


        internal T GetResource<T>(string key) where T : class {
            return App.Current.Resources[key] as T;
        }

        internal void AddToBarPanel(params FrameworkElement[] objects) {
            foreach (FrameworkElement e in BarPanel.Children) {
                e.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            foreach (FrameworkElement e in objects) {
                (BarPanel.Children.ElementAt(BarPanel.Children.IndexOf(e)) as FrameworkElement).Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        public CalculatorPage() {
            this.InitializeComponent();
            Window.Current.SizeChanged += Page_SizeChanged;
        }

        internal void ChangeView(int b, int p) {
            if (b != barview) {
                if (b == 0) {
                    AddToBarPanel(TitleBarI, CalculateButton, MoreButton);
                }
                else if (b == 1) {
                    AddToBarPanel(TitleBarI, CalculateButton, ClearButton, RefreshButton, SettingsButton);
                }
                else if (b == 2) {
                    AddToBarPanel(TitleBarT, CalculateButton, ClearButton, RefreshButton, SettingsButton);
                }
                barview = b;
            }
            if (p != paneview) {
                if (p == 0) {
                    RightPane.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else if (p == 1) {
                    RightPane.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                paneview = p;
            }
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
                //if (RightPane.Visibility == Windows.UI.Xaml.Visibility.Visible)
                //    RightPane.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ChangeView(barview, 0);
                TemperatureBox.Width = (fullWidth) - 300.0;
                TimeBox.Width = (fullWidth) - 237.0;
            }
            else {
                LeftPane.Width = fullWidth / 2.0;
                LeftPane.Margin = new Thickness(LeftPane.Margin.Left, LeftPane.Margin.Top, fullWidth / 2.0, LeftPane.Margin.Bottom);
                RightPane.Width = fullWidth / 2.0;
                RightPane.Margin = new Thickness(fullWidth / 2.0, RightPane.Margin.Top, RightPane.Margin.Right, RightPane.Margin.Bottom);
                //if (RightPane.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
                //    RightPane.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ChangeView(barview, 1);
                TemperatureBox.Width = (fullWidth / 2.0) - 300.0;
                TimeBox.Width = (fullWidth / 2.0) - 237.0;
            }
            if (fullWidth < widthToCollapseButtons) {
                ChangeView(0, paneview);
            }
            else if (fullWidth < titleBarWidthWithTitle) {
                ChangeView(1, paneview);
            }
            else {
                ChangeView(2, paneview);
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
            var i = BarPanel.Children.IndexOf(mb);
            FrameworkElement element = (FrameworkElement)BarPanel.Children.ElementAt(i);
            FlyoutBase.ShowAttachedFlyout(element);
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
