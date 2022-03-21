using System;
using System.Collections.Generic;

/// <summary>
/// Classe qui gère la transition entre un temps
/// nuageux et un temps ensoileillé.
/// 
/// Fait par EL MONTASER Osmane le 20/03/2022.
/// </summary>
public abstract class WeatherTransition {
    /// <summary>
    /// Les paramètres dernièrement calculés par
    /// la classe.
    /// </summary>
    protected Dictionary<string, float> _cloudSettings;

    /// <summary>
    /// Initialise les paramètres de la transition
    /// à zéro.
    /// 
    /// Fait par EL MONTASER Osmane le 20/03/2022.
    /// </summary>
    public WeatherTransition() {
        _cloudSettings = new Dictionary<string, float>();
        _cloudSettings.Add("cloudDensity", .0f);
        _cloudSettings.Add("shapeFactor", .0f);
        _cloudSettings.Add("shapeScale", .0f);
        _cloudSettings.Add("erosionFactor", .0f);
    }

    /// <summary>
    /// Permet de récupérer les prochains paramètres
    /// pour les nuages à mettre.
    /// 
    /// Fait par EL MONTASER Osmane le 20/03/2022.
    /// </summary>
    /// <returns>
    /// Retourne les nouveaux paramètres à mettre dans
    /// les nuages de la scène si il y en a.
    /// Retourne null sinon.
    /// </returns>
    public abstract Dictionary<string, float> GetNextCloudSettings();
}