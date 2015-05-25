package sdengine;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

import android.content.Context;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import sdengine.convertions.Converter;
import sdengine.convertions.enumerations.*;
import sdengine.exceptions.NoConnectionException;

enum endDistConvertion {
	None,
	MeterstoKilometers,
	MeterstoMiles,
	MeterstoMilestoFeet
}

enum endTempConvertion {
	None,
	KelvintoCelsius,
	FahrenheittoCelsius
}

public class Main {
	
	@Deprecated
	public Main() {
		
	}
	
	class AsyncWeatherInformation extends AsyncTask<Intent, Void, String> {
		
		protected String doInBackground(Intent... params) {
			StringBuilder sb = new StringBuilder();
			Bundle data = params[0].getExtras();
			Double lat = data.getDouble("lat");
			Double lon = data.getDouble("lon");
			String apiKey = data.getString("apiKey");
			try {
				URL u = new URL(String.format("http://api.wunderground.com/api/%s/conditions/q/%s,%s.json", apiKey, String.valueOf(Converter.round(lat)), String.valueOf(Converter.round(lon))));
				Log.d("sdengine", u.toExternalForm());
				HttpURLConnection c = (HttpURLConnection)u.openConnection();
				InputStream in = new BufferedInputStream(c.getInputStream());
				BufferedReader br = new BufferedReader( new InputStreamReader(in));
				String line;
				while ((line = br.readLine()) != null) {
					sb.append(line);
				}
				br.close();
				c.disconnect();
				return sb.toString();
			}
			catch (Exception e) {
				e.printStackTrace();
				return null;
			}
		}
		
		@Override
		protected void onPostExecute(String json) {
			
		}
	}
	
	private static Double getSpeedOfSound(Double c) {
		return 331.5 + 0.6 * c;
	}
	
	private static Double getDistance(Double speedOfSound, Double time) {
		return speedOfSound * time;
	}
	
	public static String calculate(Double temp, Double time, int tempUnit, int distUnit, boolean verboseMode, boolean[] verboseModeData) {
		endDistConvertion c = endDistConvertion.None;
		endTempConvertion t = endTempConvertion.None;
		Double newTemp, result;
		String finalDist = "";
		switch (tempUnit) {
		case 0:
			newTemp = Converter.convert(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, temp);
			t = endTempConvertion.FahrenheittoCelsius;
		case 2:
			newTemp = Converter.convert(TemperatureUnit.Kelvin, TemperatureUnit.Celsius, temp);
			t = endTempConvertion.KelvintoCelsius;
		default:
			newTemp = temp;
			t = endTempConvertion.None;
		}
		result = getDistance(getSpeedOfSound(temp), time);
		Double originalDist = result;
		switch (distUnit) {
		case 0:
			result = Converter.convert(DistanceUnit.Meters, DistanceUnit.Miles, result);
			if (result < 1) {
				result = Converter.convert(DistanceUnit.Miles,  DistanceUnit.Feet, result);
				if (result == 1) {
					finalDist = "foot";
				}
				else {
					finalDist = "feet";
				}
				c = endDistConvertion.MeterstoMilestoFeet;
			}
			else {
				if (result == 1) {
					finalDist = "mile";
				}
				else {
					finalDist = "miles";
				}
				c = endDistConvertion.MeterstoMiles;
			}
		case 1:
			if (result >= 1000) {
				result = Converter.convert(DistanceUnit.Meters,  DistanceUnit.Kilometers, result);
				if (result == 1) {
					finalDist = "kilometer";
				}
				else {
					finalDist = "kilometers";
				}
				c = endDistConvertion.MeterstoKilometers;
			}
			else {
				if (result == 1) {
					finalDist = "meter";
				}
				else {
					finalDist = "meters";
				}
				c = endDistConvertion.None;
			}
		}
		result = Converter.round(result);
		String mainMsg, unitMsg, convMsg, calcMsg, tempConvEqu = "", distConvEqu = "", finalMsg;
		mainMsg = String.format("The lightning struck approximately %s %s away.", String.valueOf(result), finalDist);
		unitMsg = String.format("\n\nTemperature unit: %s\nDistance unit: %s", (tempUnit == 0) ? "Fahrenheit" : (tempUnit == 1) ? "Celsius" : "Kelvin", (distUnit == 0) ? "Feet and Miles" : "Meters and Kilometers");
		switch (t) {
		case FahrenheittoCelsius:
			tempConvEqu = String.format("Convert Fahrenheit to Celsius: (%s - 32) * 5 / 9 = %s", String.valueOf(temp), String.valueOf(newTemp));
		case KelvintoCelsius:
			tempConvEqu = String.format("Convert Kelvin to Celsius: %s - 273.15 = %s", String.valueOf(temp), String.valueOf(newTemp));
		case None:
			tempConvEqu = "Temperature already on Celsius; no math done";
		}
		switch (c) {
		case MeterstoKilometers:
			distConvEqu = String.format("Convert Meters to Kilometers: %s / 1000 = %s", String.valueOf(originalDist), String.valueOf(result));
		case MeterstoMiles:
			distConvEqu = String.format("Convert Meters to Miles: %s / 0.00062137 = %s", String.valueOf(originalDist), String.valueOf(result));
		case MeterstoMilestoFeet:
			distConvEqu = String.format("Convert Meters to Miles then Feet: %s / 0.00062137 = %s / 5280 = %s", String.valueOf(originalDist), String.valueOf(originalDist / 0.00062137), String.valueOf(result));
		case None:
			distConvEqu = "Distance set to Meters and Kilometers; no math done";
		}
		convMsg = String.format("\n\n%s\n%s", tempConvEqu, distConvEqu);
		calcMsg = String.format("\n\nSpeed of sound: 331.5 + 0.6 * %s = %s\nTime: %s\nDistance: Speed of sound * time\n%s * %s = %s\nDistance = %s", String.valueOf(newTemp), String.valueOf(getSpeedOfSound(newTemp)), String.valueOf(time), String.valueOf(getSpeedOfSound(newTemp)), String.valueOf(time), String.valueOf(getDistance(getSpeedOfSound(newTemp), time)), String.valueOf(getDistance(getSpeedOfSound(newTemp),time)));
		finalMsg = mainMsg;
		if (verboseMode) {
			finalMsg += (verboseModeData[0]) ? unitMsg : "";
			finalMsg += (verboseModeData[1]) ? convMsg : "";
			finalMsg += (verboseModeData[2]) ? calcMsg : "";
		}
		return finalMsg;
	}
	
	public static boolean checkNetworkState(Context c) {
		ConnectivityManager m = (ConnectivityManager)c.getSystemService(Context.CONNECTIVITY_SERVICE);
		return m.getActiveNetworkInfo().isConnected();
	}
	
	public static WeatherInformation getWeatherInformation(Context context, Double lat, Double lon, String apiKey, int tempUnit, int psrUnit, int speedUnit) throws NoConnectionException, TimeoutException, InterruptedException, ExecutionException {
		if (checkNetworkState(context)) {
			Intent i = new Intent();
			i.putExtra("lat", lat);
			i.putExtra("lon", lon);
			i.putExtra("apiKey", apiKey);
			AsyncTask<Intent, Void, String> a = new Main().new AsyncWeatherInformation().execute(i);
			return new WeatherInformation(a.get(2, TimeUnit.MINUTES), tempUnit, speedUnit, psrUnit);
		}
		else {
			throw new NoConnectionException();
		}
	}
}
















