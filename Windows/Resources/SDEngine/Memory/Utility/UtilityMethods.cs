using System;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace SDEngine.Memory.Utility {
    public static class UtilityMethods {
        private static IPropertySet settings = ApplicationData.Current.LocalSettings.Values;
        
        public static bool Contains(string key) {
            return settings.ContainsKey(key);
        }
        
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
                case UnitType.Speed:
                    switch (input.ToLower()) {
                        case "miles per hour":
                            return 0;
                        case "kilometers per hour":
                            return 1;
                        case "mph":
                            return 0;
                        case "kph":
                            return 1;
                    }
                    break;
                case UnitType.Pressure:
                    switch (input.ToLower()) {
                        case "inches":
                            return 0;
                        case "millibars":
                            return 1;
                        case "mb":
                            return 1;
                    }
                    break;
                default:
                    return null;
            }
            return null;
        }
        
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
                case UnitType.Speed:
                    switch (input) {
                        case 0:
                            return "Miles per Hour";
                        case 1:
                            return "Kilometers per Hour";
                    }
                    break;
                case UnitType.Pressure:
                    switch (input) {
                        case 0:
                            return "Inches";
                        case 1:
                            return "Millibars";
                    }
                    break;
                default:
                    return null;
            }
            return null;
        }
        
        public static TValue Get<TValue>(string key, TValue def) {
            try
            {
                TValue v = (TValue)settings[key];
                return v;
            }
            catch (NullReferenceException)
            {
                return def;
            }
        }
        
        public static void Set<TValue>(string key, TValue value) {
            settings[key] = value;
        }
    }
}
