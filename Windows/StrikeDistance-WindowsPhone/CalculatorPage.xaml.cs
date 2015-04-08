﻿using SDEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StrikeDistance_WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalculatorPage : Page
    {
        bool GettingLocation, UsingStopwatch, EditingField;
        StatusBar statusBar = StatusBar.GetForCurrentView();
        DispatcherTimer Stopwatch = new DispatcherTimer() { Interval = new TimeSpan(TimeSpan.TicksPerMillisecond) };
        DateTime Start, End;

        public CalculatorPage()
        {
            this.InitializeComponent();
            Stopwatch.Tick += (s, e) =>
            {
                End = DateTime.Now;
                TimeBox.Text = Math.Round(End.Subtract(Start).TotalSeconds, Engine.Memory.DecimalPoints).ToString();
            };
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            (ApplicationView.GetForCurrentView()).SetDesiredBoundsMode(ApplicationViewBoundsMode.UseVisible);
            //if (args.NavigationMode == NavigationMode.New)
            //    Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            //switch (Engine.HasUpgradePack)
            //{
            //    case Engine.UpgradeMode.Debug: AdDisplay.IsEnabled = true; AdDisplay.IsTest = true; AdDisplay.Visibility = Windows.UI.Xaml.Visibility.Visible; break;
            //    case Engine.UpgradeMode.HasPack: AdDisplay.IsEnabled = false; AdDisplay.IsTest = false; AdDisplay.Visibility = Windows.UI.Xaml.Visibility.Collapsed; break;
            //    case Engine.UpgradeMode.NoPack: AdDisplay.IsEnabled = true; AdDisplay.IsTest = false; AdDisplay.Visibility = Windows.UI.Xaml.Visibility.Visible; break;
            //}
            statusBar.BackgroundOpacity = 1.0;
            statusBar.BackgroundColor = (App.Current.Resources["StrikeDistanceThemeBrush"] as SolidColorBrush).Color;
            statusBar.ForegroundColor = (App.Current.Resources["StrikeDistanceForegroundBrush"] as SolidColorBrush).Color;
            TempBox.Text = Engine.Memory.Temp.ToString();
            TimeBox.Text = Engine.Memory.Time.ToString();
            TempUnitButton.Content = Engine.Memory.IntToString(Engine.Memory.UnitScope.Temperature, Engine.Memory.TempUnit);
            DistUnitButton.Content = Engine.Memory.IntToString(Engine.Memory.UnitScope.Distance, Engine.Memory.DistUnit);
            #region Events
            #region TempBox
            TempBox.GotFocus += (s, e) =>
            {
                EditingField = true;
                ButtonBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                TempBox.SelectAll();
            };
            TempBox.LostFocus += (s, e) =>
            {
                if (!string.IsNullOrEmpty(TempBox.Text))
                    TempBox.Text = Engine.Memory.Temp.ToString();
                if (UsingStopwatch == false && GettingLocation == false)
                    ButtonBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
                EditingField = false;
            };
            TempBox.TextChanged += (s, e) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(TempBox.Text))
                        Engine.Memory.Temp = Math.Round(Convert.ToDouble(TempBox.Text), Engine.Memory.DecimalPoints);
                }
                catch (Exception) { }
            };
            #endregion
            #region TimeBox
            TimeBox.GotFocus += (s, e) =>
            {
                EditingField = true;
                ButtonBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                TimeBox.SelectAll();
            };
            TimeBox.LostFocus += (s, e) =>
            {
                if (!string.IsNullOrEmpty(TimeBox.Text))
                    TimeBox.Text = Engine.Memory.Time.ToString();
                if (UsingStopwatch == false && GettingLocation == false)
                    ButtonBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
                EditingField = false;
            };
            TimeBox.TextChanged += (s, e) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(TimeBox.Text))
                        Engine.Memory.Time = Math.Round(Convert.ToDouble(TimeBox.Text), Engine.Memory.DecimalPoints);
                }
                catch (Exception) { }
            };
            #endregion
            #region MenuFlyouts
            MenuFlyout TempMenu = Flyout.GetAttachedFlyout(TempUnitButton) as MenuFlyout;
            TempMenu.Opened += (s, e) =>
            {
                TempMenu.Items[Engine.Memory.TempUnit].Foreground = App.Current.Resources["PhoneAccentBrush"] as Brush;
            };
            foreach (MenuFlyoutItem i in TempMenu.Items)
            {
                i.Click += (s, e) =>
                {
                    Engine.Memory.TempUnit = Engine.Memory.StringToInt(Engine.Memory.UnitScope.Temperature, i.Text);
                    TempUnitButton.Content = i.Text;
                    foreach (MenuFlyoutItem i2 in TempMenu.Items)
                    { i2.Foreground = App.Current.Resources["PhoneBackgroundBrush"] as Brush; }
                };
            }
            MenuFlyout DistMenu = Flyout.GetAttachedFlyout(DistUnitButton) as MenuFlyout;
            DistMenu.Opened += (s, e) =>
            {
                DistMenu.Items[Engine.Memory.DistUnit].Foreground = App.Current.Resources["PhoneAccentBrush"] as Brush;
            };
            foreach (MenuFlyoutItem i in DistMenu.Items)
            {
                i.Click += (s, e) =>
                {
                    Engine.Memory.DistUnit = Engine.Memory.StringToInt(Engine.Memory.UnitScope.Distance, i.Text);
                    DistUnitButton.Content = i.Text;
                    foreach (MenuFlyoutItem i2 in DistMenu.Items)
                    { i2.Foreground = App.Current.Resources["PhoneBackgroundBrush"] as Brush; }
                };
            }
            #endregion
            #endregion
        }

        private void InvertButton_Click(object sender, RoutedEventArgs e)
        {
            TempBox.Text = (Convert.ToDecimal(TempBox.Text) * -1).ToString();
        }

        private async void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            (new MessageDialog("This feature is currently not supported.\n\nSee the about page for more information.", "Not Supported")).ShowAsync();
            //TempBox.IsTabStop = false;
            //TempBox.IsEnabled = false;
            //if (Engine.HasUpgradePack == Engine.UpgradeMode.Debug)
            //{
            //    var msg = new ContentDialog()
            //    {
            //        PrimaryButtonText = "buy now",
            //        SecondaryButtonText = "later",
            //        Content = new TextBlock()
            //        {
            //            Text = "Buy the upgrade pack today to unlock this feature!\n\nIt also removes ads, and unlocks decimal points and verbose mode!\n",
            //            TextWrapping = TextWrapping.WrapWholeWords
            //        }
            //    };
            //    msg.PrimaryButtonClick += async (s, arg) =>
            //        {
            //            //TODO: add purchase logic
            //        };
            //    msg.Opened += (s, arg) => { statusBar.ForegroundColor = Color.FromArgb(255, 255, 255, 255); };
            //    msg.Closing += (s, arg) =>
            //        {
            //            statusBar.ForegroundColor = (App.Current.Resources["StrikeDistanceForegroundBrush"] as SolidColorBrush).Color;
            //            TempBox.IsTabStop = true;
            //            TempBox.IsEnabled = true;
            //        };
            //    await msg.ShowAsync();
            //}
            //else
            //{
            //    GettingLocation = true;
            //    await statusBar.ProgressIndicator.ShowAsync();
            //    TempBox.Text = "";
            //    ButtonBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //    InvertButton.IsEnabled = false;
            //    LocationButton.IsEnabled = false;
            //    statusBar.ProgressIndicator.Text = "Getting weather data...";
            //    TempBox.Text = Convert.ToString(await Engine.GetTemperature());
            //    await statusBar.ProgressIndicator.HideAsync();
            //    LocationButton.IsEnabled = true;
            //    TempBox.IsTabStop = true;
            //    TempBox.IsEnabled = true;
            //    if (UsingStopwatch == false && EditingField == false)
            //        ButtonBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
            //    InvertButton.IsEnabled = true;
            //    GettingLocation = false;
            //}
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
            ButtonBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Stopwatch.Start();
            Start = DateTime.Now;
            End = DateTime.Now;
        }

        private void StopwatchButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Stopwatch.Stop();
            End = DateTime.Now;
            TimeBox.Text = Math.Round(End.Subtract(Start).TotalSeconds, Engine.Memory.DecimalPoints).ToString();
            TimeBox.IsEnabled = true;
            StopwatchClearButton.IsEnabled = true;
            if (GettingLocation == false && EditingField == false)
                ButtonBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
            UsingStopwatch = false;
        }

        private async void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Result = new MessageDialog(Engine.Calculate(Convert.ToDouble(TempBox.Text),
                                                     Convert.ToDouble(TimeBox.Text)), "Result");
            await Result.ShowAsync();
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            TempBox.Text = "0";
            TimeBox.Text = "0";
        }

        private void TempUnitButton_Click(object sender, RoutedEventArgs args)
        {
            Flyout.ShowAttachedFlyout(TempUnitButton);
        }

        private void DistUnitButton_Click(object sender, RoutedEventArgs e)
        {
            Flyout.ShowAttachedFlyout(DistUnitButton);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(SettingsPage)))
                throw new Exception("Failed to create initial page");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private async void AppsButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("zune:search?publisher=Greg Whatley"));
        }

        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private async void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            var m = new EmailMessage()
            {
                Subject = "Bug Report (StrikeDistance)",
                Body = "Please describe the bug, what you were doing when it occured, and any extra details you may have:\n\nWhich device do you have?:\n\nWhat version of Windows Phone are you using?:\n\nThank you for helping make StrikeDistance better!"
            };
            m.To.Add(new EmailRecipient("techperson32@live.com"));
            await EmailManager.ShowComposeNewEmailAsync(m);
        }
    }
}