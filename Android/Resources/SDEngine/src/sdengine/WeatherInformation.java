package sdengine;

import org.json.JSONException;
import org.json.JSONObject;

import sdengine.convertions.Converter;
import sdengine.convertions.enumerations.TemperatureUnit;
import android.net.Uri;
import android.util.Log;

public class WeatherInformation {
	private String jsonSource;
	
	public Double latitude;
	public Double longitude;
	public Double elevation;
	public String condtionString;
	public Double temperature;
	public Double feelsLike;
	public String humidity;
	public String windDirection;
	public Double windSpeed;
	public Double windGustSpeed;
	public String pressure;
	public Uri forecastUri;
	public Uri historicUri;
	
	public void update(int tempUnit, int speedUnit, int psrUnit) {
	    try {
	    	JSONObject data = new JSONObject(jsonSource);
			JSONObject obs = data.getJSONObject("current_observation");
			temperature = (tempUnit == 0) ? Converter.round(obs.getDouble("temp_f")) : (tempUnit == 1) ? Converter.round(obs.getDouble("temp_c")) : Converter.round(Converter.convert(TemperatureUnit.Celsius, TemperatureUnit.Kelvin, obs.getDouble("temp_c")));
			feelsLike = (tempUnit == 0) ? Converter.round(obs.getDouble("feelslike_f")) : (tempUnit == 1) ? Converter.round(obs.getDouble("feelslike_c")) : Converter.round(Converter.convert(TemperatureUnit.Celsius, TemperatureUnit.Kelvin, obs.getDouble("feelslike_c")));
			windSpeed = (speedUnit == 0) ? Converter.round(obs.getDouble("wind_mph")) : Converter.round(obs.getDouble("wind_kph"));
			windGustSpeed = (speedUnit == 0) ? Converter.round(obs.getDouble("wind_gust_mph")) : Converter.round(obs.getDouble("wind_gust_kph"));
			pressure = (psrUnit == 0) ? obs.getString("pressure_in") : obs.getString("pressure_mb");
	    } catch (JSONException e) {
	    	e.printStackTrace();
	    }
	}
	
	public WeatherInformation(String json, int tempUnit, int speedUnit, int psrUnit) {
		try {
			Log.d("sdengine", json);
			JSONObject data = new JSONObject(json);
			JSONObject obs = data.getJSONObject("current_observation");
			JSONObject loc = obs.getJSONObject("display_location");
			latitude = Converter.round(Double.parseDouble(loc.getString("latitude")));
			longitude = Converter.round(Double.parseDouble(loc.getString("longitude")));
			elevation = Converter.round(Double.parseDouble(loc.getString("elevation")));
			condtionString = (obs.getString("weather") == "") ? "Unavailable" : obs.getString("weather");
			temperature = (tempUnit == 0) ? Converter.round(obs.getDouble("temp_f")) : (tempUnit == 1) ? Converter.round(obs.getDouble("temp_c")) : Converter.round(Converter.convert(TemperatureUnit.Celsius, TemperatureUnit.Kelvin, obs.getDouble("temp_c")));
			feelsLike = (tempUnit == 0) ? Converter.round(obs.getDouble("feelslike_f")) : (tempUnit == 1) ? Converter.round(obs.getDouble("feelslike_c")) : Converter.round(Converter.convert(TemperatureUnit.Celsius, TemperatureUnit.Kelvin, obs.getDouble("feelslike_c")));
			humidity = obs.getString("relative_humidity");
			windSpeed = (speedUnit == 0) ? Converter.round(obs.getDouble("wind_mph")) : Converter.round(obs.getDouble("wind_kph"));
			windGustSpeed = (speedUnit == 0) ? Converter.round(obs.getDouble("wind_gust_mph")) : Converter.round(obs.getDouble("wind_gust_kph"));
			pressure = (psrUnit == 0) ? obs.getString("pressure_in") : obs.getString("pressure_mb");
			forecastUri = Uri.parse(obs.getString("forecast_url"));
			historicUri = Uri.parse(obs.getString("history_url"));
			jsonSource = json;
		}
		catch (JSONException e) {
			e.printStackTrace();
		}
	}
}
