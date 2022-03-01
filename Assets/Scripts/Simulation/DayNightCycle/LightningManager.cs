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
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightningPresets preset;

    /// <summary>
    /// L'heure actuelle dans le monde de la simulation.
    /// </summary>
    [SerializeField, Range(0, 24)] private float timeOfDay;

    /// <summary>
    /// Pour faire avancer le temps dans le cycle jour /
    /// nuit.
    /// 
    /// Intégrée par EL MONTASER Osmane le 01/03/2022. 
    /// </summary>
    public void Update() {
        if(preset == null)
            return;

        if(Application.isPlaying) {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            updateLightning(timeOfDay / 24f);
        } else
            updateLightning(timeOfDay / 24f);
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
    private void updateLightning(float timePercent) {
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if(directionalLight != null) {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170f, 0f));
        }
    }

    /// <summary>
    /// Récupérer le premier DirectionalLightning sur lequel
    /// travailler avec le cycle jour / nuit.
    /// </summary>
    private void onValidate() {
        if(directionalLight != null)
            return;
        
        if(RenderSettings.sun != null)
            directionalLight = RenderSettings.sun;
        else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights) {
                if(light.type == LightType.Directional) {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}
