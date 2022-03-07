using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe qui contient les fonctions à exécuter lors des
/// interactions avec l'interface des proies.
/// 
/// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
/// </summary>
public class ProieEvents : MonoBehaviour {

    /// <summary>
    /// Le panel qui s'affiche lors du clique sur le bouton
    /// proie.
    /// </summary>
    public GameObject proiePanel;

    /// <summary>
    /// Permet au code de désactiver la scrollView lorsque le
    /// panel de proie est caché.
    /// </summary>
    public GameObject proieScrollView;

    /// <summary>
    /// Exécutée au début afin de cacher par défaut tout le
    /// panel de proie.
    /// 
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    void Start() {
        if(this.name == UINames.OPEN_PROIE_BUTTON)
            toogleProiePanel();
    }

    /// <summary>
    /// Executée lors d'un clic sur un des boutons de 
    /// l'interface.
    ///
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    public void onClick() {
        Debug.Log("Click");
        if(this.name == UINames.OPEN_PROIE_BUTTON)
            toogleProiePanel();
    }

    /// <summary>
    /// Fonction permettant d'afficher ou de cacher le panel
    /// proie avec toutes les proies que l'on peut ajouter.
    /// 
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    private void toogleProiePanel() {
        var proiePanelGroup = proiePanel.GetComponent<CanvasGroup>();
        if(proiePanelGroup.alpha == 0) {
            proiePanelGroup.alpha = 1;
            proiePanelGroup.interactable = true;
            proieScrollView.GetComponent<ScrollRect>().enabled = true;
        } else {
            proiePanelGroup.alpha = 0;
            proiePanelGroup.interactable = false;
            proieScrollView.GetComponent<ScrollRect>().enabled = false;
        }
        
    }
}
