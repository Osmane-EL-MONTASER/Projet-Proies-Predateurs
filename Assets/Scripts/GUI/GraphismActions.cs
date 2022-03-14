using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe qui gère tous les paramètres graphiques
/// 
/// Fait par AVERTY Pierre le 13/03/2022.
/// </summary>
public class GraphismActions : MonoBehaviour {
    /// <summary>
    /// Menu déroulant qui gère la résolution.
    /// </summary>
    public Dropdown dropdownResolution;
    /// <summary>
    /// Menu déroulant qui gère l'affichage de l'écran.
    /// </summary>
    public Dropdown dropdownFullScreen;

    /// <summary>
    /// Methode qui modifie la résolution de l'écran.
    ///
    /// Fait par AVERTY Pierre le 13/03/2022.
    /// </summary>
    public void resolutionHandler(){
        switch(dropdownResolution.value){
            case 0:
                Screen.SetResolution(1920,1080,true);
                break;
            case 1:
                Screen.SetResolution(1280,720,true);
                break;
            case 2:
                Screen.SetResolution(800,600,true);
                break;
        }
    }

    /// <summary>
    /// Methode qui modifie le mode d'affichage de l'écran.
    ///
    /// Fait par AVERTY Pierre le 13/03/2022.
    /// </summary>
    public void fullScreenHandler(){
        switch(dropdownFullScreen.value){
            case 0:
                Screen.fullScreen = true;
                break;
            case 1:
                Screen.fullScreen = false;
                break;
        }
    }
}
