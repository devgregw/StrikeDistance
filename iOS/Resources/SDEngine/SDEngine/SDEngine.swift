//
//  SDEngine.swift
//  SDEngine
//
//  Created by Greg Whatley on 3/14/15.
//  Copyright (c) 2015 Greg Whatley. All rights reserved.
//

import Foundation
import CoreLocation

extension Double {
    func format(f: String) -> String {
        return NSString(format: "%\(f)f", self)
    }
}

public class Engine {
    init() {}
    
    public class Convertions {
        public class Temperature {
            init() {}
            
            func celsiusToFahrenheit(celsius: Double) -> Double {
                return celsius * 9 / 5 + 32
            }
            func celsiusToKelvin(celsius: Double) -> Double {
                return celsius + 273.15
            }
            
            func fahrenheitToCelsius(fahrenheit: Double) -> Double {
                return (fahrenheit - 32) * 5 / 9
            }
            func fahrenheitToKelvin(fahrenheit: Double) -> Double {
                return (fahrenheit + 459.67) * 5 / 9
            }
            
            func kelvinToCelsius(kelvin: Double) -> Double {
                return kelvin - 273.15
            }
            func kelvinToFahrenheit(kelvin: Double) -> Double {
                return kelvin * 9 / 5 - 459.67
            }
        }
        public class Distance {
            init() {}
            
            func metersToKilometers(meters: Double) -> Double {
                return meters / 1000
            }
            func metersToMiles(meters: Double) -> Double {
                return meters * 0.00062137
            }
            
            func milesToFeet(miles: Double) -> Double {
                return miles * 5280
            }
        }
    }
    
    public class Memory {
        enum UnitScope {
            case Temperature, Distance
        }
        
        init() {}
        
        func stringToInt(scope: UnitScope, input: String) -> Int {
            switch (scope) {
            case UnitScope.Temperature:
                switch (input.lowercaseString) {
                case "fahrenheit": return 0
                case "celsius": return 1
                case "kelvin": return 2
                default: return 0
                }
            case UnitScope.Distance:
                switch (input.lowercaseString) {
                case "feet/miles": return 0
                case "meters/kilometers": return 1
                default: return 0
                }
            }
        }
        func intToString(scope: UnitScope, input: Int) -> String {
            switch (scope) {
            case UnitScope.Temperature:
                switch (input) {
                case 0: return "Fahrenheit"
                case 1: return "Celsius"
                case 2: return "Fahrenheit"
                default: return "Fahrenheit"
                }
            case UnitScope.Distance:
                switch(input) {
                case 0: return "Feet/Miles"
                case 1: return "Meters/Kilometers"
                default: return "Feet/Miles"
                }
            }
        }
        
        func getTime() -> Double {
            return (NSUserDefaults.standardUserDefaults()).doubleForKey("time")
        }
        func setTime(value: Double) {
            var defaults = NSUserDefaults.standardUserDefaults()
            defaults.setDouble(value, forKey: "time")
            defaults.synchronize()
        }
        
        func getTemp() -> Double {
            return (NSUserDefaults.standardUserDefaults()).doubleForKey("temp")
        }
        func setTemp(value: Double) {
            var defaults = NSUserDefaults.standardUserDefaults()
            defaults.setDouble(value, forKey: "temp")
            defaults.synchronize()
        }
        
        func getTempUnit() -> Int {
            return (NSUserDefaults.standardUserDefaults()).integerForKey("tempUnit")
        }
        func setTempUnit(value: Int) {
            var defaults = NSUserDefaults.standardUserDefaults()
            defaults.setInteger(value, forKey: "tempUnit")
            defaults.synchronize()
        }
        
        func getDistUnit() -> Int {
            return (NSUserDefaults.standardUserDefaults()).integerForKey("distUnit")
        }
        func setDistUnit(value: Int) {
            var defaults = NSUserDefaults.standardUserDefaults()
            defaults.setInteger(value, forKey: "distUnit")
            defaults.synchronize()
        }
        
        func getAutoGet() -> Bool {
            return (NSUserDefaults.standardUserDefaults()).boolForKey("autoGet")
        }
        func setAutoGet(value: Bool) {
            var defaults = NSUserDefaults.standardUserDefaults()
            defaults.setBool(value, forKey: "autoGet")
            defaults.synchronize()
        }
        
        func getAutoConvert() -> Bool {
            return (NSUserDefaults.standardUserDefaults()).boolForKey("autoConvert")
        }
        func setAutoConvert(value: Bool) {
            var defaults = NSUserDefaults.standardUserDefaults()
            defaults.setBool(value, forKey: "autoConvert")
            defaults.synchronize()
        }
        
        func getVerboseMode() -> Bool {
            return (NSUserDefaults.standardUserDefaults()).boolForKey("verboseMode")
        }
        func setVerboseMode(value: Bool) {
            var defaults = NSUserDefaults.standardUserDefaults()
            defaults.setBool(value, forKey: "verboseMode")
            defaults.synchronize()
        }
        func getVerboseModeData() -> [Bool] {
            var data: [Bool] = []
            var defaults = NSUserDefaults.standardUserDefaults()
            data.append(defaults.boolForKey("vbUnitDetails"))
            data.append(defaults.boolForKey("vbConvertionMath"))
            data.append(defaults.boolForKey("vbCalculationMath"))
            return data
        }
        func setVerboseModeData(vbUnitDetails: Bool, vbConvertionMath: Bool, vbCalculationMath: Bool) {
            var defaults = NSUserDefaults.standardUserDefaults()
            defaults.setBool(vbUnitDetails, forKey: "vbUnitDetails")
            defaults.setBool(vbConvertionMath, forKey: "vbConvertionMath")
            defaults.setBool(vbCalculationMath, forKey: "vbCalculationMath")
            defaults.synchronize()
        }
    }
    
    func getSpeedOfSound(celsius: Double) -> Double {
        return 331.5 + 0.6 * celsius
    }
    func getDistance(speedOfSound: Double, time: Double) -> Double {
        return speedOfSound * time
    }
    func calculate(temperature: Double, time: Double) -> String {
        var newTemp: Double
        var result: Double
        var tempUnit: Int = (Memory()).getTempUnit()
        var distUnit: Int = (Memory()).getDistUnit()
        var distanceId: String = ""
        switch (tempUnit) {
        case 0: newTemp = (Convertions.Temperature()).fahrenheitToCelsius(temperature)
        case 2: newTemp = (Convertions.Temperature()).kelvinToCelsius(temperature)
        default: newTemp = temperature
        }
        result = self.getDistance(self.getSpeedOfSound(newTemp), time: time)
        switch (distUnit) {
        case 0:
            result = (Convertions.Distance()).metersToMiles(result)
            if (result < 1) {
                result = (Convertions.Distance()).milesToFeet(result)
                if (result == 1) {
                    distanceId = "foot"
                }
                else {
                    distanceId = "feet"
                }
            }
            else {
                if (result == 1) {
                    distanceId = "mile"
                }
                else {
                    distanceId = "miles"
                }
            }
        case 1:
            if (result >= 1000) {
                result = (Convertions.Distance()).metersToKilometers(result)
                if (result == 1) {
                    distanceId = "kilometer"
                }
                else {
                    distanceId = "kilometers"
                }
            }
            else {
                if (result == 1) {
                    distanceId = "meter"
                }
                else {
                    distanceId = "meters"
                }
            }
        default: (NSException(name: "Unexpected error", reason: "Variable distUnit was neither 0 or 1", userInfo: nil))
        }
        var newResult: String = result.format(".2")
        return "The lightning struck approximately \(newResult) \(distanceId) away."
    }
}


















