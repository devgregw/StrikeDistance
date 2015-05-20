package sdengine.convertions;

import java.text.DecimalFormat;

import sdengine.convertions.enumerations.DistanceUnit;
import sdengine.convertions.enumerations.TemperatureUnit;

public class Converter {
	public static Double round(Double d) {
		DecimalFormat f = new DecimalFormat("#.##");
		return Double.valueOf(f.format(d));
	}
	
	public static Double convert(TemperatureUnit from, TemperatureUnit to, Double temp) {
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
            break;
        case Celsius:
            switch (to) {
            case Fahrenheit:
                return ((temp * 9) / 5) + 32;
            case Celsius:
                return temp;
            case Kelvin:
                return temp + 273.15;
            }
            break;
        case Kelvin:
            switch (to) {
            case Fahrenheit:
                return temp * 9 / 5 - 459.67;
            case Celsius:
                return temp - 273.15;
            case Kelvin:
                return temp;
            }
            break;
		}
		return 0.0;
	}
	
	@SuppressWarnings("incomplete-switch")
	public static Double convert(DistanceUnit from, DistanceUnit to, Double dist) {
		switch (from) {
        case Miles:
            switch (to) {
            case Miles:
                return dist;
            case Feet:
                return dist * 5280;
            }
        case Meters:
            switch (to) {
            case Kilometers:
                return dist / 1000;
            case Meters:
                return dist;
            case Miles:
                return dist * 0.00062137;
            }
		}
		return 0.0;
	}
}
