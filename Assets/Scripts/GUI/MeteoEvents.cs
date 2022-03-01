using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe qui contient les fonctions à exécuter lors des
/// interactions avec l'interface météorologique.
/// 
/// Fait par EL MONTASER Osmane le 01/03/2022.
/// </summary>
public class MeteoEvents : MonoBehaviour {

    /// <summary>
    /// Le panel qui s'affiche lors du clique sur le bouton
    /// météo.
    /// </summary>
    public GameObject meteoPanel;

    /// <summary>
    /// Permet au code de désactiver la scrollView lorsque le
    /// panel de météo est caché.
    /// </summary>
    public GameObject meteoScrollView;

    /// <summary>
    /// Exécutée au début afin de cacher par défaut tout le
    /// panel de météo.
    /// 
    /// Fait par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    void Start() {
        if(this.name == UINames.OPEN_METEO_BUTTON)
            toogleMeteoPanel();
    }

    /// <summary>
    /// Executée lors d'un clic sur un des boutons de 
    /// l'interface.
    ///
    /// Fait par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    public void onClick() {
        if(this.name == UINames.OPEN_METEO_BUTTON)
            toogleMeteoPanel();
    }

    /// <summary>
    /// Fonction permettant d'afficher ou de cacher le panel
    /// météo avec toutes les météos que l'on peut ajouter.
    /// 
    /// Fait par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    private void toogleMeteoPanel() {
        var meteoPanelGroup = meteoPanel.GetComponent<CanvasGroup>();
        if(meteoPanelGroup.alpha == 0) {
            meteoPanelGroup.alpha = 1;
            meteoPanelGroup.interactable = true;
            meteoScrollView.GetComponent<ScrollRect>().enabled = true;
        } else {
            meteoPanelGroup.alpha = 0;
            meteoPanelGroup.interactable = false;
            meteoScrollView.GetComponent<ScrollRect>().enabled = false;
        }
        
    }
}
