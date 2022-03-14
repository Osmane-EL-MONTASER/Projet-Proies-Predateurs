using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe codée par Pierre AVERTY pour la transition entre les scènes. 
/// </summary>
public class MenuTransition : MonoBehaviour {
    /// <summary>
    /// Objet parent du boutton cliqué.
    /// </summary>
    public GameObject parent;
    
    /// <summary>
    /// La caméra qui affiche les interfaces ainsi que la simulation.
    /// </summary>
    public GameObject MainCamera;

    /// <summary>
    /// Le menu d'ajout d'agents en simulation.
    /// </summary>
     public GameObject AgentSimulationPanel;
    /// <summary>
    /// Le menu d'ajout d'effets environnementaux en simulation.
    /// </summary>
     public GameObject MeteoSimulationPanel;

    /// <summary>
    /// Script pour le menu d'ajout d'effets environnementaux en simulation.
    /// </summary>
     public AgentEvents MenuScript;
    
    /// <summary>
    /// Fonction qui cache et qui affiche un menu au clique.
    /// 
    /// Si la prochaine scène se trouve être la simulation,
    /// cela active les contrôles de la caméra.
    /// 
    /// Fait par Pierre AVERTY le 28/02/2022.
    /// Révisée par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    /// 
    /// <param name="newScene">Nouvelle scène qui sera affichée.</param>
    public void OnClick(GameObject newScene) {
        var parent = gameObject.transform.parent.gameObject;

        parent.SetActive(false);
        newScene.SetActive(true);

        if(newScene.name == SceneNames.SIMULATION_SCENE)
            toogleCamera(true);
        else
            toogleCamera(false);
    }

    /// <summary>
    /// Fonction qui active / désactive les contrôles de la caméra.
    /// Utile lorsque l'on passe des menus à la simulation.
    /// 
    /// Fait par EL MONTASER Osmane le 01/03/2022.
    /// Révisée par AVERTY Pierre le 14/03/2022.
    /// </summary>
    /// 
    /// <param name="newValue">Nouvelle valeur qui influera sur
    /// l'activation ou non du script de contrôle de la caméra.</param>
    public void toogleCamera(bool newValue) {
        if(MainCamera.GetComponent<MouseLook>().enabled != newValue){
            MainCamera.GetComponent<MouseLook>().enabled = newValue;

            AgentSimulationPanel.SetActive(true);
            MeteoSimulationPanel.SetActive(true);
            MenuScript.onClick();
        }
    }
}
