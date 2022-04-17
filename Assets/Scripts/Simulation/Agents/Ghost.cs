using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui modélise les "fantomes"
///
/// Fait par AVERTY Pierre le 10/04/2022
/// </summary> 
public class Ghost : MonoBehaviour {

/// <summary>
/// Méthode qui s'active au changement d'un fantome.
///
/// Fait par AVERTY Pierre le 10/04/2022 et modifiée le 17/04/2022
/// </summary> 
    void Update() {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Debug.Log(camera.transform.position.y);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        transform.localScale = new Vector3(0.03f * camera.transform.position.y,0.03f * camera.transform.position.y,0.03f * camera.transform.position.y);
    }

}
