package com.devgregw.strikedistance_androidphoneandtablet;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeoutException;

import sdengine.Main;
import sdengine.WeatherInformation;
import sdengine.exceptions.NoConnectionException;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

public class CalculatorActivity extends Activity {

	EditText temperatureBox;
	EditText timeBox;
	LocationManager man;
	LocationListener listener;
	Activity act;

	class StrikeDistanceLocationListener implements LocationListener {
		@Override
		public void onLocationChanged(Location location) {
			Log.d("StrikeDistance",
					String.format("Location changed: (%s, %s)",
							String.valueOf(location.getLatitude()),
							String.valueOf(location.getLongitude())));
			latitude = location.getLatitude();
			longitude = location.getLongitude();
		}

		@Override
		public void onStatusChanged(String provider, int status, Bundle extras) {
			String ext = "nothing";
			for (int i = 0; i < extras.keySet().toArray().length; i++) {
				Object[] a = extras.keySet().toArray();
				ext += a.toString() + "\n";
			}
			Log.d("StrikeDistance", String.format(
					"Provider status changed: %s, %s, %s", provider,
					String.valueOf(status), ext));
		}

		@Override
		public void onProviderEnabled(String provider) {

		}

		@Override
		public void onProviderDisabled(String provider) {

		}
	}

	public class LocationButtonListener implements View.OnClickListener {
		@Override
		public void onClick(View v) {
			EditText field = (EditText) findViewById(R.id.temperatureBox);
			try {
				field.setEnabled(false);
				if (latitude == 0.0 && longitude == 0.0) {
					Location l = man
							.getLastKnownLocation(LocationManager.GPS_PROVIDER);
					if (l != null) {
						latitude = l.getLatitude();
						longitude = l.getLongitude();
					}
				}
				WeatherInformationSaver.setWeatherInformation(Main
						.getWeatherInformation(
								CalculatorActivity.this, Boolean.TRUE, latitude,
								longitude, "6d5bf5f013871a92",
								tempUnit, speedUnit, psrUnit));
				updateWeather();
			} catch (NoConnectionException e) {
				e.printStackTrace();
			} catch (TimeoutException e) {
				e.printStackTrace();
			} catch (InterruptedException e) {
				e.printStackTrace();
			} catch (ExecutionException e) {
				e.printStackTrace();
			} finally {
				field.setEnabled(true);
			}
		}
	};
	
	Double latitude = 0.0, longitude = 0.0;
	int tempUnit, distUnit, speedUnit, psrUnit;
	boolean verboseMode;
	boolean[] verboseModeData;

