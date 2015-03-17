using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Storage;
using SDEngine.Convertions.Enumerations;
using SDEngine.Memory.Utility;

namespace SDEngine {
    namespace Convertions {
        namespace Enumerations {
            /// <summary>
            /// Represents available temperature units
            /// </summary>
            /// <remarks>
            /// Should only be used for SDEngine.Convertions.Converter.Convert()
            /// </remarks>
            public enum TemperatureUnit {
                Fahrenheit,
                Celsius,
                Kelvin
            }

            /// <summary>
            /// Represents available distatnce units
            /// </summary>
            /// <remarks>
            /// Should only be used for SDEngine.Convertions.Converter.Convert()
            /// </remarks>
            public enum DistanceUnit {
                Miles,
                Feet,
                Meters,
                Kilometers
            }
        }

        /// <summary>
        /// Contains methods for converting one unit to another
        /// </summary>
        public static class Converter {
            /// <summary>
            /// Converts a temperature value from one unit to another
            /// </summary>
            /// <param name="from">The <code>TemperatureUnit</code> the <code>temp</code> parameter is given in</param>
            /// <param name="to">The <code>TemperatureUnit</code> to convert the <code>temp</code> parameter to</param>
            /// <param name="temp">The value to convert</param>
            /// <returns>The converted temperature</returns>
            public static double Convert(TemperatureUnit from, TemperatureUnit to, double temp) {
                switch (from) {
                    case TemperatureUnit.Fahrenheit:
                        switch (to) {
                            case TemperatureUnit.Fahrenheit:
                                return temp;
                            case TemperatureUnit.Celsius:
                                return (temp - 32) * 5 / 9;
                            case Enumerations.TemperatureUnit.Kelvin:
                                return (temp + 459.67) * 5 / 9;
                        }
                        break;
                    case TemperatureUnit.Celsius:
                        switch (to) {
                            case TemperatureUnit.Fahrenheit:
                                return temp * 9 / 5 + 32;
                            case TemperatureUnit.Celsius:
                                return temp;
                            case TemperatureUnit.Kelvin:
                                return temp + 273.15;
                        }
                        break;
                    case TemperatureUnit.Kelvin:
                        switch (to) {
                            case TemperatureUnit.Fahrenheit:
                                return temp * 9 / 5 - 459.67;
                            case TemperatureUnit.Celsius:
                                return temp - 273.15;
                            case TemperatureUnit.Kelvin:
                                return temp;
                        }
                        break;
                }
                throw new Exception(string.Format("Switch block in SDEngine.Convertions.Converter.Convert() [temperature] did not return.\n\nAttempting to convert {0} from {1} to {2}", temp, from, to));
            }

            /// <summary>
            /// Converts a distance value from one unit to another
            /// </summary>
            /// <param name="from">The <code>DistanceUnit</code> the <code>dist</code> parameter is given in</param>
            /// <param name="to">The <code>DistanceUnit</code> to convert the <code>dist</code> parameter to</param>
            /// <param name="temp">The value to convert</param>
            /// <returns>The converted distance</returns>
            public static double? Convert(DistanceUnit from, DistanceUnit to, double dist) {
                switch (from) {
                    case DistanceUnit.Miles:
                        switch (to) {
                            case DistanceUnit.Miles:
                                return dist;
                            case DistanceUnit.Feet:
                                return dist * 5280;
                            default:
                                return null;
                        }
                    case DistanceUnit.Meters:
                        switch (to) {
                            case DistanceUnit.Kilometers:
                                return dist / 1000;
                            case DistanceUnit.Meters:
                                return dist;
                            case DistanceUnit.Miles:
                                return dist * 0.00062137;
                            default:
                                return null;
                        }
                    default:
                        return null;
                }
            }
        }
    }
    namespace Memory {
        namespace Utility {
            /// <summary>
            /// Defines a category of units
            /// </summary>
            public enum UnitType {
                Temperature,
                Distance
            }

            /// <summary>
            /// Setting keys to be used when retrieving settings
            /// </summary>
            public static class Keys {
                public const string Time = "Time"; //double
                public const string Temp = "Temp"; //double
                public const string TempUnit = "TempUnit"; //int
                public const string DistUnit = "DistUnit"; //int
                public const string AutoGet = "AutoGet"; //bool
                public const string AutoConvert = "AutoConvert"; //bool
                public const string VerboseMode = "VerboseMode"; //bool
                public const string VerboseModeData = "VerboseModeData"; //List<bool>
                public const string ForceUpgrade = "ForcedUpgrade"; //bool
            }

            /// <summary>
            /// Methods to be used to get settings and convert values
            /// </summary>
            public static class UtilityMethods {
                private static IPropertySet settings = ApplicationData.Current.LocalSettings.Values;

