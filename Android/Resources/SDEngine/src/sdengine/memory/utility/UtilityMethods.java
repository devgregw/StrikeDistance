package sdengine.memory.utility;

import android.annotation.SuppressLint;
import android.content.SharedPreferences;
import java.util.*;

/**
 * @author Greg Whatley
 */
public class UtilityMethods {
	public static boolean Contains(SharedPreferences pref, String key) {
		return pref.contains(key);
	}

	@SuppressLint("DefaultLocale")
	public static int StringToInt(UnitType scope, String input) {
		switch (scope) {
		case Temperature:
			switch (input.toLowerCase()) {
			case "fahrenheit":
				return 0;
			case "celsius":
				return 1;
			case "kelvin":
				return 2;
			default:
				return 9;
			}
		case Distance:
			switch (input.toLowerCase(Locale.US)) {
			case "feet/miles":
				return 0;
			case "meters/kilometers":
				return 1;
			default:
				return 9;
			}
		}
		return 9;
	}
	
	public static String IntToString(UnitType scope, int input) {
		switch (scope) {
		case Temperature:
			switch (input) {
			case 0:
				return "Fahrenheit";
			case 1:
				return "Celsius";
			case 2:
				return "Kelvin";
			default:
				return "Error";
			}
		case Distance:
			switch (input) {
			case 0:
				return "Feet/Miles";
			case 1:
				return "Meters/Kilometers";
			default:
				return "Error";
			}
		}
		return "Error";
	}
	
	public static boolean Get(SharedPreferences pref, String key, boolean defaultValue) {
		return pref.getBoolean(key, defaultValue);
	}
	
	public static float Get(SharedPreferences pref, String key, float defaultValue) {
		return pref.getFloat(key, defaultValue);
	}
	
	public static int Get(SharedPreferences pref, String key, int defaultValue) {
		return pref.getInt(key, defaultValue);
	}
	
	public static Set<String> Get(SharedPreferences pref, String key, Set<String> defaultValue) {
		return pref.getStringSet(key, defaultValue);
	}
	
	public static void Set(SharedPreferences pref, String key, boolean newValue) {
		pref.edit().putBoolean(key, newValue);
	}
	
	public static void Set(SharedPreferences pref, String key, float newValue) {
		pref.edit().putFloat(key, newValue);
	}
	
	public static void Set(SharedPreferences pref, String key, int newValue) {
		pref.edit().putInt(key, newValue);
	}
	
	public static void Set(SharedPreferences pref, String key, Set<String> newValue) {
		pref.edit().putStringSet(key, newValue);
	}
 }
