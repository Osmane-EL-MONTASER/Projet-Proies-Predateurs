using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant le vent dans la 
/// simulation.
/// 
/// Fait par EL MONTASER Osmane le 14/03/2022.
/// </summary>
public class Wind : Weather {
    public Wind(GameObject skyAndFogGlobalVolume, float treeFallingChance = 0.0f, float waterWaveHeight = 0.0f,
                   float humidityPercentage = 0.0f) : base(skyAndFogGlobalVolume, treeFallingChance, waterWaveHeight, humidityPercentage) {
    }
}