using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe qui contient les fonctions à exécuter lors des
/// interactions avec l'interface des statistiques des
/// agents.
/// 
/// Fait par EL MONTASER Osmane le 06/05/2022.
/// </summary>
public class AgentStatsGUI : MonoBehaviour {

    public void OnExitButtonClick() {
        GameObject.Find("AgentStatsPanel").SetActive(false);
        GameObject.Find("AgentManager").GetComponent<AgentStatsGUIUpdater>().AgentToTrack = null;
    }
}