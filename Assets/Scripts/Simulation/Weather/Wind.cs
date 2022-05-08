using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Linq;

/// <summary>
/// /// Classe représentant le vent dans la 
/// simulation.
/// 
/// Fait par EL MONTASER Osmane le 14/03/2022.
/// </summary>
public class Wind : Weather {

    /// <summary>
    /// Liste des objets à mettre à jour dans la
    /// zone.
    /// </summary>
    private List<TreeInstance> _objectsToUpdate;

    private float _intensity;

    public Wind(List<TreeInstance> objects, float intensity, float treeFallingChance = 0.0f, float waterWaveHeight = 0.0f,
                   float humidityPercentage = 0.0f) : base(treeFallingChance, waterWaveHeight, humidityPercentage) {
        _intensity = intensity;
        _objectsToUpdate = objects;
    }

    /// <summary>
    /// Permet de modifier 
    /// </summary>
    public override void Update() {
        int i = 0;
        if(WeatherObjectUpdater.ObjectsToUpdateMutex.WaitOne()) {
            foreach (var obj in _objectsToUpdate) {
                double randomGenerated = RandomGaussian();
                if(randomGenerated + _intensity - 0.75f >= 0.05) {
                    Dictionary<WeatherUpdatePropertyValued, List<TreeInstance> > dic =
                     new Dictionary<WeatherUpdatePropertyValued, List<TreeInstance>>();
                    dic.Add(new WeatherUpdatePropertyValued(WeatherUpdateProperty.Fall, i), _objectsToUpdate);
                    WeatherObjectUpdater.ObjectsToUpdate.Add(dic);
                }
                i++;
            }
        }
    }

    public float GetIntensity() {
        return _intensity;
    }
}