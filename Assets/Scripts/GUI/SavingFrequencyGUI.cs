//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

///// <summary>
///// Permet d'intéragir avec le InputField de
///// la fréquence de sauvegarde des données de la
///// simulation dans la base de données temporaire.
///// 
///// Fait par EL MONTASER Osmane le 01/04/2022.
///// </summary>
//public class SavingFrequencyGUI : MonoBehaviour {

//    /// <summary>
//    /// Une référence à l'input de la fréquence de
//    /// sauvegarde afin de supprimer les entrées
//    /// incorrects.
//    /// </summary>
//    public GameObject SaveFrequencyInput;

//    /// <summary>
//    /// La fréquence de sauvegarde utilisée lors de
//    /// la simulation.
//    /// </summary>
//    public float SaveFrequency { get; private set; }

//    /// <summary>
//    /// Fonction qui permet d'assurer que la valeur
//    /// entrée dans l'input du GUI de sélection du
//    /// monde pour la fréquence de sauvegarde est
//    /// bien un réel strictement positif.
//    /// 
//    /// Fait par EL MONTASER Osmane le 01/04/2022.
//    /// </summary>
//    public void OnEndEdit(string value) {
//        float saveFrequency = float.Parse(value);

//        if(saveFrequency <= 0)
//            SaveFrequencyInput.GetComponent<TMP_InputField>().text = "";
//        else
//            GameObject.Find("Player").GetComponent<TemporaryDataSaving>().SetSaveFrequency(saveFrequency);
//    }
//}