                /// <summary>
                /// Checks if a key is present in the settings set
                /// </summary>
                /// <param name="key">The key to check for</param>
                /// <returns>True if the key exists, false if not</returns>
                public static bool Contains(string key) {
                    return settings.ContainsKey(key);
                }

                /// <summary>
                /// Converts the string representation of a distance or temperature unit to its integer counterpart
                /// </summary>
                /// <param name="scope">The type of unit</param>
                /// <param name="input">The string representation of the unit</param>
                /// <returns>The integer representation of the unit</returns>
                public static int? StringToInt(UnitType scope, string input) {
                    switch (scope) {
                        case UnitType.Temperature:
                            switch (input.ToLower()) {
                                case "fahrenheit":
                                    return 0;
                                case "celsius":
                                    return 1;
                                case "kelvin":
                                    return 2;
                            }
                            break;
                        case UnitType.Distance:
                            switch (input.ToLower()) {
                                case "feet/miles":
                                    return 0;
                                case "meters/kilometers":
                                    return 1;
                            }
                            break;
                        default:
                            return null;
                    }
                    return null;
                }

                /// <summary>
                /// Converts the integer value of a unit to its string counterpart
                /// </summary>
                /// <param name="scope">The type of unit</param>
                /// <param name="input">The integer representation of the unit</param>
                /// <returns>The string representation of the unit</returns>
                public static string IntToString(UnitType scope, int input) {
                    switch (scope) {
                        case UnitType.Temperature:
                            switch (input) {
                                case 0:
                                    return "Fahrenheit";
                                case 1:
                                    return "Celsius";
                                case 2:
                                    return "Kelvin";
                            }
                            break;
                        case UnitType.Distance:
                            switch (input) {
                                case 0:
                                    return "Feet/Miles";
                                case 1:
                                    return "Meters/Kilometers";
                            }
                            break;
                        default:
                            return null;
                    }
                    return null;
                }

                /// <summary>
                /// Gets an object from the settings set
                /// </summary>
                /// <typeparam name="TValue">The type to cast the retrieved value to</typeparam>
                /// <param name="key">The key for the object</param>
                /// <returns>The object requested</returns>
                public static TValue Get<TValue>(string key) {
                    return (TValue)settings[key];
                }

                /// <summary>
                /// Sets an object in the settings set to the given value
                /// </summary>
                /// <typeparam name="TValue">The type the input will be</typeparam>
                /// <param name="key">The key of the object to replace</param>
                /// <param name="value">The new object</param>
                public static void Set<TValue>(string key, TValue value) {
                    settings[key] = value;
                }
            }
        }

        /// <summary>
        /// The main manager for memory; use this to set values
        /// </summary>
        public static class Manager {
            /// <summary>
            /// The value last used in the time field
            /// </summary>
            public static double Time {
                get {
                    return UtilityMethods.Get<double>(Keys.Time);
                }
                set {
                    UtilityMethods.Set<double>(Keys.Time, value);
                }
            }

            /// <summary>
            /// The last value used in the temperature field
            /// </summary>
            public static double Temp {
                get {
                    return UtilityMethods.Get<double>(Keys.Temp);
                }
                set {
                    UtilityMethods.Set<double>(Keys.Temp, value);
                }
            }

            /// <summary>
            /// The unit to display temperature measurements as
            /// </summary>
            public static int TempUnit {
                get {
                    return UtilityMethods.Get<int>(Keys.TempUnit);
                }
                set {
                    UtilityMethods.Set<int>(Keys.TempUnit, value);
                }
            }

            /// <summary>
            /// The unit to display distance measurements as
            /// </summary>
            public static int DistUnit {
                get {
                    return UtilityMethods.Get<int>(Keys.DistUnit);
                }
                set {
                    UtilityMethods.Set<int>(Keys.DistUnit, value);
                }
            }

            /// <summary>
            /// Determines if StrikeDistance will automatically get weather data on startup
            /// </summary>
            public static bool AutoGet {
                get {
                    return UtilityMethods.Get<bool>(Keys.AutoGet);
                }
                set {
                    UtilityMethods.Set<bool>(Keys.AutoGet, value);
                }
            }

            /// <summary>
            /// Determines if StrikeDistance will convert numbers when units are changed
            /// </summary>
            public static bool AutoConvert {
                get {
                    return UtilityMethods.Get<bool>(Keys.AutoConvert);
                }
                set {
                    UtilityMethods.Set<bool>(Keys.AutoConvert, value);
                }
            }

            /// <summary>
            /// Determines if Verbose Mode will be enabled
            /// </summary>
            public static bool VerboseMode {
                get {
                    return UtilityMethods.Get<bool>(Keys.VerboseMode);
                }
                set {
                    UtilityMethods.Set<bool>(Keys.VerboseMode, value);
                }
            }

            /// <summary>
            /// The data verbose mode will append
            /// </summary>
            public static List<bool> VerboseModeData {
                get {
                    return UtilityMethods.Get<List<bool>>(Keys.VerboseModeData);
                }
                set {
                    UtilityMethods.Set<List<bool>>(Keys.VerboseModeData, value);
                }
            }
        }
    }

