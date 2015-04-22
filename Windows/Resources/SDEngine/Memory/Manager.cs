using SDEngine.Memory.Utility;
using System.Collections.Generic;
using System.Linq;

namespace SDEngine.Memory {
    public static class Manager {
        public static double Time {
            get {
                return UtilityMethods.Get(Keys.Time, 0.0);
            }
            set {
                UtilityMethods.Set(Keys.Time, value);
            }
        }

        public static double Temp {
            get {
                return UtilityMethods.Get(Keys.Temp, 0.0);
            }
            set {
                UtilityMethods.Set(Keys.Temp, value);
            }
        }
        
        public static int TempUnit {
            get {
                return UtilityMethods.Get(Keys.TempUnit, 0);
            }
            set {
                UtilityMethods.Set(Keys.TempUnit, value);
            }
        }
        
        public static int DistUnit {
            get {
                return UtilityMethods.Get(Keys.DistUnit, 0);
            }
            set {
                UtilityMethods.Set(Keys.DistUnit, value);
            }
        }

        public static int SpeedUnit {
            get {
                return UtilityMethods.Get(Keys.SpeedUnit, 0);
            }
            set {
                UtilityMethods.Set(Keys.SpeedUnit, value);
            }
        }

        public static int PressureUnit {
            get {
                return UtilityMethods.Get(Keys.PressureUnit, 0);
            }
            set {
                UtilityMethods.Set(Keys.PressureUnit, value);
            }
        }
        
        public static bool AutoGet {
            get {
                return UtilityMethods.Get(Keys.AutoGet, true);
            }
            set {
                UtilityMethods.Set(Keys.AutoGet, value);
            }
        }
        
        public static bool WarnPolicy {
            get {
                return UtilityMethods.Get(Keys.WarnPolicy, true);
            }
            set {
                UtilityMethods.Set(Keys.WarnPolicy, value);
            }
        }
        
        public static bool VerboseMode {
            get {
                return UtilityMethods.Get(Keys.VerboseMode, false);
            }
            set {
                UtilityMethods.Set(Keys.VerboseMode, value);
            }
        }
        
        public static List<bool> VerboseModeData {
            get {
                var list = new List<bool>();
                list.Add(UtilityMethods.Get(Keys.vbUnit, false));
                list.Add(UtilityMethods.Get(Keys.vbConv, false));
                list.Add(UtilityMethods.Get(Keys.vbCalc, false));
                return list;
            }
            set {
                UtilityMethods.Set(Keys.vbUnit, value.ElementAt(0));
                UtilityMethods.Set(Keys.vbConv, value.ElementAt(1));
                UtilityMethods.Set(Keys.vbCalc, value.ElementAt(2));
            }
        }

        public static string csource {
            get {
                return UtilityMethods.Get<string>(Keys.CurrentWeatherInformationSource, null);
            }
            set {
                UtilityMethods.Set<string>(Keys.CurrentWeatherInformationSource, value);
            }
        }
    }
}
