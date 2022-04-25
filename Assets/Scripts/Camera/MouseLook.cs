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
    public float mouseSensitivity = 100f;

    /// <summary>
    /// L'objet de type Capsule Collider à bouger afin de pouvoir 
    /// détecter les collisions et bien orienter la caméra.
    /// </summary>
    public Transform playerBody;
    
    /// <summary>
    /// La valeur de rotation actuelle de la caméra sur l'axe x.
    /// </summary>
    private float xRotation = 0f;

    /// <summary>
    /// La vitesse de déplacement de la caméra à chaque tick.
    /// </summary>
    public float cameraSpeed = 100f;

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
        handleMouseControl();
        handleKeyControl();
    }

    /// <summary>
    /// Gestion du mouvement de la caméra avec le clavier.
    /// Utilise les touches Z, Q, S, D pour se déplacer.
    /// Espace, et left ctrl pour monter ou descendre.
    /// 
    /// Fait par Osmane EL MONTASER le 13/02/2022.
    /// </summary>
    private void handleKeyControl() {
        if(Input.GetKey(KeyCode.LeftControl))
            cameraSpeed = 600f;
        else
            cameraSpeed = 100f;

        if(Input.GetKey("space"))
            playerBody.Translate(Vector3.up * Time.deltaTime * cameraSpeed, Space.World);
        if(Input.GetKey(KeyCode.LeftShift))
            playerBody.Translate(Vector3.down * Time.deltaTime * cameraSpeed, Space.World);

        if(Input.GetKey(KeyCode.Q))
            playerBody.Translate(Vector3.left * Time.deltaTime * cameraSpeed, playerBody);

        if(Input.GetKey(KeyCode.D))
            playerBody.Translate(Vector3.right * Time.deltaTime * cameraSpeed, playerBody);
        
        if(Input.GetKey(KeyCode.S))
            playerBody.Translate(Vector3.back * Time.deltaTime * cameraSpeed, playerBody);

        if(Input.GetKey(KeyCode.Z))
            playerBody.Translate(Vector3.forward * Time.deltaTime * cameraSpeed, playerBody);
    }

    /// <summary>
    /// Gestion du mouvement de la caméra avec la souris.
    /// 
    /// Ajoutée par Osmane EL MONTASER le 13/02/2022.
    /// Insipirée de la vidéo de Brackeys :
    /// https://www.youtube.com/watch?v=_QajrabyTJc.
    /// </summary>
    private void handleMouseControl() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
