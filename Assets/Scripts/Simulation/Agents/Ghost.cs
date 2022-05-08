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

public Dictionary<string, string> Attributes;

public int n;


/// <summary>
/// Méthode qui s'active à l'initialisation du script.
/// 
/// Fait par AVERTY Pierre le 17/04/2022.
/// </summary>
    void Start() {
        _camera = ConfigCamera.Instance.CurrentCamera;
        Debug.Log(n);
    }

/// <summary>
/// Méthode qui s'active au changement d'un fantome.
///
/// Fait par AVERTY Pierre le 10/04/2022 et modifiée le 17/04/2022
/// </summary> 
    void Update() {
        
           
            _worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _worldPosition.y = Terrain.activeTerrain.SampleHeight(_worldPosition);

            transform.position = _worldPosition;

            if(Input.GetMouseButtonDown(0)){
                AgentManager.Instance.newAgentInSim(_worldPosition, Attributes);

                n--;
            }
            if(n == 0){
                _camera.GetComponent<AgentCamera>().ExitAgentLook();
                Destroy(gameObject);
            }
                
    }

}
