package sdengine.convertions;
import sdengine.convertions.enumerations.*;

/**
 * Contains methods for converting one unit to another
 * @author Greg Whatley
 */
public class Converter {
	
	/**
	 * Converts a temperature value from one unit to another
	 * @param from The unit the temperature value is given in
	 * @param to The unit the temperature value is to be converted to
	 * @param temp The temperature value
	 * @return The converted temperature
	 * @throws Exception
	 */
	public static double Convert(TemperatureUnit from, TemperatureUnit to, double temp) throws Exception {
		switch (from) {
		case Fahrenheit:
			switch (to) {
			case Fahrenheit:
				return temp;
			case Celsius:
				return (temp - 32) * 5 / 9;
			case Kelvin:
				return (temp + 459.67) * 5 / 9;
			}
		case Celsius:
			switch (to) {
			case Fahrenheit:
				return temp * 9 / 5 + 32;
			case Celsius:
				return temp;
			case Kelvin:
				return temp + 273.15;
			}
		case Kelvin:
			switch (to) {
			case Fahrenheit:
				return temp * 9 / 5 - 459.67;
			case Celsius:
				return temp - 273.15;
			case Kelvin:
				return temp;
			}
		default:
			throw new java.lang.Exception("Switch block in sdengine.convertions.converter.convert() [temperature] did not return.");
		}
	}
	
	/**
	 * Converts one distance value from one unit to another
	 * @param from The unit the distance value is given in
	 * @param to The unit the distance value is to be converted to
	 * @param dist The distance value
	 * @return The converted distance
	 */
	public static double Convert(DistanceUnit from, DistanceUnit to, double dist) {
		switch (from) {
		case Miles:
			switch (to) {
			case Miles:
				return dist;
			case Feet:
				return dist * 5280;
			default:
				return (Double)null;
			}
		case Meters:
			switch (to) {
			case Kilometers:
				return dist / 1000;
			case Meters:
				return dist;
			case Miles:
				return dist * 0.00062137;
			default:
				return (Double)null;
			}
		default:
			return (Double)null;
		}
	}
}
