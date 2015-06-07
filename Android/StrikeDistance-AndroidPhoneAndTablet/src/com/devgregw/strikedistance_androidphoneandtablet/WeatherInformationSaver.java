package com.devgregw.strikedistance_androidphoneandtablet;

import sdengine.WeatherInformation;

public class WeatherInformationSaver {
	static WeatherInformation w;

	public static WeatherInformation getWeatherInformation() {
		return w;
	}

	public static void setWeatherInformation(WeatherInformation weather) {
		w = weather;
	}
}
