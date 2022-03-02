using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe codée par Osmane EL MONTASER pour le prototype. Elle
/// est insiprée d'une vidéo qui explique comment contrôler la
/// caméra. Brackeys, https://www.youtube.com/watch?v=_QajrabyTJc.
/// </summary>
public class MouseLook : MonoBehaviour {
    /// <summary>
    /// La sensibilité de la souris qui affectera à quelle vitesse
    /// la caméra pivote.
    /// </summary>
    public float MouseSensitivity = 100f;

    /// <summary>
    /// L'objet de type Capsule Collider à bouger afin de pouvoir 
    /// détecter les collisions et bien orienter la caméra.
    /// </summary>
    public Transform PlayerBody;
    
    /// <summary>
    /// La valeur de rotation actuelle de la caméra sur l'axe x.
    /// </summary>
    private float _xRotation = 0f;

    /// <summary>
    /// La vitesse de déplacement de la caméra à chaque tick.
    /// </summary>
    public float CameraSpeed = 100f;

    /// <summary>
    /// La fonction exécutée au démarrage.
    /// 
    /// Fait par Osmane EL MONTASER le 27/02/2022.
    /// </summary>
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// La fonction exécutée à chaque tick.
    /// 
    /// Fait par Osmane EL MONTASER le 27/02/2022.
    /// </summary>
    void Update() {
        HandleMouseControl();
        HandleKeyControl();
    }

    /// <summary>
    /// Gestion du mouvement de la caméra avec le clavier.
    /// Utilise les touches Z, Q, S, D pour se déplacer.
    /// Espace, et left ctrl pour monter ou descendre.
    /// 
    /// Fait par Osmane EL MONTASER le 13/02/2022.
    /// </summary>
    private void HandleKeyControl() {
        if(Input.GetKey(KeyCode.LeftControl))
            CameraSpeed = 600f;
        else
            CameraSpeed = 100f;

        if(Input.GetKey("space"))
            PlayerBody.Translate(Vector3.up * Time.deltaTime * CameraSpeed, Space.World);
        if(Input.GetKey(KeyCode.LeftShift))
            PlayerBody.Translate(Vector3.down * Time.deltaTime * CameraSpeed, Space.World);

        if(Input.GetKey(KeyCode.Q))
            PlayerBody.Translate(Vector3.left * Time.deltaTime * CameraSpeed, PlayerBody);

        if(Input.GetKey(KeyCode.D))
            PlayerBody.Translate(Vector3.right * Time.deltaTime * CameraSpeed, PlayerBody);
        
        if(Input.GetKey(KeyCode.S))
            PlayerBody.Translate(Vector3.back * Time.deltaTime * CameraSpeed, PlayerBody);

        if(Input.GetKey(KeyCode.Z))
            PlayerBody.Translate(Vector3.forward * Time.deltaTime * CameraSpeed, PlayerBody);
    }

    /// <summary>
    /// Gestion du mouvement de la caméra avec la souris.
    /// 
    /// Ajoutée par Osmane EL MONTASER le 13/02/2022.
    /// Insipirée de la vidéo de Brackeys :
    /// https://www.youtube.com/watch?v=_QajrabyTJc.
    /// </summary>
    private void HandleMouseControl() {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        PlayerBody.Rotate(Vector3.up * mouseX);
    }
}
