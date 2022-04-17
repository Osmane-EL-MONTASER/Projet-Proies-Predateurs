using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe codée par Pierre AVERTY pour l'ajout d'un agent dans la simulation. 
///
/// Fait par AVERTY Pierre le 28/03/2022 et modifiée le 03/04/2022.
/// </summary>
public class AddSimulationAgent : MonoBehaviour {
    // Start is called before the first frame update

    /// <summary>
    /// Référence à la caméra afin de changer la vue basique
    /// en la vue d'ajout d'agent.
    /// </summary>
    public GameObject Camera;
    /// <summary>
    /// Panel Agent Simulation.
    /// </summary>
    public GameObject AgentSimulationPanel;

    /// <summary>
    /// GameObject Parent.
    /// </summary>
    public GameObject Parent;

    /// <summary>
    /// Champs pour les agents.
    /// </summary>
    public TMP_InputField agentField;

    /// <summary>
    /// Méthode qui s'active à l'initialisation du script.
    /// 
    /// Fait par AVERTY Pierre le 28/03/2022.
    /// </summary>
    void Start() {
        Camera = ConfigCamera.Instance.CurrentCamera;
    }
    
    /// <summary>
    /// Méthode qui s'active la fin de l'ajout d'agents.
    /// 
    /// Fait par AVERTY Pierre le 28/03/2022, modifiée le 03/04/2022 et le 10/04/2022.
    /// </summary>
    public void onEndEdit(){
        AgentSimulationPanel.SetActive(true);
        Parent.SetActive(false);
        
        var MainCamera = ConfigCamera.Instance.CurrentCamera;

        if(agentField.text == "1"){
            agentField.text = "";

            Camera.GetComponent<AgentCamera>().EnterAgentLook();
            AgentManager.Instance.newGhostInSim();
        }

        if(MainCamera.GetComponent<BasicCamera>().enabled != true) {
            MainCamera.GetComponent<BasicCamera>().enabled = true;
            GameObject.Find("Player").GetComponent<PlayerMovements>().enabled = true;
        } else {
            MainCamera.GetComponent<BasicCamera>().enabled = false;
            GameObject.Find("Player").GetComponent<PlayerMovements>().enabled = false;
        }
    }

}
