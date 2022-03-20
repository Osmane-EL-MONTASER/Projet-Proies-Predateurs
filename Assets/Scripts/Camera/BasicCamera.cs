using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe codée par Osmane EL MONTASER pour le prototype. Elle
/// est insiprée d'une vidéo qui explique comment contrôler la
/// caméra. Brackeys, https://www.youtube.com/watch?v=_QajrabyTJc.
/// </summary>
public class BasicCamera : MonoBehaviour {
    /// <summary>
    /// La sensibilité de la souris qui affectera à quelle vitesse
    /// la caméra pivote.
    /// </summary>
    public float MouseSensitivity = 100f;

    /// <summary>
    /// Le wrapper de la caméra.
    /// </summary>
    public Transform PlayerBody;

    /// <summary>
    /// L'objet de type Capsule Collider à bouger afin de pouvoir 
    /// détecter les collisions et bien orienter la caméra.
    /// </summary>
    public Transform PlayerCollisions;
    
    /// <summary>
    /// La valeur de rotation actuelle de la caméra sur l'axe x.
    /// </summary>
    private float _xRotation = 0f;

    /// <summary>
    /// La fonction exécutée au démarrage.
    /// 
    /// Fait par Osmane EL MONTASER le 27/02/2022.
    /// </summary>
    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// La fonction exécutée à chaque tick.
    /// 
    /// Fait par Osmane EL MONTASER le 27/02/2022.
    /// </summary>
    void Update() {
        HandleMouseControl();
    }

    /// <summary>
    /// Gestion du mouvement de la caméra avec la souris.
    /// 
    /// Ajoutée par Osmane EL MONTASER le 13/02/2022.
    /// Insipirée de la vidéo de Brackeys :
    /// https://www.youtube.com/watch?v=_QajrabyTJc.
    /// </summary>
    private void HandleMouseControl() {
        if(Input.GetMouseButton(1)) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            PlayerCollisions.Rotate(Vector3.up * mouseX);
        } else if(Input.GetMouseButtonUp(1)) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
