using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe qui contient les fonctions à exécuter lors des
/// interactions avec l'interface des autotrophes.
/// 
/// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
/// </summary>
public class AutotropheEvents : MonoBehaviour {

    /// <summary>
    /// Le panel qui s'affiche lors du clique sur le bouton
    /// autotrophe.
    /// </summary>
    public GameObject autotrophePanel;

    /// <summary>
    /// Permet au code de désactiver la scrollView lorsque le
    /// panel de autotrophe est caché.
    /// </summary>
    public GameObject autotropheScrollView;

    /// <summary>
    /// Exécutée au début afin de cacher par défaut tout le
    /// panel de autotrophe.
    /// 
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    void Start() {
        if(this.name == UINames.OPEN_AUTOTROPHE_BUTTON)
            toogleAutotrophePanel();
    }

    /// <summary>
    /// Executée lors d'un clic sur un des boutons de 
    /// l'interface.
    ///
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    public void onClick() {
        if(this.name == UINames.OPEN_AUTOTROPHE_BUTTON)
            toogleAutotrophePanel();
    }

    /// <summary>
    /// Fonction permettant d'afficher ou de cacher le panel
    /// autotrophe avec toutes les autotrophes que l'on peut ajouter.
    /// 
    /// Fait par AVERTY Pierre le 07/03/2022 inspirée par celle de EL MONTASER Osmane.
    /// </summary>
    private void toogleAutotrophePanel() {
        var autotrophePanelGroup = autotrophePanel.GetComponent<CanvasGroup>();
        if(autotrophePanelGroup.alpha == 0) {
            autotrophePanelGroup.alpha = 1;
            autotrophePanelGroup.interactable = true;
            autotropheScrollView.GetComponent<ScrollRect>().enabled = true;
        } else {
            autotrophePanelGroup.alpha = 0;
            autotrophePanelGroup.interactable = false;
            autotropheScrollView.GetComponent<ScrollRect>().enabled = false;
        }
        
    }
}
