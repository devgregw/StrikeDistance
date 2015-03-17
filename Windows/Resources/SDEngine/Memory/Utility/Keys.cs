using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEngine.Memory.Utility {
    /// <summary>
    /// Setting keys to be used when retrieving settings
    /// </summary>
    public static class Keys {
        public const string Time = "Time"; //Type: double
        public const string Temp = "Temp"; //Type: double
        public const string TempUnit = "TempUnit"; //Type: int
        public const string DistUnit = "DistUnit"; //Type: int
        public const string AutoGet = "AutoGet"; //Type: bool
        public const string AutoConvert = "AutoConvert"; //Type: bool
        public const string VerboseMode = "VerboseMode"; //Type: bool
        public const string VerboseModeData = "VerboseModeData"; //Type: List<bool>
        public const string ForceUpgrade = "ForcedUpgrade"; //Type: bool
    }
}
