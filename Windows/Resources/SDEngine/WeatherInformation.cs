using SDEngine.Convertions;
using SDEngine.Convertions.Enumerations;
using SDEngine.Memory;
using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace SDEngine {
    public class WeatherInformation {
        private XDocument sourceDocument;

        public double Latitude {
            get; internal set;
        }

        public double Longitude {
            get; internal set;
        }

        public double Elevation {
            get; internal set;
        }

        public string ConditionString {
            get; internal set;
        }

        public double Temperature {
            get; internal set;
        }

        public double FeelsLike {
            get; internal set;
        }

        public string Humidity {
            get; internal set;
        }

        public string WindDirection {
            get; internal set;
        }

        public double WindSpeed {
            get; internal set;
        }

        public double WindGustSpeed {
            get; internal set;
        }

        public double Pressure {
            get; internal set;
        }

        public Uri ForecastUrl {
            get; internal set;
        }

        public Uri HistoryUrl {
            get; internal set;
        }

        public void Update() {
            XElement root = sourceDocument.Element("response"),
                     observation = root.Element("current_observation"),
                     location = observation.Element("display_location");
            Temperature = (Manager.TempUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "temp_f").Value),
                    2) :
                (Manager.TempUnit == 1) ?
                    Math.Round(
                        double.Parse(
                            observation.Element(
                                "temp_c").Value),
                        2) :
                    Math.Round(
                        Converter.Convert(
                            TemperatureUnit.Kelvin,
                            TemperatureUnit.Celsius,
                            double.Parse(
                                observation.Element(
                                    "temp_c").Value)),
                        2);
            FeelsLike = (Manager.TempUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "feelslike_f").Value),
                    2) :
                (Manager.TempUnit == 1) ?
                    Math.Round(
                        double.Parse(
                            observation.Element(
                                "feelslike_c").Value),
                        2) :
                    Math.Round(
                        Converter.Convert(
                            TemperatureUnit.Celsius,
                            TemperatureUnit.Kelvin,
                            double.Parse(
                                observation.Element(
                                    "feelslike_c").Value)),
                        2);
            WindSpeed = (Manager.SpeedUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "wind_mph").Value),
                    2) :
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "wind_kph").Value),
                    2);
            WindGustSpeed = (Manager.SpeedUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "wind_gust_mph").Value),
                    2) :
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "wind_gust_kph").Value),
                    2);
            Pressure = (Manager.PressureUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "pressure_in").Value),
                    2) :
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "pressure_mb").Value),
                    2);
            Manager.Temp = Temperature;
        }

        public WeatherInformation(string xml) {
            sourceDocument = XDocument.Parse(xml);
            XElement root = sourceDocument.Element("response"),
                     observation = root.Element("current_observation"),
                     location = observation.Element("display_location");
            Latitude = Math.Round(
                double.Parse(
                    location.Element(
                        "latitude").Value),
                2);
            Longitude = Math.Round(
                double.Parse(
                    location.Element(
                        "longitude").Value),
                2);
            Elevation = Math.Round(
                double.Parse(
                    location.Element(
                        "elevation").Value),
                2);
            ConditionString = observation.Element("weather").Value;
            Debug.WriteLine("Temperature");
            Temperature = (Manager.TempUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "temp_f").Value),
                    2) :
                (Manager.TempUnit == 1) ?
                    Math.Round(
                        double.Parse(
                            observation.Element(
                                "temp_c").Value),
                        2) :
                    Math.Round(
                        Converter.Convert(
                            TemperatureUnit.Kelvin,
                            TemperatureUnit.Celsius,
                            double.Parse(
                                observation.Element(
                                    "temp_c").Value)),
                        2);
            Debug.WriteLine("FeelsLike");
            FeelsLike = (Manager.TempUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "feelslike_f").Value),
                    2) :
                (Manager.TempUnit == 1) ?
                    Math.Round(
                        double.Parse(
                            observation.Element(
                                "feelslike_c").Value),
                        2) :
                    Math.Round(
                        Converter.Convert(
                            TemperatureUnit.Celsius,
                            TemperatureUnit.Kelvin,
                            double.Parse(
                                observation.Element(
                                    "feelslike_c").Value)),
                        2);
            Humidity = observation.Element("relative_humidity").Value;
            WindDirection = observation.Element("wind_dir").Value;
            Debug.WriteLine("WindSpeed");
            WindSpeed = (Manager.SpeedUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "wind_mph").Value),
                    2) :
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "wind_kph").Value),
                    2);
            Debug.WriteLine("WindGustSpeed");
            WindGustSpeed = (Manager.SpeedUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "wind_gust_mph").Value),
                    2) :
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "wind_gust_kph").Value),
                    2);
            Debug.WriteLine("Pressure");
            Pressure = (Manager.PressureUnit == 0) ?
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "pressure_in").Value),
                    2) :
                Math.Round(
                    double.Parse(
                        observation.Element(
                            "pressure_mb").Value),
                    2);
            ForecastUrl = new Uri(observation.Element("forecast_url").Value, UriKind.Absolute);
            HistoryUrl = new Uri(observation.Element("history_url").Value, UriKind.Absolute);
            Manager.Temp = Temperature;
            Manager.csource = xml;
        }
    }
}
