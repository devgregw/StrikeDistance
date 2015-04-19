using SDEngine.Convertions.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Devices.Geolocation;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace SDEngine
{
    public static class Main {
        private static double GetSpeedOfSound(double Celsius) {
            return (331.5 + 0.6 * Celsius);
        }

        private static double GetDistance(double SpeedOfSound, double Time) {
            return (SpeedOfSound * Time);
        }
        
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
        
        public static bool CheckConnection() {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
        
        public static async Task<double> GetTemperature(string wukey) {
            if (CheckConnection()) {
                Geolocator Locator = new Geolocator();
                Geoposition Position = await Locator.GetGeopositionAsync();
                Geocoordinate Coordinate = Position.Coordinate;
                HttpClient Client = new HttpClient();
                double Temperature = 0.0;
                XDocument main = XDocument.Parse(
                    await Client.GetStringAsync(
                        new Uri(
                            string.Format(
                                "http://api.wunderground.com/api/{0}/conditions/q/{1},{2}.xml",
                                wukey,
                                Math.Round(Coordinate.Point.Position.Latitude, 3),
                                Math.Round(Coordinate.Point.Position.Longitude, 3)),
                                UriKind.Absolute)));
                await new MessageDialog(
                    string.Format(
                        "{0}, {1}",
                        Math.Round(
                            Coordinate.Point.Position.Latitude,
                            3),
                        Math.Round(
                            Coordinate.Point.Position.Longitude,
                            3))).ShowAsync();
                XElement response, observation, temp_c;
                response = main.Element("response");
                observation = response.Element("current_observation");
                temp_c = observation.Element("temp_c");
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
                return Math.Round(Temperature, 2);
            }
            else {
                throw new InvalidOperationException("StrikeDistance cannot connect to the weather server.");
            }
        }
    }
}
