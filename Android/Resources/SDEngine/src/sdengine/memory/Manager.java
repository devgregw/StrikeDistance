package sdengine.memory;

import java.util.Set;
import java.util.TreeSet;

import android.content.SharedPreferences;
import sdengine.memory.utility.*;

/**
 * @author Greg Whatley
 */
public class Manager {
	public static float GetTime(SharedPreferences pref) {
		return UtilityMethods.Get(pref, Keys.Time, 0f);
	}
	
	public static void SetTime(SharedPreferences pref, float time) {
		UtilityMethods.Set(pref, Keys.Time, time);
	}
	
	public static float GetTemp(SharedPreferences pref) {
		return UtilityMethods.Get(pref, Keys.Temp, 0f);
	}
	
	public static void SetTemp(SharedPreferences pref, float temp) {
		UtilityMethods.Set(pref, Keys.Temp, temp);
	}
	
	public static int GetTempUnit(SharedPreferences pref) {
		return UtilityMethods.Get(pref, Keys.TempUnit, 0);
	}
	
	public static void SetTempUnit(SharedPreferences pref, int tempUnit) {
		UtilityMethods.Set(pref, Keys.TempUnit, tempUnit);
	}
	
	public static int GetDistUnit(SharedPreferences pref) {
		return UtilityMethods.Get(pref, Keys.DistUnit, 0);
	}
	
	public static void SetDistUnit(SharedPreferences pref, int distUnit) {
		UtilityMethods.Set(pref, Keys.DistUnit, distUnit);
	}
	
	public static boolean GetAutoGet(SharedPreferences pref) {
		return UtilityMethods.Get(pref, Keys.AutoGet, true);
	}
	
	public static void SetAutoGet(SharedPreferences pref, boolean autoGet) {
		UtilityMethods.Set(pref, Keys.AutoGet, autoGet);
	}
	
	public static boolean GetAutoConvert(SharedPreferences pref) {
		return UtilityMethods.Get(pref, Keys.AutoConvert, true);
	}
	
	public static void SetAutoConvert(SharedPreferences pref, boolean autoConvert) {
		UtilityMethods.Set(pref, Keys.AutoConvert, autoConvert);
	}
	
	public static boolean GetVerboseMode(SharedPreferences pref) {
		return UtilityMethods.Get(pref, Keys.VerboseMode, false);
	}
	
	public static void SetVerboseMode(SharedPreferences pref, boolean verboseMode) {
		UtilityMethods.Set(pref, Keys.VerboseMode, verboseMode);
	}
	
	public static TreeSet<Boolean> GetVerboseModeData(SharedPreferences pref) {
		TreeSet<Boolean> s = new TreeSet<Boolean>();
		TreeSet<String> def = new TreeSet<String>();
		def.add("false");
		def.add("false");
		def.add("false");
		Set<String> ss = UtilityMethods.Get(pref, Keys.VerboseModeData, def);
		Object[] array = ss.toArray();
		for (int i = 0; i < array.length; i++) {
			s.add(Boolean.parseBoolean((String)array[i]));
		}
		return s;
	}
	
	public static void SetVerboseModeData(SharedPreferences pref, boolean unitDetails, boolean convertionMath, boolean calculationMath) {
		TreeSet<String> set = new TreeSet<String>();
		set.add(String.valueOf(unitDetails));
		set.add(String.valueOf(convertionMath));
		set.add(String.valueOf(calculationMath));
		UtilityMethods.Set(pref, Keys.VerboseModeData, set);
	}
}
