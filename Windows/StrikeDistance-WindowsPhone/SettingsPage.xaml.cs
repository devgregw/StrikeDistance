using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SDEngine.Memory;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StrikeDistance_WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            tempUnit.SelectionChanged -= tempUnit_SelectionChanged;
            distUnit.SelectionChanged -= distUnit_SelectionChanged;
            autoGet.Checked -= autoGet_Checked;
            autoGet.Unchecked -= autoGet_Unchecked;
            warnPolicy.Checked -= warnPolicy_Checked;
            warnPolicy.Unchecked -= warnPolicy_Unchecked;
            verboseMode.Toggled -= verboseMode_Toggled;
            unitDetails.Checked -= unitDetails_Checked;
            unitDetails.Unchecked -= unitDetails_Unchecked;
            convMath.Checked -= convMath_Checked;
            convMath.Unchecked -= convMath_Unchecked;
            calcMath.Checked -= calcMath_Checked;
            calcMath.Unchecked -= calcMath_Unchecked;
            {
                tempUnit.SelectedIndex = Manager.TempUnit;
                distUnit.SelectedIndex = Manager.DistUnit;
                autoGet.IsChecked = Manager.AutoGet;
                warnPolicy.IsChecked = Manager.WarnPolicy;
                verboseMode.IsOn = Manager.VerboseMode;
                unitDetails.IsChecked = Manager.VerboseModeData.ElementAt(0);
                convMath.IsChecked = Manager.VerboseModeData.ElementAt(1);
                calcMath.IsChecked = Manager.VerboseModeData.ElementAt(2);
                tempUnit.SelectionChanged -= tempUnit_SelectionChanged;
                distUnit.SelectionChanged -= distUnit_SelectionChanged;
            }
            autoGet.Checked += autoGet_Checked;
            autoGet.Unchecked += autoGet_Unchecked;
            warnPolicy.Checked += warnPolicy_Checked;
            warnPolicy.Unchecked += warnPolicy_Unchecked;
            verboseMode.Toggled += verboseMode_Toggled;
            unitDetails.Checked += unitDetails_Checked;
            unitDetails.Unchecked += unitDetails_Unchecked;
            convMath.Checked += convMath_Checked;
            convMath.Unchecked += convMath_Unchecked;
            calcMath.Checked += calcMath_Checked;
            calcMath.Unchecked += calcMath_Unchecked;
        }

        private void tempUnit_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.TempUnit = (int)((ComboBoxItem)tempUnit.SelectedItem).Tag;
        }

        private void distUnit_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.DistUnit = (int)((ComboBoxItem)distUnit.SelectedItem).Tag;
        }

        private void autoGet_Checked(object sender, RoutedEventArgs e) {
            Manager.AutoGet = true;
        }

        private void autoGet_Unchecked(object sender, RoutedEventArgs e) {
            Manager.AutoGet = false;
        }

        private void warnPolicy_Unchecked(object sender, RoutedEventArgs e) {
            Manager.WarnPolicy = false;
        }

        private void warnPolicy_Checked(object sender, RoutedEventArgs e) {
            Manager.WarnPolicy = true;
        }

        private void verboseMode_Toggled(object sender, RoutedEventArgs e) {
            Manager.VerboseMode = verboseMode.IsOn;
        }

        private void unitDetails_Unchecked(object sender, RoutedEventArgs e) {
            bool conv = convMath.IsChecked.Value, calc = calcMath.IsChecked.Value;
            Manager.VerboseModeData = new List<bool>() {
                false,
                conv,
                calc
            };
        }

        private void unitDetails_Checked(object sender, RoutedEventArgs e) {
            bool conv = convMath.IsChecked.Value, calc = calcMath.IsChecked.Value;
            Manager.VerboseModeData = new List<bool>() {
                true,
                conv,
                calc
            };
        }

        private void convMath_Checked(object sender, RoutedEventArgs e) {
            bool unit = unitDetails.IsChecked.Value, calc = calcMath.IsChecked.Value;
            Manager.VerboseModeData = new List<bool>() {
                unit,
                false,
                calc
            };
        }

        private void calcMath_Checked(object sender, RoutedEventArgs e) {
            bool unit = unitDetails.IsChecked.Value, conv = convMath.IsChecked.Value;
            Manager.VerboseModeData = new List<bool>() {
                unit,
                conv,
                true
            };
        }

        private void convMath_Unchecked(object sender, RoutedEventArgs e) {
            bool calc = calcMath.IsChecked.Value, unit = unitDetails.IsChecked.Value;
            Manager.VerboseModeData = new List<bool>() {
                unit,
                false,
                calc
            };
        }

        private void calcMath_Unchecked(object sender, RoutedEventArgs e) {
            bool conv = convMath.IsChecked.Value, unit = unitDetails.IsChecked.Value;
            Manager.VerboseModeData = new List<bool>() {
                unit,
                conv,
                false
            };
        }
    }
}
