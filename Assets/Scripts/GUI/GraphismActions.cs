using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphismActions : MonoBehaviour {
/// <summary>
/// Classe qui gère tous les paramètres graphiques
/// 
/// Fait par AVERTY Pierre le 13/03/2022.
/// </summary>

    /// <summary>
    /// 
    ///
    /// Fait par AVERTY Pierre le 13/03/2022.
    /// </summary>

    public Dropdown dropdownResolution;
    public Dropdown dropdownFullScreen;

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
