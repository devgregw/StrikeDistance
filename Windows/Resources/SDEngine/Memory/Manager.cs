using SDEngine.Memory.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEngine.Memory {
    /// <summary>
    /// The main manager for memory; use this to set values
    /// </summary>
    public static class Manager {
        /// <summary>
        /// The value last used in the time field
        /// </summary>
        public static double Time {
            get {
                return UtilityMethods.Get<double>(Keys.Time);
            }
            set {
                UtilityMethods.Set<double>(Keys.Time, value);
            }
        }

        /// <summary>
        /// The last value used in the temperature field
        /// </summary>
        public static double Temp {
            get {
                return UtilityMethods.Get<double>(Keys.Temp);
            }
            set {
                UtilityMethods.Set<double>(Keys.Temp, value);
            }
        }

        /// <summary>
        /// The unit to display temperature measurements as
        /// </summary>
        public static int TempUnit {
            get {
                return UtilityMethods.Get<int>(Keys.TempUnit);
            }
            set {
                UtilityMethods.Set<int>(Keys.TempUnit, value);
            }
        }

        /// <summary>
        /// The unit to display distance measurements as
        /// </summary>
        public static int DistUnit {
            get {
                return UtilityMethods.Get<int>(Keys.DistUnit);
            }
            set {
                UtilityMethods.Set<int>(Keys.DistUnit, value);
            }
        }

        /// <summary>
        /// Determines if StrikeDistance will automatically get weather data on startup
        /// </summary>
        public static bool AutoGet {
            get {
                return UtilityMethods.Get<bool>(Keys.AutoGet);
            }
            set {
                UtilityMethods.Set<bool>(Keys.AutoGet, value);
            }
        }

        /// <summary>
        /// Determines if StrikeDistance will convert numbers when units are changed
        /// </summary>
        public static bool AutoConvert {
            get {
                return UtilityMethods.Get<bool>(Keys.AutoConvert);
            }
            set {
                UtilityMethods.Set<bool>(Keys.AutoConvert, value);
            }
        }

        /// <summary>
        /// Determines if Verbose Mode will be enabled
        /// </summary>
        public static bool VerboseMode {
            get {
                return UtilityMethods.Get<bool>(Keys.VerboseMode);
            }
            set {
                UtilityMethods.Set<bool>(Keys.VerboseMode, value);
            }
        }

        /// <summary>
        /// The data verbose mode will append
        /// </summary>
        public static List<bool> VerboseModeData {
            get {
                return UtilityMethods.Get<List<bool>>(Keys.VerboseModeData);
            }
            set {
                UtilityMethods.Set<List<bool>>(Keys.VerboseModeData, value);
            }
        }
    }
}
