using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using System;

/// <summary>
/// Classe qui permet de mettre à jour le GUI des
/// statistiques d'un agent.
/// 
/// Fait par EL MONTASER Osmane le 06/05/2022.
/// </summary>
public class AgentStatsGUIUpdater : MonoBehaviour {

    /// <summary>
    /// Référence au GUI à mettre à jour.
    /// </summary>
    public GameObject AgentStatsGUI;

    public Agent AgentToTrack;

    /// <summary>
    /// Fonction qui met à jour le GUI des statistiques
    /// en fonction des données de l'agent sélectionné.
    /// </summary>
    void Update() {
        //Si un agent est sélectionné, que le GUI existe et qu'il est actif.
        if(AgentToTrack != null && AgentStatsGUI != null && AgentStatsGUI.active) {
            //Mise à jour du GUI
            GameObject.Find("SpeciesName").GetComponent<TMPro.TextMeshProUGUI>().text 
            = "Espèce : " + AgentToTrack.Attributes["SpeciesName"];

            GameObject.Find("Age").GetComponent<TMPro.TextMeshProUGUI>().text 
            = "Age : " + Math.Round(Convert.ToDouble(AgentToTrack.Attributes["Age"]), 3);

            GameObject.Find("Sex").GetComponent<TMPro.TextMeshProUGUI>().text 
            = "Sexe : " + AgentToTrack.Attributes["Gender"];

            GameObject.Find("Icon").GetComponent<RawImage>().texture = PrefabUtility.GetIconForGameObject(AgentToTrack.gameObject);

            GameObject.Find("HPText").GetComponent<TMPro.TextMeshProUGUI>().text 
            = Math.Round(Convert.ToDouble(AgentToTrack.Attributes["Health"]), 3) + " / " 
                + Math.Round(Convert.ToDouble(AgentToTrack.Attributes["MaxHealth"]), 3);
            GameObject.Find("HealthBar").GetComponent<Slider>().value 
            = float.Parse(AgentToTrack.Attributes["Health"]) / float.Parse(AgentToTrack.Attributes["MaxHealth"]);

            GameObject.Find("StaminaText").GetComponent<TMPro.TextMeshProUGUI>().text 
            = Math.Round(Convert.ToDouble(AgentToTrack.Attributes["Stamina"]), 3) + " / " + "1";
            GameObject.Find("StaminaBar").GetComponent<Slider>().value 
            = float.Parse(AgentToTrack.Attributes["Stamina"]);

            GameObject.Find("ActionText").GetComponent<TMPro.TextMeshProUGUI>().text 
            = "Action : " + AgentToTrack.GetCurrentAction();
            
            GameObject.Find("HungerText").GetComponent<TMPro.TextMeshProUGUI>().text 
            = Math.Round(Convert.ToDouble(AgentToTrack.Attributes["EnergyNeeds"]), 3) + " / " + "1";
            GameObject.Find("HungerBar").GetComponent<Slider>().value 
            = float.Parse(AgentToTrack.Attributes["EnergyNeeds"]);

            GameObject.Find("ThirstinessText").GetComponent<TMPro.TextMeshProUGUI>().text 
            = Math.Round(Convert.ToDouble(AgentToTrack.Attributes["WaterNeeds"]), 3) + " / " + "1";
            GameObject.Find("ThirstinessBar").GetComponent<Slider>().value 
            = float.Parse(AgentToTrack.Attributes["WaterNeeds"]);


        }
    }
}