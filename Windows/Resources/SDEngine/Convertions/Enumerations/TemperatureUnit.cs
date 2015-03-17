using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEngine.Convertions.Enumerations {
    /// <summary>
    /// Represents available temperature units
    /// </summary>
    /// <remarks>
    /// Should only be used for SDEngine.Convertions.Converter.Convert()
    /// </remarks>
    public enum TemperatureUnit {
        Fahrenheit,
        Celsius,
        Kelvin
    }
}
