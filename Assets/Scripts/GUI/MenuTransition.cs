using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private GameObject MainCamera;

    /// <summary>
    /// Le menu d'ajout d'agents en simulation.
    /// </summary>
    public GameObject AgentSimulationPanel;
    /// <summary>
    /// Le menu d'ajout d'effets environnementaux en simulation.
    /// </summary>
    public GameObject MeteoSimulationPanel;
    public GameObject GraphPanel;

    /// <summary>
    /// Type d'un nouvel agent.
    /// </summary>
     public string newAgentType;

    /// <summary>
    /// Fonction qui cache et qui affiche un menu au clique.
    /// 
    /// Si la prochaine scène se trouve être la simulation,
    /// cela active les contrôles de la caméra.
    /// 
    /// Fait par Pierre AVERTY le 28/02/2022, modifiée le 03/04/2022, le 10/04/2022 et le 16/04/2022.
    /// Révisée par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    /// 
    /// A refactoriser (overwrite la classe avec un bool)
    /// <param name="newScene">Nouvelle scène qui sera affichée.</param>
    public void OnClick(GameObject newScene) {
        if(!parent)
            parent = gameObject.transform.parent.gameObject;

        parent.SetActive(false);
        newScene.SetActive(true);

        if(newScene.name.Contains(SceneNames.SIMULATION_SCENE)) {
            toogleCamera(true);
            GameObject.Find("AgentManager").GetComponent<AgentManager>().enabled = true;
            GameObject.Find("Player").GetComponent<DataUpdater>().enabled = true;
            GraphPanel.SetActive(true);
        } else
            toogleCamera(false);

        if(newScene.name == "Panel New Agent Panel Config"){
            AgentManager.Instance.newAgentType = newAgentType;

            GameObject.Find("Canvas/Panel New Agent Panel Config/Image").GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
            GameObject.Find("Canvas/Panel New Agent Panel Config/Image/Text").GetComponent<TextMeshProUGUI>().text = gameObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
        }

    }

    /// <summary>
    /// Fonction qui active / désactive les contrôles de la caméra.
    /// Utile lorsque l'on passe des menus à la simulation.
    /// 
    /// Fait par EL MONTASER Osmane le 01/03/2022.
    /// Révisée par AVERTY Pierre le 14/03/2022, le 25/03/2022 et le 28/03/2022.
    /// </summary>
    /// 
    /// <param name="newValue">Nouvelle valeur qui influera sur
    /// l'activation ou non du script de contrôle de la caméra.</param>
    public void toogleCamera(bool newValue) {
        MainCamera = ConfigCamera.Instance.CurrentCamera;
        
        if(MainCamera.GetComponent<BasicCamera>().enabled != newValue) {
            MainCamera.GetComponent<BasicCamera>().enabled = newValue;
            GameObject.Find("Player").GetComponent<PlayerMovements>().enabled = false;

            if(AgentSimulationPanel && MeteoSimulationPanel){
                AgentSimulationPanel.SetActive(true);
                MeteoSimulationPanel.SetActive(true);

                GameObject.Find("Player").GetComponent<PlayerMovements>().enabled = true;
            }
            
        }
    }
}
