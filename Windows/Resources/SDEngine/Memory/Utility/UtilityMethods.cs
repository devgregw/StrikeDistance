using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
