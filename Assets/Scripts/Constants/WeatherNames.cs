using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe contenant toutes les constantes correspondantes
/// au nom des météos du projet.
/// 
/// Fait par EL MONTASER Osmane le 13/03/2022.
/// </summary>
public static class WeatherNames {
    //Le nom de toutes les météos possibles.
    public const string WIND_WEATHER = "wind";
    public const string THUNDERSTORM_WEATHER = "thunderstorm";
    public const string STORM_WEATHER = "storm";
    public const string DROUGHT_WEATHER = "drought";
    public const string RAIN_WEATHER = "rain";

    public static Color WIND_COLOR = Color.red;
    public static Color THUNDERSTORM_COLOR = Color.green;
    public static Color STORM_COLOR = Color.blue;
    public static Color DROUGHT_COLOR = Color.yellow;
    public static Color RAIN_COLOR = new Color(0f, 255f, 255f);
}
