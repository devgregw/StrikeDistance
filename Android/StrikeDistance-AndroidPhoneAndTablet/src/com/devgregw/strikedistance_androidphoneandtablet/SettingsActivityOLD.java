package com.devgregw.strikedistance_androidphoneandtablet;

import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.OnSharedPreferenceChangeListener;
import android.os.Bundle;
import android.preference.ListPreference;
import android.preference.Preference;
import android.preference.PreferenceActivity;
import android.preference.PreferenceFragment;
import android.preference.PreferenceGroup;

public class SettingsActivityOLD extends PreferenceActivity {

	public static OnSharedPreferenceChangeListener changeListener;
	public static SharedPreferences preferences;

	public static class SettingsFragment extends PreferenceFragment {

		public SharedPreferences pref2;

		public SettingsFragment() {

		}

		@Override
		public void onCreate(Bundle savedInstanceState) {
			super.onCreate(savedInstanceState);
			addPreferencesFromResource(R.xml.preferences);
			preferences = getPreferenceScreen().getSharedPreferences();
			changeListener = new OnSharedPreferenceChangeListener() {

				@Override
				public void onSharedPreferenceChanged(
						SharedPreferences sharedPreferences, String key) {
					Preference preference = findPreference(key);
					SharedPreferences.Editor e = pref2.edit();
					if (preference instanceof ListPreference) {
						ListPreference listPreference = (ListPreference) preference;
						String entry = listPreference.getEntry().toString();
						listPreference.setSummary(entry);
						if (key == "tempUnit")
							e.putInt(key, ((entry == "Fahrenheit") ? 0
									: (entry == "Celsius") ? 1 : 2));
						else if (key == "distUnit")
							e.putInt(key, ((entry == "Feet/Miles") ? 0 : 1));
						else if (key == "speedUnit")
							e.putInt(key, ((entry == "Miles per hour") ? 0 : 1));
						else if (key == "psrUnit")
							e.putInt(key, ((entry == "Inches") ? 0 : 1));
					} else {
						e.putBoolean(key, sharedPreferences.getBoolean(key,
								((key == "autoGet") ? true : false)));
					}
					e.commit();
				}
			};
		}

		@Override
		public void onResume() {
			super.onResume();
			for (int i = 0; i < getPreferenceScreen().getPreferenceCount(); ++i) {
				Preference preference = getPreferenceScreen().getPreference(i);
				if (preference instanceof PreferenceGroup) {
					PreferenceGroup preferenceGroup = (PreferenceGroup) preference;
					for (int j = 0; j < preferenceGroup.getPreferenceCount(); ++j) {
						updatePreference(preferenceGroup.getPreference(j));
					}
				} else {
					updatePreference(preference);
				}
			}
		}

		public void updatePreference(Preference preference) {
			if (preference instanceof ListPreference) {
				ListPreference listPreference = (ListPreference) preference;
				listPreference.setSummary(listPreference.getEntry());
			}
		}
	}

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		final SettingsFragment fragment = new SettingsFragment();
		fragment.pref2 = getSharedPreferences("preferences",
				Context.MODE_PRIVATE);
		getFragmentManager().beginTransaction()
				.replace(android.R.id.content, fragment).commit();

	}

	@Override
	protected void onResume() {
		super.onResume();
		preferences.registerOnSharedPreferenceChangeListener(changeListener);
	}

	@Override
	protected void onPause() {
		super.onPause();
		preferences.unregisterOnSharedPreferenceChangeListener(changeListener);
	}
}
