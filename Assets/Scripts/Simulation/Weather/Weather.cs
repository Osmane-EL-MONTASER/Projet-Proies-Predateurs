using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

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
public abstract class Weather {
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

    /// <summary>
    /// Permet de calculer une valeur aléatoire suivant une loi
    /// gaussienne centrée réduite.
    /// 
    /// Fait par Oneiros90 sur https://answers.unity.com/questions
    /// /421968/normal-distribution-random.html.
    /// </summary>
    /// <param name="minValue">
    /// La valeur minimale (celle de gauche).
    /// </param>
    /// <param name="maxValue">
    /// La valeur maximale (celle de droite).
    /// </param>
    /// <returns></returns>
    public static double RandomGaussian(double minValue = 0.0, double maxValue = 1.0) {
        double u, v, S;
        System.Random rand = new System.Random();

        do {
            u = 2.0 * rand.NextDouble() - 1.0;
            v = 2.0 * rand.NextDouble() - 1.0;
            S = u * u + v * v;
        } while (S >= 1.0);

        // Standard Normal Distribution
        double std = u * Math.Sqrt(-2.0 * Math.Log(S) / S);

        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        double mean = (minValue + maxValue) / 2.0;
        double sigma = (maxValue - mean) / 3.0;
        return Math.Clamp(std * sigma + mean, minValue, maxValue);
    }

    abstract public void Update();
}
