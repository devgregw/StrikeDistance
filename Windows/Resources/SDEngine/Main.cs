using SDEngine.Convertions.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Devices.Geolocation;
using Windows.Networking.Connectivity;
using Windows.Web.Http;

namespace SDEngine {
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
