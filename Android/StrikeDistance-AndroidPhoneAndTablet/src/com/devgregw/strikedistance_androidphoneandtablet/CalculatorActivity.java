package com.devgregw.strikedistance_androidphoneandtablet;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeoutException;

import sdengine.*;
import sdengine.exceptions.NoConnectionException;
import android.app.ActionBar;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
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

	class StrikeDistanceLocationListener implements LocationListener {

		@Override
		public void onLocationChanged(Location location) {
			Log.d("StrikeDistance", String.format("Location changed: (%s, %s)", String.valueOf(location.getLatitude()), String.valueOf(location.getLongitude())));
			latitude = location.getLatitude();
			longitude = location.getLongitude();
		}

		@Override
		public void onStatusChanged(String provider, int status,
				Bundle extras) {
			String ext = "nothing";
			for (int i = 0; i < extras.keySet().toArray().length; i++) {
				Object[] a = extras.keySet().toArray();
				ext += a.toString() + "\n";
			}
			Log.d("StrikeDistance", String.format("Provider status changed: %s, %s, %s", provider, String.valueOf(status), ext));
		}

		@Override
		public void onProviderEnabled(String provider) {
			Log.d("StrikeDistance", "Location provider enabled: " + provider);
		}

		@Override
		public void onProviderDisabled(String provider) {
			Log.d("StrikeDistance", "Location provider disabled: " + provider);
		}
		
	}
	
	double latitude, longitude;
	WeatherInformation w;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_calculator);
		final LocationManager man = (LocationManager)getSystemService(Context.LOCATION_SERVICE);
		LocationListener listener = new StrikeDistanceLocationListener();
		man.requestLocationUpdates(LocationManager.GPS_PROVIDER, 1, 0, listener);
		((Button)findViewById(R.id.action1_1)).setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				EditText field = (EditText)findViewById(R.id.temperatureBox);
				try {
					field.setEnabled(false);
					if (latitude == 0.0 && longitude == 0.0) {
						Location l = man.getLastKnownLocation(LocationManager.GPS_PROVIDER);
						if (l != null) {
							latitude = l.getLatitude();
							longitude = l.getLongitude();
						}
					}
					w = Main.getWeatherInformation(getApplicationContext(), latitude, longitude, "6d5bf5f013871a92", 0, 0, 0);
					if (w.temperature != null) {
						field.setText(String.valueOf(w.temperature));
						((TextView)findViewById(R.id.latitiude)).setText(String.valueOf(latitude));
						((TextView)findViewById(R.id.longitude)).setText(String.valueOf(longitude));
						((TextView)findViewById(R.id.elevation)).setText(String.valueOf(w.elevation));
						((TextView)findViewById(R.id.conditions)).setText(w.condtionString);
						((TextView)findViewById(R.id.temperature)).setText(String.valueOf(w.temperature));
						((TextView)findViewById(R.id.feelslike)).setText(String.valueOf(w.feelsLike));
						((TextView)findViewById(R.id.humidity)).setText(w.humidity);
						((TextView)findViewById(R.id.winddirection)).setText(w.windDirection);
						((TextView)findViewById(R.id.windspeed)).setText(String.valueOf(w.windSpeed));
						((TextView)findViewById(R.id.windgustspeed)).setText(String.valueOf(w.windGustSpeed));
						((TextView)findViewById(R.id.pressure)).setText(w.pressure);
						((Button)findViewById(R.id.viewOnWU)).setEnabled(true);
						((Button)findViewById(R.id.viewHistorical)).setEnabled(true);
						((Button)findViewById(R.id.viewOnWU)).setOnClickListener(new View.OnClickListener() {
							
							@Override
							public void onClick(View v) {
								Intent i = new Intent();
								i.setAction(Intent.ACTION_VIEW);
								i.setData(w.forecastUri);
								startActivity(i);
							}
						});
						((Button)findViewById(R.id.viewHistorical)).setOnClickListener(new View.OnClickListener() {
							
							@Override
							public void onClick(View v) {
								Intent i = new Intent();
								i.setAction(Intent.ACTION_VIEW);
								i.setData(w.historicUri);
								startActivity(i);
							}
						});
					}
					else {
						String u = getString(R.string.unavailable);
						((TextView)findViewById(R.id.latitiude)).setText(u);
						((TextView)findViewById(R.id.longitude)).setText(u);
						((TextView)findViewById(R.id.elevation)).setText(u);
						((TextView)findViewById(R.id.conditions)).setText(u);
						((TextView)findViewById(R.id.temperature)).setText(u);
						((TextView)findViewById(R.id.feelslike)).setText(u);
						((TextView)findViewById(R.id.humidity)).setText(u);
						((TextView)findViewById(R.id.winddirection)).setText(u);
						((TextView)findViewById(R.id.windspeed)).setText(u);
						((TextView)findViewById(R.id.windgustspeed)).setText(u);
						((TextView)findViewById(R.id.pressure)).setText(u);
						((Button)findViewById(R.id.viewOnWU)).setEnabled(false);
						((Button)findViewById(R.id.viewHistorical)).setEnabled(false);
						AlertDialog.Builder b = new AlertDialog.Builder(CalculatorActivity.this, AlertDialog.THEME_DEVICE_DEFAULT_DARK);
						b.setTitle("An error occured!");
						b.setMessage("Something happened and StrikeDistance was unable to get weather information.");
						b.setPositiveButton("Okay", new DialogInterface.OnClickListener() {
							@Override
							public void onClick(DialogInterface dialog,
									int which) {
								dialog.dismiss();
							}
						});
						b.show();
					}
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
		});
		ActionBar b = getActionBar();
		b.setLogo(R.drawable.ic_logo);
	}
	
	@Override
	public void onResume() {
		super.onResume();
	}
	
	@Override
	public void onPause() {
		super.onPause();
	}
	
	@Override
	public void onStop() {
		super.onStop();
	}
	
	@Override
	public void onDestroy() {
		super.onDestroy();
	}
	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.calculator, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
		int id = item.getItemId();
		if (id == R.id.action_settings) {
			startActivity(new Intent(CalculatorActivity.this, SettingsActivity.class));
			return true;
		}
		else if (id == R.id.action_feedback) {
			Intent i = new Intent(Intent.ACTION_SENDTO, Uri.fromParts("mailto", "devgregw@outlook.com", null));
			i.putExtra(Intent.EXTRA_SUBJECT, "StrikeDistance Feedback Submission (Android)");
			startActivity(Intent.createChooser(i, "Send feedback"));
			return true;
		}
		else if (id == R.id.action_calculate) {
			AlertDialog.Builder b = new AlertDialog.Builder(CalculatorActivity.this, AlertDialog.THEME_DEVICE_DEFAULT_DARK);
			b.setTitle("Result");
			b.setPositiveButton("Close", new DialogInterface.OnClickListener() {

				@Override
				public void onClick(DialogInterface dialog, int which) {
					dialog.dismiss();
				}
			});
			b.setCancelable(true);
			b.setMessage(
					Main.calculate(
							Double.parseDouble(
								((EditText)findViewById(R.id.temperatureBox)).getText().toString()),
							Double.parseDouble(
								((EditText)findViewById(R.id.EditText01)).getText().toString()),
							0,
							0,
							false,
							null));
			b.show();
			return true;
		}
		return super.onOptionsItemSelected(item);
	}
}
