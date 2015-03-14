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

namespace SDEngine
{
    public class Engine
    {
        public static class Convertions
        {
            public static class Temperature
            {
                public static double CelsiusToFahrenheit(double C)
                {
                    return C * 9 / 5 + 32;
                }
                public static double CelsiusToKelvin(double C)
                {
                    return C + 273.15;
                }

                public static double FahrenheitToCelsius(double F)
                {
                    return (F - 32) * 5 / 9;
                }
                public static double FahrenheitToKelvin(double F)
                {
                    return (F + 459.67) * 5 / 9;
                }

                public static double KelvinToCelsius(double K)
                {
                    return K - 273.15;
                }
                public static double KelvinToFahrenheit(double K)
                {
                    return K * 9 / 5 - 459.67;
                }
            }
            public static class Distance
            {
                public static double MetersToKilometers(double Meters)
                {
                    return Meters / 1000;
                }
                public static double MetersToMiles(double Meters)
                {
                    return Meters * 0.00062137;
                }

                public static double MilesToFeet(double Miles)
                {
                    return Miles * 5280;
                }
            }
        }
        public static class Memory
        {
            public enum UnitScope
            {
                Temperature, Distance
            }

            private static class Keys
            {
                public const string Time = "Time";
                public const string Temp = "Temp";
                public const string TempUnit = "Units:Temp";
                public const string DistUnit = "Units:Dist";
                public const string DecimalPoints = "DecimalPoints";
                public const string AutoGet = "AutoGet";
                public const string AutoConvert = "AutoConvert";
                public const string AutoRefresh = "AutoRefresh";
                public const string VerboseMode = "VerboseMode";
                public const string VB_UnitDetails = "VerboseMode:UnitDetails";
                public const string VB_ConversionMath = "VerboseMode:ConversionMath";
                public const string VB_CalculationMath = "VerboseMode:CalculationMath";
                public const string Debug_ShowAds = "Debug:ShowAds";
                public const string ForceUpgrade = "ForcedUpgrade";
            }

            public static IPropertySet Settings = ApplicationData.Current.LocalSettings.Values;

            public static bool DoesContain(string Key)
            {
                return Settings.ContainsKey(Key);
            }

            public static bool ForcedUpgrade
            {
                get { return (bool)Settings[Keys.ForceUpgrade]; }
                set { Settings[Keys.ForceUpgrade] = value; }
            }

            public static int StringToInt(UnitScope Scope, string Input)
            {
                switch (Scope)
                {
                    case UnitScope.Temperature:
                        switch (Input.ToLower())
                        {
                            case "fahrenheit": return 0;
                            case "celsius": return 1;
                            case "kelvin": return 2;
                        }
                        break;
                    case UnitScope.Distance:
                        switch (Input.ToLower())
                        {
                            case "feet/miles": return 0;
                            case "meters/kilometers": return 1;
                        }
                        break;
                }
                return 0;
            }
            public static string IntToString(UnitScope Scope, int Input)
            {
                switch (Scope)
                {
                    case UnitScope.Temperature:
                        switch (Input)
                        {
                            case 0: return "Fahrenheit";
                            case 1: return "Celsius";
                            case 2: return "Kelvin";
                        }
                        break;
                    case UnitScope.Distance:
                        switch (Input)
                        {
                            case 0: return "Feet/Miles";
                            case 1: return "Meters/Kilometers";
                        }
                        break;
                }
                return "";
            }

            public static double Time
            {
                get { return Convert.ToDouble(Settings[Keys.Time]); }
                set { Settings[Keys.Time] = value; }
            }
            public static double Temp
            {
                get { return Convert.ToDouble(Settings[Keys.Temp]); }
                set { Settings[Keys.Temp] = value; }
            }

