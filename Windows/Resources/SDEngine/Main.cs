using SDEngine.Convertions.Enumerations;
using SDEngine.Memory;
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
    internal enum endDistConvertion {
        None,
        MeterstoKilometers,
        MeterstoMiles,
        MeterstoMilestoFeet
    }

    internal enum endTempConvertion {
        None,
        KelvintoCelsius,
        FahrenheittoCelsius
    }
    public static class Main {
        private static double GetSpeedOfSound(double Celsius) {
            return (331.5 + 0.6 * Celsius);
        }

        private static double GetDistance(double SpeedOfSound, double Time) {
            return (SpeedOfSound * Time);
        }

        public static string Calculate(double Temperature, double Time) {
            endDistConvertion c = endDistConvertion.None;
            endTempConvertion t = endTempConvertion.None;
            double temp, res;
            int tempunit, distunit;
            string finaldist = "";
            tempunit = Manager.TempUnit;
            distunit = Manager.DistUnit;
            switch (tempunit) {
                case 0:
                    temp = Convertions.Converter.Convert(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, Temperature);
                    t = endTempConvertion.FahrenheittoCelsius;
                    break;
                case 2:
                    temp = Convertions.Converter.Convert(TemperatureUnit.Kelvin, TemperatureUnit.Celsius, Temperature);
                    t = endTempConvertion.KelvintoCelsius;
                    break;
                default:
                    temp = Temperature;
                    t = endTempConvertion.None;
                    break;
            }
            res = GetDistance(GetSpeedOfSound(temp), Time);
            double originaDist = res;
            switch (distunit) {
                case 0:
                    res = (double)Convertions.Converter.Convert(DistanceUnit.Meters, DistanceUnit.Miles, res);
                    if (res < 1) {
                        res = (double)Convertions.Converter.Convert(DistanceUnit.Miles, DistanceUnit.Feet, res);
                        if (res == 1)
                            finaldist = "foot";
                        else
                            finaldist = "feet";
                        c = endDistConvertion.MeterstoMilestoFeet;
                    }
                    else {
                        if (res == 1)
                            finaldist = "mile";
                        else
                            finaldist = "miles";
                        c = endDistConvertion.MeterstoMiles;
                    }
                    break;
                case 1:
                    if (res >= 1000) {
                        res = (double)Convertions.Converter.Convert(DistanceUnit.Meters, DistanceUnit.Kilometers, res);
                        if (res == 1)
                            finaldist = "kilometer";
                        else
                            finaldist = "kilometers";
                        c = endDistConvertion.MeterstoKilometers;
                    }
                    else {
                        if (res == 1)
                            finaldist = "meter";
                        else
                            finaldist = "meters";
                        c = endDistConvertion.None;
                    }
                    break;
            }
            res = Math.Round(res, 2);
            string mainMsg, unitMsg, convMsg, calcMsg, tempConvEqu = "", distConvEqu = "", finalMsg;
            mainMsg = string.Format("The lightning struck approximately {0} {1} away.", res, finaldist);
            unitMsg = string.Format(
                "\n\nTemperature unit: {0}\nDistance unit: {1}",
                (Manager.TempUnit == 0) ?
                    "Fahrenheit" :
                    (Manager.TempUnit == 1) ?
                        "Celsius" :
                        "Kelvin",
                (Manager.DistUnit == 0) ?
                    "Feet and Miles" :
                    "Meters and Kilometers");
            switch (t) {
                case endTempConvertion.FahrenheittoCelsius:
                    tempConvEqu = string.Format(
                        "Convert Fahrenheit to Celsius: ({0} - 32) * 5 / 9 = {1}",
                        Temperature,
                        temp);
                    break;
                case endTempConvertion.KelvintoCelsius:
                    tempConvEqu = string.Format(
                        "Convert Kelvin to Celsius: {0} - 273.15 = {1}",
                        Temperature,
                        temp);
                    break;
                case endTempConvertion.None:
                    tempConvEqu = "Temperature already in Celsius; no math done";
                    break;
            }
            switch (c) {
                case endDistConvertion.MeterstoKilometers:
                    distConvEqu = string.Format(
                        "Convert Meters to Kilometers: {0} / 1000 = {1}",
                        originaDist,
                        res);
                    break;
                case endDistConvertion.MeterstoMiles:
                    distConvEqu = string.Format(
                        "Convert Meters to Miles: {0} / 0.00062137 = {1}",
                        originaDist,
                        res);
                    break;
                case endDistConvertion.MeterstoMilestoFeet:
                    distConvEqu = string.Format(
                        "Convert Meters to Miles then Feet: {0} / 0.00062137 = {1} / 5280 = {2}",
                        originaDist,
                        originaDist / 0.00062137,
                        res);
                    break;
                case endDistConvertion.None:
                    distConvEqu = "Distance set to meters and kilometers; no math done";
                    break;
            }
            convMsg = string.Format(
                "\n\n{0}\n{1}",
                tempConvEqu,
                distConvEqu);
            calcMsg = string.Format(
                "\n\nSpeed of sound = 331.5 + 0.6 * {0}: {1}\nTime = {2}\nDistance = Speed of sound * time\n{1} * {2} = {3}\nDistance = {3}",
                temp,
                GetSpeedOfSound(temp),
                Time,
                GetDistance(GetSpeedOfSound(temp), Time));
            finalMsg = mainMsg;
            if (Manager.VerboseMode) {
                finalMsg += (Manager.VerboseModeData.ToArray()[0]) ? unitMsg : "";
                finalMsg += (Manager.VerboseModeData.ToArray()[1]) ? convMsg : "";
                finalMsg += (Manager.VerboseModeData.ToArray()[2]) ? calcMsg : "";
            }
            return finalMsg;
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
