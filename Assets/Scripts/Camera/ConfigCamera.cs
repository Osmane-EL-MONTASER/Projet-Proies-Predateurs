using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton qui représente le paramètres de la caméra.
/// 
/// Fait par AVERTY Pierre le 14/03/2022.
/// </summary>
public class ConfigCamera : MonoBehaviour {

/// <summary>
/// Instance du singleton.
/// </summary>
    private static ConfigCamera instance = null;

/// <summary>
/// Nom de la caméra courante.
/// </summary>
    private string currentName {get; set; }

/// <summary>
/// Camera actuelle.
/// </summary>
    private GameObject _currentCamera {get; set; }
/// <summary>
/// Constructeur de la classe mis en privé pour faire un singleton.
/// 
/// Fait par AVERTY Pierre le 14/03/2022.
/// </summary>
    private ConfigCamera() {
        _currentCamera = GameObject.Find("Main Camera");
    }

/// <summary>
/// Méthode qui crée l'instance du singleton et si elle existe déjà, la retourne.
/// 
/// Fait par AVERTY Pierre le 14/03/2022.
/// </summary>
     public static ConfigCamera Instance {
        get {
            if (instance==null) {
                instance = new ConfigCamera();
            }
            return instance;
        }
    }

/// <summary>
/// Getter de la caméra actuelle.
/// 
/// Fait par AVERTY Pierre le 14/03/2022.
/// </summary>
     public GameObject CurrentCamera {
        get {
            return _currentCamera;
        }
        set {
            _currentCamera = value;
        }
    }
}
