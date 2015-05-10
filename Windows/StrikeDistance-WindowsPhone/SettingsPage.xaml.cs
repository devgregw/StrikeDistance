using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SDEngine.Memory;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StrikeDistance_WindowsPhone {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            tempUnit.SelectionChanged -= tempUnit_SelectionChanged;
            distUnit.SelectionChanged -= distUnit_SelectionChanged;
            speedUnit.SelectionChanged -= speedUnit_SelectionChanged;
            pressureUnit.SelectionChanged -= pressureUnit_SelectionChanged;
            autoGet.Checked -= autoGet_Checked;
            autoGet.Unchecked -= autoGet_Unchecked;
            verboseMode.Toggled -= verboseMode_Toggled;
            unitDetails.Checked -= unitDetails_Checked;
            unitDetails.Unchecked -= unitDetails_Unchecked;
            convMath.Checked -= convMath_Checked;
            convMath.Unchecked -= convMath_Unchecked;
            calcMath.Checked -= calcMath_Checked;
            calcMath.Unchecked -= calcMath_Unchecked;
            var data = Manager.VerboseModeData;
            tempUnit.SelectedIndex = Manager.TempUnit;
            distUnit.SelectedIndex = Manager.DistUnit;
            speedUnit.SelectedIndex = Manager.SpeedUnit;
            pressureUnit.SelectedIndex = Manager.PressureUnit;
            autoGet.IsChecked = Manager.AutoGet;
            verboseMode.IsOn = Manager.VerboseMode;
            if (verboseMode.IsOn) {
                unitDetails.IsEnabled = true;
                convMath.IsEnabled = true;
                calcMath.IsEnabled = true;
            }
            else {
                unitDetails.IsEnabled = false;
                convMath.IsEnabled = false;
                calcMath.IsEnabled = false;
            }
            unitDetails.IsChecked = data.ElementAt(0);
            convMath.IsChecked = data.ElementAt(1);
            calcMath.IsChecked = data.ElementAt(2);
            tempUnit.SelectionChanged += tempUnit_SelectionChanged;
            distUnit.SelectionChanged += distUnit_SelectionChanged;
            speedUnit.SelectionChanged += speedUnit_SelectionChanged;
            pressureUnit.SelectionChanged += pressureUnit_SelectionChanged;
            autoGet.Checked += autoGet_Checked;
            autoGet.Unchecked += autoGet_Unchecked;
            verboseMode.Toggled += verboseMode_Toggled;
            unitDetails.Checked += unitDetails_Checked;
            unitDetails.Unchecked += unitDetails_Unchecked;
            convMath.Checked += convMath_Checked;
            convMath.Unchecked += convMath_Unchecked;
            calcMath.Checked += calcMath_Checked;
            calcMath.Unchecked += calcMath_Unchecked;
        }

        private void tempUnit_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.TempUnit = tempUnit.SelectedIndex;
        }

        private void distUnit_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.DistUnit = distUnit.SelectedIndex;
        }

        private void speedUnit_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.SpeedUnit = speedUnit.SelectedIndex;
        }

        private void pressureUnit_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.PressureUnit = pressureUnit.SelectedIndex;
        }

        private void autoGet_Checked(object sender, RoutedEventArgs e) {
            Manager.AutoGet = true;
        }

        private void autoGet_Unchecked(object sender, RoutedEventArgs e) {
            Manager.AutoGet = false;
        }

        private void verboseMode_Toggled(object sender, RoutedEventArgs e) {
            Manager.VerboseMode = verboseMode.IsOn;
            if (verboseMode.IsOn) {
                unitDetails.IsEnabled = true;
                convMath.IsEnabled = true;
                calcMath.IsEnabled = true;
            }
            else {
                unitDetails.IsEnabled = false;
                convMath.IsEnabled = false;
                calcMath.IsEnabled = false;
            }
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
                true,
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
