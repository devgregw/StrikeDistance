using SDEngine.Memory.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        public static bool AutoGet {
            get {
                return UtilityMethods.Get(Keys.AutoGet, true);
            }
            set {
                UtilityMethods.Set(Keys.AutoGet, value);
            }
        }
        
        public static bool AutoConvert {
            get {
                return UtilityMethods.Get(Keys.AutoConvert, true);
            }
            set {
                UtilityMethods.Set(Keys.AutoConvert, value);
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
                return UtilityMethods.Get(Keys.VerboseModeData, new List<bool>() { false, false, false });
            }
            set {
                UtilityMethods.Set(Keys.VerboseModeData, value);
            }
        }
    }
}
