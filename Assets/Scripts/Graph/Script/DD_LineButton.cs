using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe reprise et modifiée par HAMICHE Bilal le 05/05, elle permet de gérer les
/// évenements liés aux boutons pour les courbes et la fenetre
/// </summary>
public class DD_LineButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private GameObject m_Line;
    public GameObject m_Label;

    public GameObject line {
        get { return m_Line; }
        set {
            DD_Lines lines = value.GetComponent<DD_Lines>();
            if (null == lines) {
                Debug.LogWarning(this.ToString() + "LineButton error : set line null == value.GetComponent<Lines>()");
                return;
            } else {
                m_Line = value;
                SetLineButton(lines);
            }
        }
    }
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// Méthode permettant d'ajouter les labels en fonction des noms de lignes
    /// </summary>
    /// <param name="lines"> Variable de type DD_Lines qui représente les courbes sur le graphe</param>
    private void SetLabel(DD_Lines lines) {

        if ((null == m_Label) || (null == m_Label.GetComponent<Text>())) {
            m_Label = null;
        }

        try {
            m_Label.GetComponent<Text>().text = lines.GetComponent<DD_Lines>().lineName;
            m_Label.GetComponent<Text>().color = lines.GetComponent<DD_Lines>().color;
        } catch {
            m_Label.GetComponent<Text>().color = Color.white;
        }
    }

    /// <summary>
    /// Méthode permettant d'ajouter les boutons des lignes en fonction des noms de lignes
    /// </summary>
    /// <param name="lines"> Les courbes sur le graphes</param>
    public void SetLineButton(DD_Lines lines) {

        name = string.Format("Button{0}", lines.gameObject.name);
        GetComponent<Image>().color = lines.color;

        SetLabel(lines);
    }



    /// <summary>
    /// Méthode permettant d'afficher du texte lorsque l'on passe la souris sur un bouton
    /// </summary>
    /// <param name="eventData"> Variable de type PointerEventData qui permet de
    /// détecter si le pointeur de la souris est sur le bouton </param>
    public void OnPointerEnter(PointerEventData eventData) {

        
        if (eventData.pointerCurrentRaycast.gameObject != gameObject)
            return;

        if (null == m_Label)
            return;

        DD_DataDiagram dd = GetComponentInParent<DD_DataDiagram>();
        if (null == dd) {
            return;
        }

        m_Label.transform.SetParent(dd.transform);
        m_Label.transform.position = transform.position + new Vector3(0, - GetComponent<RectTransform>().rect.height / 2, 0);
        m_Label.SetActive(true);
    }

    /// <summary>
    /// Méthode permettant de cacher les labels lorsque l'on enleve la souris du bouton
    /// </summary>
    /// <param name="eventData"> Variable de type PointerEventData qui permet de
    /// détecter si la souris est sur le bouton </param>
    public void OnPointerExit(PointerEventData eventData) {


        if (null == m_Label)
            return;

        m_Label.transform.SetParent(transform);
        m_Label.SetActive(false);
    }

    /// <summary>
    /// Methode permettant d'augmenter ou de réduire ou/et d'afficher ou de masquer les courbes 
    /// lors d'un clic sur les boutons correspondant
    /// </summary>
    public void OnButtonClick() {

        if (true == Input.GetKey(KeyCode.LeftControl)) {
            return;
        }

        if (null == m_Line) {
            Debug.LogWarning(this.ToString() + "error OnButtonClick : null == m_Line");
            return;
        }

        DD_Lines lines = m_Line.GetComponent<DD_Lines>();
        if(null == lines) {
            Debug.LogWarning(this.ToString() + "error OnButtonClick : null == DD_Lines");
            return;
        } else {
            lines.IsShow = !lines.IsShow;
        }
    }

    /// <summary>
    /// Méthode qui permet de supprimer un label si il existe plus
    /// </summary>
    public void DestroyLineButton() {

        if(null != m_Label)
            Destroy(m_Label);
    }
}
