using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe qui contient les fonctions à exécuter lors des
/// interactions avec l'interface des predateurs.
/// 
/// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
/// </summary>
public class PredateurEvents : MonoBehaviour {

    /// <summary>
    /// Le panel qui s'affiche lors du clique sur le bouton
    /// predateur.
    /// </summary>
    public GameObject predateurPanel;

    /// <summary>
    /// Permet au code de désactiver la scrollView lorsque le
    /// panel de predateur est caché.
    /// </summary>
    public GameObject predateurScrollView;

    /// <summary>
    /// Exécutée au début afin de cacher par défaut tout le
    /// panel de predateur.
    /// 
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    void Start() {
        if(this.name == UINames.OPEN_PREDATEUR_BUTTON)
            tooglePredateurPanel();
    }

    /// <summary>
    /// Executée lors d'un clic sur un des boutons de 
    /// l'interface.
    ///
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    public void onClick() {
        if(this.name == UINames.OPEN_PREDATEUR_BUTTON)
            tooglePredateurPanel();
    }

    /// <summary>
    /// Fonction permettant d'afficher ou de cacher le panel
    /// predateur avec toutes les predateurs que l'on peut ajouter.
    /// 
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    private void tooglePredateurPanel() {
        var predateurPanelGroup = predateurPanel.GetComponent<CanvasGroup>();
        if(predateurPanelGroup.alpha == 0) {
            predateurPanelGroup.alpha = 1;
            predateurPanelGroup.interactable = true;
            predateurScrollView.GetComponent<ScrollRect>().enabled = true;
        } else {
            predateurPanelGroup.alpha = 0;
            predateurPanelGroup.interactable = false;
            predateurScrollView.GetComponent<ScrollRect>().enabled = false;
        }
        
    }
}