    /// <summary>
    /// The core of SDEngine
    /// </summary>
    public static class Main {
        /// <summary>
        /// Gets the speed of sound in the current temperature
        /// </summary>
        /// <param name="Celsius">The current temperature in celsius</param>
        /// <returns>The speed of sound</returns>
        private static double GetSpeedOfSound(double Celsius) {
            return (331.5 + 0.6 * Celsius);
        }

        /// <summary>
        /// Gets the distance from you to the lightning strike
        /// </summary>
        /// <param name="SpeedOfSound">The speed of sound</param>
        /// <param name="Time">The time between the flash and boom</param>
        /// <returns>The distance from you to the lightning strike</returns>
        private static double GetDistance(double SpeedOfSound, double Time) {
            return (SpeedOfSound * Time);
        }

        /// <summary>
        /// Calculates everything and formats it for the result message
        /// </summary>
        /// <param name="Temperature">The current temperature in any unit</param>
        /// <param name="Time">The time between the flash and boom</param>
        /// <returns>The formatted result message</returns>
        public static string Calculate(double Temperature, double Time) {
            double temp, res;
            int tempunit, distunit;
            string finaldist = "";
            tempunit = Memory.Manager.TempUnit;
            distunit = Memory.Manager.DistUnit;
            switch (tempunit) {
                case 0:
                    temp = Convertions.Converter.Convert(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, Temperature);
                    break;
                case 2:
                    temp = Convertions.Converter.Convert(TemperatureUnit.Kelvin, TemperatureUnit.Celsius, Temperature);
                    break;
                default:
                    temp = Temperature;
                    break;
            }
            res = GetDistance(GetSpeedOfSound(temp), Time);
            switch (distunit) {
                case 0:
                    res = (double)Convertions.Converter.Convert(DistanceUnit.Meters, DistanceUnit.Miles, res);
                    if (res < 1) {
                        res = (double)Convertions.Converter.Convert(DistanceUnit.Miles, DistanceUnit.Feet, res);
                        if (res == 1)
                            finaldist = "foot";
                        else
                            finaldist = "feet";
                    }
                    else {
                        if (res == 1)
                            finaldist = "mile";
                        else
                            finaldist = "miles";
                    }
                    break;
                case 1:
                    if (res >= 1000) {
                        res = (double)Convertions.Converter.Convert(DistanceUnit.Meters, DistanceUnit.Kilometers, res);
                        if (res == 1)
                            finaldist = "kilometer";
                        else
                            finaldist = "kilometers";
                    }
                    else {
                        if (res == 1)
                            finaldist = "meter";
                        else
                            finaldist = "meters";
                    }
                    break;
            }
            res = Math.Round(res, 2);
            return string.Format("This lighting struck approximately {0} {1} away.", res, finaldist);
        }

        /// <summary>
        /// Checks if the device has an Internet connection
        /// </summary>
        /// <returns>True if connected, false if not</returns>
        public static bool CheckConnection() {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        /// <summary>
        /// Gets the current temperature in Celsius from WorldWeatherOnline
        /// </summary>
        /// <returns>The temperature in Celsius</returns>
        public static async Task<double> GetTemperature() {
            Debug.WriteLine(new { SynchronizationContext.Current });
            if (CheckConnection()) {
                Geolocator Locator = new Geolocator();
                Geoposition Position = await Locator.GetGeopositionAsync();
                Geocoordinate Coordinate = Position.Coordinate;
                HttpClient Client = new HttpClient();
                double Temperature;
                Uri u = new Uri(string.Format("http://api.worldweatheronline.com/free/v1/weather.ashx?q={0},{1}&format=xml&num_of_days=1&date=today&cc=yes&key={2}",
                                              Coordinate.Point.Position.Latitude,
                                              Coordinate.Point.Position.Longitude,
                                              "63c11a7d6961b161060643766ca5df0e8f7fc3fd"),
                                              UriKind.Absolute);
                string Raw = await Client.GetStringAsync(u);
                XElement main = XElement.Parse(Raw), current_condition, temp_c;
                current_condition = main.Element("current_condition");
                temp_c = current_condition.Element("temp_C");
                Temperature = Convert.ToDouble(temp_c.Value);
                switch (Memory.Manager.TempUnit) {
                    case 0:
                        Temperature = Convertions.Converter.Convert(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit, Temperature);
                        break;
                    case 1:
                        break;
                    case 2:
                        Temperature = Convertions.Converter.Convert(TemperatureUnit.Celsius, TemperatureUnit.Kelvin, Temperature);
                        break;
                }
                return Temperature;
            }
            else {
                throw new InvalidOperationException("StrikeDistance cannot connect to the weather server.");
            }
        }
    }
}