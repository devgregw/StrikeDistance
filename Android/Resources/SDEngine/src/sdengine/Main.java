package sdengine;

import java.math.BigDecimal;
import java.util.Locale;

import android.content.Context;
import android.content.SharedPreferences;
import sdengine.memory.Manager;
import sdengine.convertions.Converter;
import sdengine.convertions.enumerations.*;

/**
 * @author Greg Whatley
 */
public class Main {
	public static BigDecimal Round(float d) {
        BigDecimal bd = new BigDecimal(Float.toString(d));
        bd = bd.setScale(2, BigDecimal.ROUND_HALF_UP);       
        return bd;
    }
	
	private static float GetSpeedOfSound(float celsius) {
		return 331.5f + 0.6f * celsius;
	}
	
	private static float GetDistance(float speedOfSound, float time) {
		return speedOfSound * time;
	}
	
	@SuppressWarnings("unused")
	private static String Calculate(Context context, float temperature, float time, String msgFormat) {
		SharedPreferences prefs = context.getSharedPreferences("StrikeDistancePreferences", Context.MODE_PRIVATE);
		float newTemp, result;
		int tempUnit, distUnit;
		String distWord = "";
		tempUnit = Manager.GetTempUnit(prefs);
		distUnit = Manager.GetDistUnit(prefs);
		switch (tempUnit) {
		case 0:
			newTemp = Converter.Convert(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, temperature);
		case 1:
			newTemp = temperature;
		case 2:
			newTemp = Converter.Convert(TemperatureUnit.Kelvin, TemperatureUnit.Celsius, temperature);
		default:
			newTemp = temperature;
		}
		result = GetDistance(GetSpeedOfSound(newTemp), time);
		switch (distUnit) {
		case 0:
			result = Converter.Convert(DistanceUnit.Meters, DistanceUnit.Miles, result);
			if (result < 1f) {
				result = Converter.Convert(DistanceUnit.Miles, DistanceUnit.Feet, result);
				if (result == 1f)
					distWord = "foot";
				else
					distWord = "feet";
			}
			else {
				if (result == 1f)
					distWord = "mile";
				else
					distWord = "miles";
			}
		case 1:
			if (result >= 1000f) {
				result = Converter.Convert(DistanceUnit.Meters, DistanceUnit.Kilometers, result);
				if (result == 1f)
					distWord = "kilometer";
				else
					distWord = "kilometers";
			}
			else {
				if (result == 1)
					distWord = "meter";
				else
					distWord = "meters";
			}
		default:
			if (result >= 1000f) {
				result = Converter.Convert(DistanceUnit.Meters, DistanceUnit.Kilometers, result);
				if (result == 1f)
					distWord = "kilometer";
				else
					distWord = "kilometers";
			}
			else {
				if (result == 1)
					distWord = "meter";
				else
					distWord = "meters";
			}
		}
		BigDecimal d = Round(result);
		return String.format(Locale.US, msgFormat, d.toString(), distWord);
	}
}
