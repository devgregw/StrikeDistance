﻿using SDEngine.Convertions.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEngine.Convertions {
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
