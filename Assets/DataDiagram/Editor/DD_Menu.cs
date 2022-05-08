using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Classe reprise par Bilal HAMICHE pour le prototype. 
/// Elle permet de créer le menu qui affiche le graphe
/// </summary>
public class DD_Menu : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [MenuItem("GameObject/UI/DataDiagram")]
    public static void AddDataDiagramInGameObject() {

        GameObject parent = null;
        if (null != Selection.activeTransform) {
            parent = Selection.activeTransform.gameObject;
        } else {
            parent = null;
        }

        if ((null == parent) || (null == parent.GetComponentInParent<Canvas>())) {
            Canvas canvas = FindObjectOfType<Canvas>();
            if(null == canvas) {
                Debug.LogError("AddDataDiagram : Impossible de trouver un canvas dans la scene!");
                return;
            } else {
                parent = FindObjectOfType<Canvas>().gameObject;
            }
        }
        
        GameObject prefab = Resources.Load("Prefabs/DataDiagram") as GameObject;
        if (null == prefab) {
            Debug.LogError("AddDataDiagram : Erreur de chargement de DataDiagram!");
            return;
        }
        /// <summary>
        /// Variable de type GameObject 
        /// 
        /// </summary>

        GameObject dataDiagram;
        if (null != parent)
            dataDiagram = Instantiate(prefab, parent.transform);
        else
            dataDiagram = Instantiate(prefab);

        if(null == dataDiagram) {
            Debug.LogError("AddDataDiagram : Erreur lors de l'instanciation de DataDiagram");
            return;
        }

        Undo.RegisterCreatedObjectUndo(dataDiagram, "Created dataDiagram");
        dataDiagram.name = "DataDiagram"; 
    }
}
