using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui modélise les "fantomes"
///
/// Fait par AVERTY Pierre le 10/04/2022
/// </summary> 
public class Ghost : MonoBehaviour {
private GameObject _camera;
private Vector3 _worldPosition;


/// <summary>
/// Méthode qui s'active à l'initialisation du script.
/// 
/// Fait par AVERTY Pierre le 17/04/2022.
/// </summary>
    void Start() {
        _camera = ConfigCamera.Instance.CurrentCamera;
    }

/// <summary>
/// Méthode qui s'active au changement d'un fantome.
///
/// Fait par AVERTY Pierre le 10/04/2022 et modifiée le 17/04/2022
/// </summary> 
    void Update() {
        Plane plane = new Plane(Vector3.up, -100f);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out distance)) {
                _worldPosition = ray.GetPoint(distance);
        }

        transform.position = _worldPosition;
        transform.localScale = new Vector3(1f,1f,1f);

        if(Input.GetMouseButtonDown(0)){
            AgentManager.Instance.newAgentInSim(_worldPosition);
            _camera.GetComponent<AgentCamera>().ExitAgentLook();
            Destroy(gameObject);
        }
    }

}
