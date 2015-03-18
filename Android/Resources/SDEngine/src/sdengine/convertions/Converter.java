package sdengine.convertions;
import sdengine.convertions.enumerations.*;

/**
 * @author Greg Whatley
 */
public class Converter {
	
	@SuppressWarnings("null")
	public static float Convert(TemperatureUnit from, TemperatureUnit to, float temp) {
		switch (from) {
		case Fahrenheit:
			switch (to) {
			case Fahrenheit:
				return temp;
			case Celsius:
				return (temp - 32f) * 5f / 9f;
			case Kelvin:
				return (temp + 459.67f) * 5f / 9f;
			}
		case Celsius:
			switch (to) {
			case Fahrenheit:
				return temp * 9f / 5f + 32f;
			case Celsius:
				return temp;
			case Kelvin:
				return temp + 273.15f;
			}
		case Kelvin:
			switch (to) {
			case Fahrenheit:
				return temp * 9f / 5f - 459.67f;
			case Celsius:
				return temp - 273.15f;
			case Kelvin:
				return temp;
			}
		default:
			return (Float)null;
		}
	}

	@SuppressWarnings("null")
	public static float Convert(DistanceUnit from, DistanceUnit to, float dist) {
		switch (from) {
		case Miles:
			switch (to) {
			case Miles:
				return dist;
			case Feet:
				return dist * 5280f;
			default:
				return (Float)null;
			}
		case Meters:
			switch (to) {
			case Kilometers:
				return dist / 1000f;
			case Meters:
				return dist;
			case Miles:
				return dist * 0.00062137f;
			default:
				return (Float)null;
			}
		default:
			return (Float)null;
		}
	}
}