	public void updateWeather() {
		final WeatherInformation w = WeatherInformationSaver
				.getWeatherInformation();
		if (w == null) {
			return;
		} else if (w.temperature != null) {
			w.update(tempUnit, speedUnit, psrUnit);
			temperatureBox.setText(String.valueOf(w.temperature));
			((TextView) findViewById(R.id.latitiude)).setText(String
					.valueOf(w.latitude) + "°");
			((TextView) findViewById(R.id.longitude)).setText(String
					.valueOf(w.longitude) + "°");
			((TextView) findViewById(R.id.elevation)).setText(String
					.valueOf(w.elevation));
			((TextView) findViewById(R.id.conditions))
					.setText(w.condtionString);
			((TextView) findViewById(R.id.temperature))
					.setText(String.valueOf(w.temperature)
							+ ((tempUnit == 0) ? " °F"
									: (tempUnit == 1) ? " °C" : " K"));
			((TextView) findViewById(R.id.feelslike))
					.setText(String.valueOf(w.feelsLike)
							+ ((tempUnit == 0) ? " °F"
									: (tempUnit == 1) ? " °C" : " K"));
			((TextView) findViewById(R.id.humidity)).setText(w.humidity);
			((TextView) findViewById(R.id.winddirection))
					.setText(w.windDirection);
			((TextView) findViewById(R.id.windspeed)).setText(String
					.valueOf(w.windSpeed)
					+ ((speedUnit == 0) ? " MPH" : " KPH"));
			((TextView) findViewById(R.id.windgustspeed)).setText(String
					.valueOf(w.windGustSpeed)
					+ ((speedUnit == 0) ? " MPH" : " KPH"));
			((TextView) findViewById(R.id.pressure)).setText(w.pressure
					+ ((psrUnit == 0) ? " in" : " mb"));
			((Button) findViewById(R.id.viewOnWU)).setEnabled(true);
			((Button) findViewById(R.id.viewHistorical)).setEnabled(true);
			((Button) findViewById(R.id.viewOnWU))
					.setOnClickListener(new View.OnClickListener() {
						@Override
						public void onClick(View v) {
							Intent i = new Intent();
							i.setAction(Intent.ACTION_VIEW);
							i.setData(w.forecastUri);
							startActivity(i);
						}
					});
			((Button) findViewById(R.id.viewHistorical))
					.setOnClickListener(new View.OnClickListener() {
						@Override
						public void onClick(View v) {
							Intent i = new Intent();
							i.setAction(Intent.ACTION_VIEW);
							i.setData(w.historicUri);
							startActivity(i);
						}
					});
		} else {
			String u = getString(R.string.unavailable);
			temperatureBox.setText(getSharedPreferences("hidden_preferences",
					Context.MODE_PRIVATE).getString("temp", "0.0"));
			((TextView) findViewById(R.id.latitiude)).setText(u);
			((TextView) findViewById(R.id.longitude)).setText(u);
			((TextView) findViewById(R.id.elevation)).setText(u);
			((TextView) findViewById(R.id.conditions)).setText(u);
			((TextView) findViewById(R.id.temperature)).setText(u);
			((TextView) findViewById(R.id.feelslike)).setText(u);
			((TextView) findViewById(R.id.humidity)).setText(u);
			((TextView) findViewById(R.id.winddirection)).setText(u);
			((TextView) findViewById(R.id.windspeed)).setText(u);
			((TextView) findViewById(R.id.windgustspeed)).setText(u);
			((TextView) findViewById(R.id.pressure)).setText(u);
			((Button) findViewById(R.id.viewOnWU)).setEnabled(false);
			((Button) findViewById(R.id.viewHistorical)).setEnabled(false);
			AlertDialog.Builder b = new AlertDialog.Builder(
					CalculatorActivity.this,
					AlertDialog.THEME_DEVICE_DEFAULT_DARK);
			b.setTitle("An error occured!");
			b.setMessage("Something happened and StrikeDistance was unable to get weather information.");
			b.setPositiveButton("Okay", new DialogInterface.OnClickListener() {
				@Override
				public void onClick(DialogInterface dialog, int which) {
					dialog.dismiss();
				}
			});
			b.show();
		}
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_calculator);
		act = this;
		temperatureBox = (EditText) findViewById(R.id.temperatureBox);
		timeBox = (EditText) findViewById(R.id.EditText01);
		SharedPreferences pref = getSharedPreferences("strikedistance_preferences", Context.MODE_PRIVATE);
		tempUnit = pref.getInt("tempUnit",  0);
		distUnit = pref.getInt("distUnit", 0);
		speedUnit = pref.getInt("speedUnit", 0);
		psrUnit = pref.getInt("psrUnit", 0);
		verboseMode = pref.getBoolean("vbMain", false);
		verboseModeData = new boolean[] { pref.getBoolean("vbUnitDetails", false), pref.getBoolean("vbConvMath", false), pref.getBoolean("vbCalcMath", false) };
		man = (LocationManager) getSystemService(Context.LOCATION_SERVICE);
		listener = new StrikeDistanceLocationListener();
		man.requestLocationUpdates(LocationManager.GPS_PROVIDER, 1, 0, listener);
		((Button) findViewById(R.id.action1_1)).setOnClickListener(new LocationButtonListener());
		getActionBar().setLogo(R.drawable.ic_logo);
	}

	@Override
	protected void onResume() {
		super.onResume();
		SharedPreferences pref = getSharedPreferences("strikedistance_preferences", Context.MODE_PRIVATE);
		tempUnit = pref.getInt("tempUnit",  0);
		distUnit = pref.getInt("distUnit", 0);
		speedUnit = pref.getInt("speedUnit", 0);
		psrUnit = pref.getInt("psrUnit", 0);
		verboseMode = pref.getBoolean("vbMain", false);
		verboseModeData = new boolean[] { pref.getBoolean("vbUnitDetails", false), pref.getBoolean("vbConvMath", false), pref.getBoolean("vbCalcMath", false) };
		updateWeather();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.calculator, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		int id = item.getItemId();
		if (id == R.id.action_settings) {
			startActivity(new Intent(CalculatorActivity.this,
					SettingsActivity.class));
			return true;
		} else if (id == R.id.action_feedback) {
			Intent i = new Intent(Intent.ACTION_SENDTO, Uri.fromParts("mailto",
					"devgregw@outlook.com", null));
			i.putExtra(Intent.EXTRA_SUBJECT,
					"StrikeDistance Feedback Submission (Android)");
			startActivity(Intent.createChooser(i, "Send feedback"));
			return true;
		} else if (id == R.id.action_calculate) {
			AlertDialog.Builder b = new AlertDialog.Builder(
					CalculatorActivity.this,
					AlertDialog.THEME_DEVICE_DEFAULT_DARK);
			b.setTitle("Result");
			b.setPositiveButton("Close", new DialogInterface.OnClickListener() {
				@Override
				public void onClick(DialogInterface dialog, int which) {
					dialog.dismiss();
				}
			});
			b.setCancelable(true);
			b.setMessage(Main.calculate(Double
					.parseDouble(((EditText) findViewById(R.id.temperatureBox))
							.getText().toString()), Double
					.parseDouble(((EditText) findViewById(R.id.EditText01))
							.getText().toString()), tempUnit, distUnit, verboseMode,
					verboseModeData));
			b.show();
			return true;
		}
		return super.onOptionsItemSelected(item);
	}
}
