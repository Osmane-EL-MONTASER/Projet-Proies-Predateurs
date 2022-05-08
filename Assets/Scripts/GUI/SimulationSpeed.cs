using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe qui permet de gérer la vitesse de
/// simulation changée dans le GUI en question.
/// 
/// Fait par EL MONTASER Osmane le 06/05/2022.
/// </summary>
public class SimulationSpeed : MonoBehaviour {
    public GameObject InputSimulationSpeed;

    public void OnValueChanged() {
        if(!InputSimulationSpeed.GetComponent<TMP_InputField>().text.Equals("")
            && InputSimulationSpeed.GetComponent<TMP_InputField>().text.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' }) != -1)
            ActionNames.TimeSpeed = ActionNames.DAY_DURATION * float.Parse(InputSimulationSpeed.GetComponent<TMP_InputField>().text.TrimStart('0'));
    }

    public void OnEditEnd() {
        if(InputSimulationSpeed.GetComponent<TMP_InputField>().text.Equals(""))
            InputSimulationSpeed.GetComponent<TMP_InputField>().text = "24";
        
        ActionNames.TimeSpeed = ActionNames.DAY_DURATION * float.Parse(InputSimulationSpeed.GetComponent<TMP_InputField>().text);
    }
}