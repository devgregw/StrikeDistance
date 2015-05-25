package com.devgregw.strikedistance_androidphoneandtablet;

import android.content.SharedPreferences;
import android.content.SharedPreferences.OnSharedPreferenceChangeListener;
import android.os.Bundle;
import android.preference.PreferenceActivity;
import android.preference.PreferenceFragment;

public class SettingsActivity extends PreferenceActivity {

	PreferenceFragment fragment = new PreferenceFragment() {
		@Override
	    public void onCreate(Bundle savedInstanceState) {
	        super.onCreate(savedInstanceState);
	        addPreferencesFromResource(R.xml.preferences);
	        SharedPreferences m = getPreferences(MODE_PRIVATE);
			m.registerOnSharedPreferenceChangeListener(new OnSharedPreferenceChangeListener() {

				@Override
				public void onSharedPreferenceChanged(
						SharedPreferences sharedPreferences, String key) {
					if (key == getString(R.string.temp_key)) {
						int v = Integer.parseInt(sharedPreferences.getString(key, "0"));
						fragment.findPreference(key).setSummary((v == 0) ? getString(R.string.temp_0) : (v == 1) ? getString(R.string.temp_1) : getString(R.string.temp_2));
					}
					if (key == getString(R.string.dist_key)) {
						int v = Integer.parseInt(sharedPreferences.getString(key, "0"));
						fragment.findPreference(key).setSummary((v == 0) ? getString(R.string.dist_0) : getString(R.string.dist_1));
					}
					if (key == getString(R.string.speed_key)) {
						int v = Integer.parseInt(sharedPreferences.getString(key, "0"));
						fragment.findPreference(key).setSummary((v == 0) ? getString(R.string.speed_0) : getString(R.string.speed_1));
					}
					if (key == getString(R.string.psr_key)) {
						int v = Integer.parseInt(sharedPreferences.getString(key, "0"));
						fragment.findPreference(key).setSummary((v == 0) ? getString(R.string.psr_0) : getString(R.string.psr_1));
					}
				}
			});
	        {
				int v = Integer.parseInt(m.getString(getString(R.string.temp_key), "0"));
				fragment.findPreference(getString(R.string.temp_key)).setSummary((v == 0) ? getString(R.string.temp_0) : (v == 1) ? getString(R.string.temp_1) : getString(R.string.temp_2));
			}
			{
				int v = Integer.parseInt(m.getString(getString(R.string.dist_key), "0"));
				fragment.findPreference(getString(R.string.dist_key)).setSummary((v == 0) ? getString(R.string.dist_0) : getString(R.string.dist_1));
			}
			{
				int v = Integer.parseInt(m.getString(getString(R.string.speed_key), "0"));
				fragment.findPreference(getString(R.string.speed_key)).setSummary((v == 0) ? getString(R.string.speed_0) : getString(R.string.speed_1));
			}
			{
				int v = Integer.parseInt(m.getString(getString(R.string.psr_key), "0"));
				fragment.findPreference(getString(R.string.psr_key)).setSummary((v == 0) ? getString(R.string.psr_0) : getString(R.string.psr_1));
			}
	    };
	};
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getFragmentManager().beginTransaction().replace(android.R.id.content, fragment).commit();
	}
}
