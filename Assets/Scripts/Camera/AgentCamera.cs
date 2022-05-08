using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe inspirée de la classe WeatherCamera faites par EL MUNTASER Osmane.
/// 
/// Fait par AVERTY Pierre le 16/04/2022.
/// </summary>
public class AgentCamera : MonoBehaviour {
    /// <summary>
    /// L'objet de type Capsule Collider à bouger afin de pouvoir 
    /// détecter les collisions et bien orienter la caméra.
    /// </summary>
    public Transform PlayerBody;

    /// <summary>
    /// La vitesse de déplacement de la caméra à chaque tick.
    /// </summary>
    public float CameraSpeed = 250f;

    /// <summary>
    /// Référence au script de vue de base avec la souris pour
    /// effectuer la transition avec celui-ci lorsque l'on en
    /// a terminé avec cette vue.
    /// </summary>
    public GameObject Camera;

    /// <summary>
    /// Variable qui permet la compatibilité du scrolling sous
    /// Mac OS. Plus d'informations sur la documentation Unity :
    /// https://docs.unity3d.com/ScriptReference/Input-mouseScrollDelta.html
    /// </summary>
    public float ScrollScale = 25f;

    /// <summary>
    /// Savoir si la transition vers la nouvelle caméra est
    /// terminée ou non.
    /// </summary>
    private bool _isTransitionFinished = false;

    /// <summary>
    /// Savoir si la transition de pour quitter cette vue
    /// est terminée ou pas.
    /// </summary>
    private bool _isExitTransitionFinished = false;

    /// <summary>
    /// Savoir si la transition en cours est celle qui nous
    /// permet de quitter ou d'entrer dans la vue.
    /// </summary>
    private bool _transitionType;

    /// <summary>
    /// Référence vers le script de rotation d'une caméra.
    /// </summary>
    private SmoothCameraRotation _cameraRotationScript;

    /// <summary>
    /// Référence vers le script de translation d'une caméra.
    /// </summary>
    private SmoothCameraTranslation _cameraTranslationScript;

    /// <summary>
    /// Variable contenant l'ancienne valeur de l'angle de
    /// la caméra avant le changement de caméra.
    /// </summary>
    private float _oldEulerAngle;

    /// <summary>
    /// L'ancienne hauteur de la caméra à remettre 
    /// lorsque l'on revient sur la caméra de base.
    /// </summary>
    private float _oldHeight;

    /// <summary>
    /// La fonction exécutée au démarrage.
    /// 
    /// Fait par AVERTY Pierre le 16/04/2022.
    /// </summary>
    void Start() {}

    /// <summary>
    /// La fonction exécutée à chaque tick.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    void Update() {
        HandleMouseControl();
        HandleKeyControl();

        if((_transitionType && !_isTransitionFinished) || (!_transitionType && !_isExitTransitionFinished))
            makeTransition();

        if(_isExitTransitionFinished && !_transitionType) {
            Camera.GetComponent<BasicCamera>().enabled = true;
            this.enabled = false;
        }
    }

    /// <summary>
    /// Sortir de la vue d'ajout d'agents pour revenir à la
    /// vue de base.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022 et modifiée par AVERTY Pierre le 16/04/2022.
    /// </summary>
    public void ExitAgentLook() {
        _isExitTransitionFinished = false;
        _transitionType = false;
        Camera.GetComponent<Camera>().orthographic = false;

        _cameraRotationScript = new SmoothCameraRotation(startRotation: transform.eulerAngles.x, targetRotation: _oldEulerAngle, rotationSpeed: 100f);
        _cameraTranslationScript = new SmoothCameraTranslation(startTranslation: transform.position.y, targetTranslation: _oldHeight, translationSpeed: 800f);
    }

    /// <summary>
    /// Entrer dans la vue d'ajout d'agents tout en sortant
    /// de celle de base.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022 et modifiée par AVERTY Pierre le 16/04/2022.
    /// </summary>
    public void EnterAgentLook() {
        Camera.GetComponent<BasicCamera>().enabled = false;
        this.enabled = true;
        _isTransitionFinished = false;
        _transitionType = true;

        _oldEulerAngle = transform.eulerAngles.x;
        _oldHeight = transform.position.y;
        
        _cameraRotationScript = new SmoothCameraRotation(startRotation: transform.eulerAngles.x, targetRotation: 90f, rotationSpeed: 100f);
        _cameraTranslationScript = new SmoothCameraTranslation(startTranslation: transform.position.y, targetTranslation: 25f, translationSpeed: 25f);
    }

    /// <summary>
    /// Permet d'actualiser la position de la caméra suivant
    /// la transition à réaliser.
    /// 
/// Fait par EL MONTASER Osmane le 12/03/20p22 et modifiée par AVERTY Pierre le 16/04/2022.
    /// </summary>
    private void makeTransition() {
        float nextRotation = _cameraRotationScript.GetNextRotation();
        float nextTranslation = _cameraTranslationScript.GetNextTranslation();

        if(nextRotation != -1f)
            transform.localRotation = Quaternion.Euler(nextRotation, 0f, 0f);

        if(nextTranslation != -1f)
            transform.position = new Vector3(transform.position.x, nextTranslation, transform.position.z);


        if(nextTranslation == -1f && nextRotation == -1f) {
            if(_transitionType) {
                _isTransitionFinished = true;
                Camera.GetComponent<Camera>().orthographic = true;
                Camera.GetComponent<Camera>().orthographicSize = 25f;
            }
            else
                _isExitTransitionFinished = true;
        }
    }

    /// <summary>
    /// Gestion du mouvement de la caméra avec le clavier.
    /// Utilise les touches Z, Q, S, D pour se déplacer.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    private void HandleKeyControl() {
        if(Input.GetKey(KeyCode.Q))
            PlayerBody.Translate(Vector3.left * Time.deltaTime * CameraSpeed, PlayerBody);

        if(Input.GetKey(KeyCode.D))
            PlayerBody.Translate(Vector3.right * Time.deltaTime * CameraSpeed, PlayerBody);
        
        if(Input.GetKey(KeyCode.S))
            PlayerBody.Translate(Vector3.back * Time.deltaTime * CameraSpeed, PlayerBody);

        if(Input.GetKey(KeyCode.Z))
            PlayerBody.Translate(Vector3.forward * Time.deltaTime * CameraSpeed, PlayerBody);

        if(Input.GetKey(KeyCode.Escape))
            ExitAgentLook();
    }

    /// <summary>
    /// Gestion du zoom dans la vue d'édition.
    /// 
    /// Ajoutée par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    private void HandleMouseControl() {
        Camera.GetComponent<Camera>().orthographicSize -= Input.mouseScrollDelta.y * ScrollScale;
    }
}
