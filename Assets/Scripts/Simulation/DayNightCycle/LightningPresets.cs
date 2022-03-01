using UnityEngine;

/// <summary>
/// Une des classes qui permet de gérer le cycle jour
/// nuit de la simulation.
/// 
/// Elle est directement reprise de la vidéo suivante :
/// https://www.youtube.com/watch?v=m9hj9PdO328.
/// 
/// Intégrée par EL MONTASER Osmane le 01/03/2022.
/// </summary>
[CreateAssetMenu(fileName = "LightningPresets", menuName = "Proies Prédateurs/LightningPresets", order = 0)]
public class LightningPresets : ScriptableObject {
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
