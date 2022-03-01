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
    /// Constructeur de la classe.
    /// 
    /// Fait par Pierre AVERTY le 28/02/2022
    /// </summary>
    public MenuTransition() {
        
    }

    /// <summary>
    /// Fonction qui cache et qui affiche un menu au clique.
    /// 
    /// Fait par Pierre AVERTY le 28/02/2022
    /// <param name="newScene">Nouvelle scène qui sera affichée</param>
    /// </summary>
    public void OnClick(GameObject newScene) {
        parent = gameObject.transform.parent.gameObject;

        parent.SetActive(false);
        newScene.SetActive(true);
    }

}
