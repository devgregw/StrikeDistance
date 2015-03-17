using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace SDEngine.Memory.Utility {
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
