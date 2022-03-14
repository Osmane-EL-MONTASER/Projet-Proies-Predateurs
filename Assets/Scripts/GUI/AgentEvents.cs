using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe qui contient les fonctions à exécuter lors des
/// interactions avec l'interface des predateurs.
/// 
/// Fait par AVERTY Pierre le 13/03/2022.
/// </summary>
public class AgentEvents : MonoBehaviour {

    /// <summary>
    /// Le panel qui contient les agents de type "prédateurs".
    /// </summary>
    public GameObject predatorsPanel;

    /// <summary>
    /// Le panel qui contient les agents de type "proies".
    /// </summary>
    public GameObject preysPanel;
    /// <summary>
    /// Le panel qui contient les animaux de type "autotrophes".
    /// </summary>
    public GameObject autotrophsPanel;

    /// <summary>
    /// Exécutée au début afin de cacher par défaut tous les panels
    ///
    /// Fait par AVERTY Pierre le 13/03/2022.
    /// </summary>
    public void Start() {
        preysPanel.SetActive(true);
        preysPanel.GetComponent<CanvasGroup>().alpha = 0;
        autotrophsPanel.SetActive(false);
        predatorsPanel.SetActive(false);
    }

    /// <summary>
    /// Executée lors d'un clic sur un des boutons de 
    /// l'interface.
    ///
    /// Fait par AVERTY Pierre le 13/03/2022.
    /// </summary>
    public void onClick() {
        tooglePannels();
    }

    /// <summary>
    /// Fonction permettant d'afficher ou de cacher les panels
    ///
    /// Fait par AVERTY Pierre le 13/03/2022 et modifiée le 14/03/2022.
    /// </summary>
    private void tooglePannels() {
        switch (this.name){
            case UINames.OPEN_PROIE_BUTTON:
                if(!preysPanel.active ^ preysPanel.GetComponent<CanvasGroup>().alpha == 0){
                    preysPanel.GetComponent<CanvasGroup>().alpha = 1;
                    preysPanel.SetActive(true);
                }
                else
                    preysPanel.GetComponent<CanvasGroup>().alpha = 0;

                autotrophsPanel.SetActive(false);
                predatorsPanel.SetActive(false);
                break;
            case UINames.OPEN_PREDATEUR_BUTTON:
                if(!predatorsPanel.active ^ predatorsPanel.GetComponent<CanvasGroup>().alpha == 0){
                    predatorsPanel.GetComponent<CanvasGroup>().alpha = 1;
                    predatorsPanel.SetActive(true);
                }
                else
                    predatorsPanel.GetComponent<CanvasGroup>().alpha = 0;

                preysPanel.SetActive(false);
                autotrophsPanel.SetActive(false);
                predatorsPanel.SetActive(true);
                break;
            case UINames.OPEN_AUTOTROPHE_BUTTON:
                if(!autotrophsPanel.active  ^ autotrophsPanel.GetComponent<CanvasGroup>().alpha == 0){
                    autotrophsPanel.GetComponent<CanvasGroup>().alpha = 1;
                    autotrophsPanel.SetActive(true);
                }
                else
                    autotrophsPanel.GetComponent<CanvasGroup>().alpha = 0;

                preysPanel.SetActive(false);
                autotrophsPanel.SetActive(true);
                predatorsPanel.SetActive(false);
                break;
            default:
                preysPanel.SetActive(false);
                autotrophsPanel.SetActive(false);
                predatorsPanel.SetActive(false);
                break;
        }
    }
}
