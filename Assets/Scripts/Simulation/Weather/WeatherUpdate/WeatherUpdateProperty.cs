public struct WeatherUpdatePropertyValued {
    public WeatherUpdateProperty Property;
    public double Modifier;

    public WeatherUpdatePropertyValued(WeatherUpdateProperty wUP, double val) { 
        Property = wUP; 
        Modifier = val;
    } 
}

public enum WeatherUpdateProperty {
    Fall,
    Thirstiness
}