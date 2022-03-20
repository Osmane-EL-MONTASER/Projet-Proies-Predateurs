using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerMovements : MonoBehaviour {

    private Vector3 _lastTranslation;

    private Transform _lastRelativeToTransform;

    /// <summary>
    /// L'objet de type Capsule Collider à bouger afin de pouvoir 
    /// détecter les collisions et bien orienter la caméra.
    /// </summary>
    public Transform PlayerBody;

    /// <summary>
    /// Les particules de pluie qui tombent autour de la caméra.
    /// </summary>
    public GameObject RainParticles;

    /// <summary>
    /// La vitesse de déplacement de la caméra à chaque tick.
    /// </summary>
    public float CameraSpeed = 100f;
    
    void Start() {

    }

    void Update() {
        if(Input.GetKey(KeyCode.LeftControl))
            CameraSpeed = 600f;
        else
            CameraSpeed = 100f;

        if(Input.GetKey("space")) {
            PlayerBody.Translate(Vector3.up * Time.deltaTime * CameraSpeed, Space.World);
            RainParticles.GetComponent<Transform>().Translate(Vector3.up * Time.deltaTime * CameraSpeed, Space.World);
            _lastTranslation = Vector3.up * Time.deltaTime * CameraSpeed;
        }
            
        if(Input.GetKey(KeyCode.LeftShift)) {
            PlayerBody.Translate(Vector3.down * Time.deltaTime * CameraSpeed, Space.World);
            RainParticles.GetComponent<Transform>().Translate(Vector3.down * Time.deltaTime * CameraSpeed, Space.World);
            _lastTranslation = Vector3.down * Time.deltaTime * CameraSpeed;
        }
            
        if(Input.GetKey(KeyCode.Q)) {
            PlayerBody.Translate(Vector3.left * Time.deltaTime * CameraSpeed, PlayerBody);
            RainParticles.GetComponent<Transform>().Translate(Vector3.left * Time.deltaTime * CameraSpeed, PlayerBody);
            _lastRelativeToTransform = PlayerBody;
            _lastTranslation = Vector3.left * Time.deltaTime * CameraSpeed;
        }
            
        if(Input.GetKey(KeyCode.D)) {
            PlayerBody.Translate(Vector3.right * Time.deltaTime * CameraSpeed, PlayerBody);
            RainParticles.GetComponent<Transform>().Translate(Vector3.right * Time.deltaTime * CameraSpeed, PlayerBody);
            _lastRelativeToTransform = PlayerBody;
            _lastTranslation = Vector3.right * Time.deltaTime * CameraSpeed;
        }
            
        if(Input.GetKey(KeyCode.S)) {
            PlayerBody.Translate(Vector3.back * Time.deltaTime * CameraSpeed, PlayerBody);
            RainParticles.GetComponent<Transform>().Translate(Vector3.back * Time.deltaTime * CameraSpeed, PlayerBody);
            _lastRelativeToTransform = PlayerBody;
            _lastTranslation = Vector3.back * Time.deltaTime * CameraSpeed;
        }

        if(Input.GetKey(KeyCode.Z)) {
            PlayerBody.Translate(Vector3.forward * Time.deltaTime * CameraSpeed, PlayerBody);
            RainParticles.GetComponent<Transform>().Translate(Vector3.forward * Time.deltaTime * CameraSpeed, PlayerBody);
            _lastRelativeToTransform = PlayerBody;
            _lastTranslation = Vector3.forward * Time.deltaTime * CameraSpeed;
        }
    }

    void OnCollisionStay() {
        if(_lastRelativeToTransform != null) {
            PlayerBody.Translate(-1 * _lastTranslation, _lastRelativeToTransform);
            RainParticles.GetComponent<Transform>().Translate(-1 * _lastTranslation, _lastRelativeToTransform);            
        } else {
            PlayerBody.Translate(-1 * _lastTranslation, Space.World);
            RainParticles.GetComponent<Transform>().Translate(-1 * _lastTranslation, Space.World);
        }
        
    }
}