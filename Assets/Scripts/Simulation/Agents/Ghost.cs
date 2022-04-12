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
/// Fait par AVERTY Pierre le 10/04/2022
/// </summary> 
    void Update() {
       Vector3 pos = Input.mousePosition;

       transform.position = new Vector3(pos.x, pos.y, pos.z - 1f);
    }
}
