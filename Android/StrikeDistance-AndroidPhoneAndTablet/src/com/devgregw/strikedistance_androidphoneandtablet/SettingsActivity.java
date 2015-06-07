package com.devgregw.strikedistance_androidphoneandtablet;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.CompoundButton.OnCheckedChangeListener;
import android.widget.Spinner;
import android.widget.Switch;

public class SettingsActivity extends Activity implements OnItemSelectedListener, OnCheckedChangeListener {

	Spinner tempSpinner, distSpinner, speedSpinner, psrSpinner;
	CheckBox autoGet, vbUnitDetails, vbConvMath, vbCalcMath;
	Switch vbMain;
	SharedPreferences pref;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_settings);
		pref = getSharedPreferences("strikedistance_preferences", Context.MODE_PRIVATE);
		getActionBar().setLogo(R.drawable.ic_logo);
		tempSpinner = (Spinner)findViewById(R.id.tempSpinner);
		distSpinner = (Spinner)findViewById(R.id.distSpinner);
		speedSpinner = (Spinner)findViewById(R.id.speedSpinner);
		psrSpinner = (Spinner)findViewById(R.id.psrSpinner);
		autoGet = (CheckBox)findViewById(R.id.autoGet);
		vbUnitDetails = (CheckBox)findViewById(R.id.vbUnitDetails);
		vbConvMath = (CheckBox)findViewById(R.id.vbConvMath);
		vbCalcMath = (CheckBox)findViewById(R.id.vbCalcMath);
		vbMain = (Switch)findViewById(R.id.vbMain);
		ArrayAdapter<CharSequence> tempSpinnerAdapter = ArrayAdapter.createFromResource(this, R.array.tempUnitsEntries, R.layout.spinner_item);
		tempSpinnerAdapter.setDropDownViewResource(R.layout.spinner_dropdown_item);
		tempSpinner.setAdapter(tempSpinnerAdapter);
		ArrayAdapter<CharSequence> distSpinnerAdapter = ArrayAdapter.createFromResource(this, R.array.distUnitsEntries, R.layout.spinner_item);
		distSpinnerAdapter.setDropDownViewResource(R.layout.spinner_dropdown_item);
		distSpinner.setAdapter(distSpinnerAdapter);
		ArrayAdapter<CharSequence> speedSpinnerAdapter = ArrayAdapter.createFromResource(this, R.array.speedUnitsEntries, R.layout.spinner_item);
		speedSpinnerAdapter.setDropDownViewResource(R.layout.spinner_dropdown_item);
		speedSpinner.setAdapter(speedSpinnerAdapter);
		ArrayAdapter<CharSequence> psrSpinnerAdapter = ArrayAdapter.createFromResource(this, R.array.psrUnitsEntries, R.layout.spinner_item);
		psrSpinnerAdapter.setDropDownViewResource(R.layout.spinner_dropdown_item);
		psrSpinner.setAdapter(psrSpinnerAdapter);
		tempSpinner.setSelection(pref.getInt("tempUnit", 0));
		distSpinner.setSelection(pref.getInt("distUnit", 0));
		speedSpinner.setSelection(pref.getInt("speedUnit", 0));
		psrSpinner.setSelection(pref.getInt("psrUnit", 0));
		autoGet.setChecked(pref.getBoolean("autoGet", true));
		vbUnitDetails.setChecked(pref.getBoolean("vbUnitDetails", false));
		vbConvMath.setChecked(pref.getBoolean("vbConvMath", false));
		vbCalcMath.setChecked(pref.getBoolean("vbCalcMath", false));
		vbMain.setChecked(pref.getBoolean("vbMain", false));
		boolean isChecked = vbMain.isChecked();
		vbUnitDetails.setVisibility((isChecked) ? View.VISIBLE : View.GONE);
		vbConvMath.setVisibility((isChecked) ? View.VISIBLE : View.GONE);
		vbCalcMath.setVisibility((isChecked) ? View.VISIBLE : View.GONE);
		tempSpinner.setOnItemSelectedListener(this);
		distSpinner.setOnItemSelectedListener(this);
		speedSpinner.setOnItemSelectedListener(this);
		psrSpinner.setOnItemSelectedListener(this);
		autoGet.setOnCheckedChangeListener(this);
		vbUnitDetails.setOnCheckedChangeListener(this);
		vbConvMath.setOnCheckedChangeListener(this);
		vbCalcMath.setOnCheckedChangeListener(this);
		vbMain.setOnCheckedChangeListener(this);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.settings, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		return super.onOptionsItemSelected(item);
	}

	@Override
	public void onItemSelected(AdapterView<?> parent, View view, int position,
			long id) {
		Log.d("StrikeDistance", "Preference change: " + parent.getClass() + ", " + position);
		pref = this.getSharedPreferences("strikedistance_preferences", Context.MODE_PRIVATE);
		SharedPreferences.Editor editor = pref.edit();
		if (parent == tempSpinner) {
			editor.putInt("tempUnit", position);
		}
		else if (parent == distSpinner) {
			editor.putInt("distUnit", position);
		}
		else if (parent == speedSpinner) {
			editor.putInt("speedUnit", position);
		}
		else if (parent == psrSpinner) {
			editor.putInt("psrUnit", position);
		}
		editor.commit();
	}

	@Override
	public void onNothingSelected(AdapterView<?> parent) {
		
	}

	@Override
	public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
		Log.d("StrikeDistance", "Preference change: " + buttonView.toString() + ", " + isChecked);
		pref = this.getSharedPreferences("strikedistance_preferences", Context.MODE_PRIVATE);
		SharedPreferences.Editor editor = pref.edit();
		if (buttonView == autoGet) {
			editor.putBoolean("autoGet", isChecked);
		}
		else if (buttonView == vbUnitDetails) {
			editor.putBoolean("vbUnitDetails", isChecked);
		}
		else if (buttonView == vbConvMath) {
			editor.putBoolean("vbConvMath", isChecked);
		}
		else if (buttonView == vbCalcMath) {
			editor.putBoolean("vbCalcMath", isChecked);
		}
		else if (buttonView == vbMain) {
			editor.putBoolean("vbMain", isChecked);
			vbUnitDetails.setVisibility((isChecked) ? View.VISIBLE : View.GONE);
			vbConvMath.setVisibility((isChecked) ? View.VISIBLE : View.GONE);
			vbCalcMath.setVisibility((isChecked) ? View.VISIBLE : View.GONE);
		}
		editor.commit();
	}
}
