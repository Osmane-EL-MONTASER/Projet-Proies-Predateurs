using System;
using System.Collections.Generic;

/// <summary>
/// Classe qui gère la transition entre un temps
/// nuageux et un temps ensoileillé.
/// 
/// Fait par EL MONTASER Osmane le 20/03/2022.
/// </summary>
public class CloudyToSunnyTransition : WeatherTransition {
    /// <summary>
    /// Initialise les paramètres de la transition
    /// à zéro.
    /// 
    /// Fait par EL MONTASER Osmane le 20/03/2022.
    /// </summary>
    public CloudyToSunnyTransition() : base() {
        _cloudSettings["cloudDensity"] = 1.0f;
        _cloudSettings["shapeFactor"] = 0.65f;
        _cloudSettings["shapeScale"] = 4.1f;
        _cloudSettings["erosionFactor"] = .0f;
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
    public override Dictionary<string, float> GetNextCloudSettings() {
        processNextCloudSettings();
        return _cloudSettings;
    }

    private void processNextCloudSettings() {
        if(_cloudSettings["cloudDensity"] == 1.0f)
            _cloudSettings["cloudDensity"] = 0.6f;
        else
            _cloudSettings["cloudDensity"] -= 0.0005f;

        if(_cloudSettings["cloudDensity"] <= .0f)
            _cloudSettings = null;
    }
}