package sdengine;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.text.MessageFormat;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

import sdengine.convertions.Converter;
import sdengine.convertions.enumerations.DistanceUnit;
import sdengine.convertions.enumerations.TemperatureUnit;
import sdengine.exceptions.NoConnectionException;
import android.app.ProgressDialog;
import android.content.Context;
import android.net.ConnectivityManager;
import android.os.AsyncTask;

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
	
	class AsyncWeatherInformation extends AsyncTask<Object, Void, String> {
		
		ProgressDialog dlg;
		Boolean show;
		
		protected String doInBackground(Object... params) {
			Context context = (Context)params[3];
			show = (Boolean)params[4];
			if (show) dlg = ProgressDialog.show(context, "Getting weather data...", "This shouldn't take long.", true, false);
			StringBuilder sb = new StringBuilder();
			Double lat = (Double)params[0];
			Double lon = (Double)params[1];
			String apiKey = (String)params[2];
			try {
				URL u = new URL(String.format("http://api.wunderground.com/api/%s/conditions/q/%s,%s.json", apiKey, String.valueOf(Converter.round(lat)), String.valueOf(Converter.round(lon))));
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
			finally {
				if (show && dlg != null) dlg.dismiss();
			}
		}
		
		@Override
		protected void onPreExecute() {
			super.onPreExecute();
		}

		@Override
		protected void onPostExecute(String result) {
			super.onPostExecute(result);
		}
	}
	
	private static Double getSpeedOfSound(Double c) {
		return Converter.round(331.5 + 0.6 * c);
	}
	
	private static Double getDistance(Double speedOfSound, Double time) {
		return Converter.round(speedOfSound * time);
	}
	
	private static String format(String format, Object... args) {
		return (new MessageFormat(format)).format(args);
	}
	
	public static String calculate(Double temperature, Double time, int tempUnit, int distUnit, boolean verboseMode, boolean[] verboseModeData) {
		endDistConvertion c = endDistConvertion.None;
        endTempConvertion t = endTempConvertion.None;
        double temp, res;
        int tempunit, distunit;
        String finaldist = "";
        tempunit = tempUnit;
        distunit = distUnit;
        switch (tempunit) {
            case 0:
                temp = Converter.convert(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, temperature);
                t = endTempConvertion.FahrenheittoCelsius;
                break;
            case 2:
                temp = Converter.convert(TemperatureUnit.Kelvin, TemperatureUnit.Celsius, temperature);
                t = endTempConvertion.KelvintoCelsius;
                break;
            default:
                temp = temperature;
                t = endTempConvertion.None;
                break;
        }
        res = getDistance(getSpeedOfSound(temp), time);
        double originaDist = res;
        switch (distunit) {
            case 0:
                res = Converter.convert(DistanceUnit.Meters, DistanceUnit.Miles, res);
                if (res < 1) {
                    res = Converter.convert(DistanceUnit.Miles, DistanceUnit.Feet, res);
                    if (res == 1)
                        finaldist = "foot";
                    else
                        finaldist = "feet";
                    c = endDistConvertion.MeterstoMilestoFeet;
                }
                else {
                    if (res == 1)
                        finaldist = "mile";
                    else
                        finaldist = "miles";
                    c = endDistConvertion.MeterstoMiles;
                }
                break;
            case 1:
                if (res >= 1000) {
                    res = Converter.convert(DistanceUnit.Meters, DistanceUnit.Kilometers, res);
                    if (res == 1)
                        finaldist = "kilometer";
                    else
                        finaldist = "kilometers";
                    c = endDistConvertion.MeterstoKilometers;
                }
                else {
                    if (res == 1)
                        finaldist = "meter";
                    else
                        finaldist = "meters";
                    c = endDistConvertion.None;
                }
                break;
        }
        res = Converter.round(res);
        String mainMsg, unitMsg, convMsg, calcMsg, tempConvEqu = "", distConvEqu = "", finalMsg;
        mainMsg = format("The lightning struck approximately {0} {1} away.", res, finaldist);
        unitMsg = format(
            "\n\nTemperature unit: {0}\nDistance unit: {1}",
            (tempunit == 0) ?
                "Fahrenheit" :
                (tempunit == 1) ?
                    "Celsius" :
                    "Kelvin",
            (distunit == 0) ?
                "Feet and Miles" :
                "Meters and Kilometers");
        switch (t) {
            case FahrenheittoCelsius:
                tempConvEqu = format(
                    "Convert Fahrenheit to Celsius: ({0} - 32) * 5 / 9 = {1}",
                    temperature,
                    temp);
                break;
            case KelvintoCelsius:
                tempConvEqu = format(
                    "Convert Kelvin to Celsius: {0} - 273.15 = {1}",
                    temperature,
                    temp);
                break;
            case None:
                tempConvEqu = "Temperature already in Celsius; no math done";
                break;
        }
        switch (c) {
            case MeterstoKilometers:
                distConvEqu = format(
                    "Convert Meters to Kilometers: {0} / 1000 = {1}",
                    originaDist,
                    res);
                break;
            case MeterstoMiles:
                distConvEqu = format(
                    "Convert Meters to Miles: {0} / 0.00062137 = {1}",
                    originaDist,
                    res);
                break;
            case MeterstoMilestoFeet:
                distConvEqu = format(
                    "Convert Meters to Miles then Feet: {0} / 0.00062137 = {1} / 5280 = {2}",
                    originaDist,
                    originaDist / 0.00062137,
                    res);
                break;
            case None:
                distConvEqu = "Distance set to meters and kilometers; no math done";
                break;
        }
        convMsg = format(
            "\n\n{0}\n{1}",
            tempConvEqu,
            distConvEqu);
        calcMsg = format(
            "\n\nSpeed of sound = 331.5 + 0.6 * {0} °C = {1} meters/second\nTime = {2} seconds\nDistance = Speed of sound * time\n{1} meters/second * {2} seconds = {3} meters",
            temp,
            getSpeedOfSound(temp),
            time,
            getDistance(getSpeedOfSound(temp), time));
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
	
	public static WeatherInformation getWeatherInformation(Context context, Boolean dialog, Double lat, Double lon, String apiKey, int tempUnit, int psrUnit, int speedUnit) throws NoConnectionException, TimeoutException, InterruptedException, ExecutionException {
		if (checkNetworkState(context)) {
			AsyncTask<Object, Void, String> a = new Main().new AsyncWeatherInformation().execute(lat, lon, apiKey, context, dialog);
			return new WeatherInformation(a.get(2, TimeUnit.MINUTES), tempUnit, speedUnit, psrUnit);
		}
		else {
			throw new NoConnectionException();
		}
	}
}