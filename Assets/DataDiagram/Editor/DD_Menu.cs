using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
                Debug.LogError("AddDataDiagram : can not find a canvas in scene!");
                return;
            } else {
                parent = FindObjectOfType<Canvas>().gameObject;
            }
        }
        
        GameObject prefab = Resources.Load("Prefabs/DataDiagram") as GameObject;
        if (null == prefab) {
            Debug.LogError("AddDataDiagram : Load DataDiagram Error!");
            return;
        }

        GameObject dataDiagram;
        if (null != parent)
            dataDiagram = Instantiate(prefab, parent.transform);
        else
            dataDiagram = Instantiate(prefab);

        if(null == dataDiagram) {
            Debug.LogError("AddDataDiagram : Instantiate DataDiagram Error!");
            return;
        }

        Undo.RegisterCreatedObjectUndo(dataDiagram, "Created dataDiagram");
        dataDiagram.name = "DataDiagram"; 
    }
}
