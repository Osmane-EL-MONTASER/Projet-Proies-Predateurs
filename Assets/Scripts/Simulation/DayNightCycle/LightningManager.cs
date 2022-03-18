using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui permet de gérer le cycle jour
/// nuit récupérée dans la vidéo :
/// https://www.youtube.com/watch?v=m9hj9PdO328.
/// 
/// Intégrée par EL MONTASER Osmane le 01/03/2022.
/// </summary>
public class LightningManager : MonoBehaviour {
    [SerializeField] private Light _sunLight;
    [SerializeField] private Light _moonLight;
    [SerializeField] private LightningPresets _preset;

    /// <summary>
    /// L'heure actuelle dans le monde de la simulation.
    /// </summary>
    [SerializeField, Range(0, 240)] private float _timeOfDay;

    /// <summary>
    /// Pour faire avancer le temps dans le cycle jour /
    /// nuit.
    /// 
    /// Intégrée par EL MONTASER Osmane le 01/03/2022. 
    /// </summary>
    public void Update() {
        if(_preset == null)
            return;

        if(Application.isPlaying) {
            _timeOfDay += Time.deltaTime;
            _timeOfDay %= 240;
            UpdateLightning(_timeOfDay / 240f);
        } else
            UpdateLightning(_timeOfDay / 240f);
    }

    /// <summary>
    /// Fonction qui permet de convertir le temps actuel en
    /// un degré de rotation du soleil (le DirectionalLightning
    /// de la scène).
    /// 
    /// Intégrée par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    /// <param name="timePercent">Le pourcentage du temps actuel 
    /// par rapport à 24h.</param>
    private void UpdateLightning(float timePercent) {
        RenderSettings.ambientLight = _preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _preset.FogColor.Evaluate(timePercent);

        if(_sunLight != null) {
            _sunLight.color = _preset.DirectionalColor.Evaluate(timePercent);
            _sunLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170f, 0f));
            _moonLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) + 90f, -170f, 0f));
        }
    }

    /// <summary>
    /// Récupérer le premier _sunLightning sur lequel
    /// travailler avec le cycle jour / nuit.
    /// </summary>
    private void OnValidate() {
        if(_sunLight != null)
            return;
        
        if(RenderSettings.sun != null)
            _sunLight = RenderSettings.sun;
        else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights) {
                if(light.type == LightType.Directional) {
                    _sunLight = light;
                    return;
                }
            }
        }
    }
}
