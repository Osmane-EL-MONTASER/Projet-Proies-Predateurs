using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant un effet météorologique
/// tel que :
/// Pluie, Tempête, Vent, Orage, Sécheresse.
/// 
/// Chaque effet météorologique aura ses propres
/// valeurs pour chacun des effets prévus dans
/// la classe mère Weather.
/// 
/// Fait par EL MONTASER Osmane le 11/03/2022.
/// </summary>
public class Weather : MonoBehaviour {
    /// <summary>
    /// La probabilité qu'un arbre tombe.
    /// </summary>
    public float TreeFallingChance;

    /// <summary>
    /// La hauteur des vagues de l'eau des lacs /
    /// océans pour simuler le vent sur l'eau par
    /// exemple.
    /// </summary>
    public float WaterWaveHeight;

    /// <summary>
    /// Taux d'humidité de la zone.
    /// </summary>
    public float HumidityPercentage;

    //A COMPLETER POUR AJOUTER D'AUTRES EFFETS SUR
    //L'ENVIRONNEMENT TEL QUE LA TEMPERATURE...

    /// <summary>
    /// 
    /// </summary>
    /// <param name="treeFallingChance"></param>
    /// <param name="waterWaveHeight"></param>
    /// <param name="humidityPercentage"></param>
    public Weather(float treeFallingChance = 0.0f, float waterWaveHeight = 0.0f,
                   float humidityPercentage = 0.0f) {
        
    }
}