            public static int TempUnit
            {
                get { return Convert.ToInt32(Settings[Keys.TempUnit]); }
                set { if (value != Convert.ToInt32(Settings[Keys.TempUnit])) Settings[Keys.TempUnit] = value; }
            }
            public static int DistUnit
            {
                get { return Convert.ToInt32(Settings[Keys.DistUnit]); }
                set { if (value != Convert.ToInt32(Settings[Keys.DistUnit])) Settings[Keys.DistUnit] = value; }
            }
            public static int DecimalPoints
            {
                get { return Convert.ToInt32(Settings[Keys.DecimalPoints]); }
                set { if (value != Convert.ToInt32(Settings[Keys.DecimalPoints])) Settings[Keys.DecimalPoints] = value; }
            }

            public static bool AutoGet
            {
                get { return Convert.ToBoolean(Settings[Keys.AutoGet]); }
                set { Settings[Keys.AutoGet] = value; }
            }
            public static bool AutoConvert
            {
                get { return Convert.ToBoolean(Settings[Keys.AutoConvert]); }
                set { Settings[Keys.AutoConvert] = value; }
            }

            public static bool VerboseMode
            {
                get { return Convert.ToBoolean(Settings[Keys.VerboseMode]); }
                set { Settings[Keys.VerboseMode] = value; }
            }
            public static bool VB_UnitDetails
            {
                get { return Convert.ToBoolean(Settings[Keys.VB_UnitDetails]); }
                set { Settings[Keys.VB_UnitDetails] = value; }
            }
            public static bool VB_ConversionMath
            {
                get { return Convert.ToBoolean(Settings[Keys.VB_ConversionMath]); }
                set { Settings[Keys.VB_ConversionMath] = value; }
            }
            public static bool VB_CalculationMath
            {
                get { return Convert.ToBoolean(Settings[Keys.VB_CalculationMath]); }
                set { Settings[Keys.VB_CalculationMath] = value; }
            }
        }

        private static double GetSpeedOfSound(double Celsius)
        {
            return (331.5 + 0.6 * Celsius);
        }
        private static double GetDistance(double SpeedOfSound, double Time)
        {
            return (SpeedOfSound * Time);
        }
        public static string Calculate(double Temperature, double Time)
        {
            double temp, res;
            int tempunit, distunit;
            string finaldist = "";
            tempunit = Memory.TempUnit;
            distunit = Memory.DistUnit;
            switch (tempunit)
            {
                case 0:
                    temp = Convertions.Temperature.FahrenheitToCelsius(Temperature);
                    break;
                case 2:
                    temp = Convertions.Temperature.KelvinToCelsius(Temperature);
                    break;
                default:
                    temp = Temperature;
                    break;
            }
            res = GetDistance(GetSpeedOfSound(temp), Time);
            switch (distunit)
            {
                case 0:
                    res = Convertions.Distance.MetersToMiles(res);
                    if (res < 1)
                    {
                        res = Convertions.Distance.MilesToFeet(res);
                        if (res == 1)
                            finaldist = "foot";
                        else
                            finaldist = "feet";
                    }
                    else
                    {
                        if (res == 1)
                            finaldist = "mile";
                        else
                            finaldist = "miles";
                    }
                    break;
                case 1:
                    if (res >= 1000)
                    {
                        res = Convertions.Distance.MetersToKilometers(res);
                        if (res == 1)
                            finaldist = "kilometer";
                        else
                            finaldist = "kilometers";
                    }
                    else
                    {
                        if (res == 1)
                            finaldist = "meter";
                        else
                            finaldist = "meters";
                    }
                    break;
            }
            res = Math.Round(res, Memory.DecimalPoints);
            return string.Format("This lighting struck approximately {0} {1} away.", res, finaldist);
        }

        public static bool CheckConnection()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        public static async Task<double> GetTemperature()
        {
            Debug.WriteLine(new { SynchronizationContext.Current });
            if (CheckConnection())
            {
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
                switch (Memory.TempUnit)
                {
                    case 0:
                        Temperature = Convertions.Temperature.CelsiusToFahrenheit(Temperature);
                        break;
                    case 1:
                        break;
                    case 2:
                        Temperature = Convertions.Temperature.CelsiusToKelvin(Temperature);
                        break;
                }
                return Temperature;
            }
            else
            {
                throw new InvalidOperationException("StrikeDistance cannot connect to the weather server.");
            }
        }
    }
}
