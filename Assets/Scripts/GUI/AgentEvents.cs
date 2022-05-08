using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

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

    public Sprite[] spriteList;

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
        string tempPath = "Data Source=tempDB.db;Version=3";
        DBHelper _dbHelper = new(tempPath);
        Dictionary<string,string> datas = _dbHelper.SelectSpeciesInfo();

        switch (this.name){
            case UINames.OPEN_PROIE_BUTTON:

                foreach(KeyValuePair<string,string> entry in datas){
                    if(entry.Value.Equals("prey")){
                        processTemplates(preysPanel.transform.Find("ContentProies/ScrollView/ViewPort/Content"),"ContentProies/ScrollView/ViewPort/Content/Template", entry.Key, preysPanel);
                    }
                }
                if(!preysPanel.active ^ preysPanel.GetComponent<CanvasGroup>().alpha == 0){
                    preysPanel.GetComponent<CanvasGroup>().alpha = 1;
                }
                else
                    preysPanel.GetComponent<CanvasGroup>().alpha = 0;

                preysPanel.SetActive(true);
                autotrophsPanel.SetActive(false);
                predatorsPanel.SetActive(false);
                break;
            case UINames.OPEN_PREDATEUR_BUTTON:
    
                foreach(KeyValuePair<string,string> entry in datas){
                    if(entry.Value.Equals("predator")){
                        processTemplates(predatorsPanel.transform.Find("ContentProies/Scroll View/ViewPort/Content"),"ContentProies/Scroll View/ViewPort/Content/Template", entry.Key, predatorsPanel);
                    }
                }
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

                foreach(KeyValuePair<string,string> entry in datas){
                    if(entry.Value.Equals("autotroph")){
                        processTemplates(autotrophsPanel.transform.Find("ContentProies/Scroll View/ViewPort/Content"),"ContentProies/Scroll View/ViewPort/Content/Template", entry.Key, autotrophsPanel);
                    }
                }
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
    private void processTemplates(Transform parent, string path, string name, GameObject panel){
        GameObject duplicate = panel.transform.Find(path).gameObject;
        Debug.Log(parent);
        duplicate = Instantiate(duplicate, parent);
        duplicate.name = name;
        Image image = duplicate.GetComponent<Image>();
        GameObject child = duplicate.transform.Find("Text (TMP)").gameObject;
        foreach(Sprite sprite in spriteList){
            if(sprite.name == name)
                image.sprite = sprite;
        }
        child.GetComponent<TMP_Text>().text = Regex.Replace(name, "[0-9]", "");
        child.GetComponent<TMP_Text>().enabled = true;
        foreach(Behaviour component in duplicate.GetComponents(typeof(Behaviour))){
            component.enabled = true;
        }
    }
}

