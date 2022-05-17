using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Classe reprise par HAMICHE Bilal le 29/04 pour le prototype. 
/// Elle est utilisée pour le menu contenant le graphe.
/// Cette classe est reprise depuis ce lien https://assetstore.unity.com/packages/tools/gui/dynamic-line-chart-108651
/// </summary>
public class MenuGraph : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    [MenuItem("GameObject/UI/DataDiagram")]
    public static void AddDataDiagramInGameObject()
    {

        GameObject parent = null;
        if (null != Selection.activeTransform)
        {
            parent = Selection.activeTransform.gameObject;
        }
        else
        {
            parent = null;
        }

        if ((null == parent) || (null == parent.GetComponentInParent<Canvas>()))
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (null == canvas)
            {
                Debug.LogError("AddDataDiagram : Impossible de trouver un canvas dans la scene!");
                return;
            }
            else
            {
                parent = FindObjectOfType<Canvas>().gameObject;
            }
        }

        GameObject prefab = Resources.Load("Prefabs/DataDiagram") as GameObject;
        if (null == prefab)
        {
            Debug.LogError("AddDataDiagram : Erreur de chargement de DataDiagram!");
            return;
        }

        GameObject dataDiagram;
        if (null != parent)
            dataDiagram = Instantiate(prefab, parent.transform);
        else
            dataDiagram = Instantiate(prefab);

        if (null == dataDiagram)
        {
            Debug.LogError("AddDataDiagram : Erreur lors de l'instanciation de DataDiagram!");
            return;
        }

        Undo.RegisterCreatedObjectUndo(dataDiagram, "Created dataDiagram");
        dataDiagram.name = "DataDiagram";
    }
}