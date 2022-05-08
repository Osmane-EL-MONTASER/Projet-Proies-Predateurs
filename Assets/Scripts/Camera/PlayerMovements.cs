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

    public Rigidbody RigidBodyC;

    /// <summary>
    /// Les particules de pluie qui tombent autour de la caméra.
    /// </summary>
    public GameObject RainParticles;

    /// <summary>
    /// La vitesse de déplacement de la caméra à chaque tick.
    /// </summary>
    public float CameraSpeed = 100f;

    private bool isCollision = false;

    private Vector3 oldPos;

    private GameObject followedAgent;
    
    void Start() {

    }

    void Update() {
        RigidBodyC.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void OnCollisionEnter(Collision other) {
        isCollision = true;
        PlayerBody.position = oldPos;
    }

    void OnCollisionExit(Collision other) {
        isCollision = false;
    }

    void FixedUpdate() {
        if(!isCollision) {
            if(Input.GetKey("space")) {
                oldPos = RigidBodyC.position;
                Vector3 tempVect = new Vector3(0, 1, 0);
                tempVect = tempVect.normalized * Time.deltaTime * CameraSpeed;
                RigidBodyC.MovePosition(transform.position + tempVect);
                //RainParticles.GetComponent<Transform>().Translate(Vector3.up * Time.deltaTime * CameraSpeed, Space.World);
                _lastTranslation = Vector3.up * Time.deltaTime * CameraSpeed;
            }
                
            if(Input.GetKey(KeyCode.LeftShift)) {
                oldPos = RigidBodyC.position;
                Vector3 tempVect = new Vector3(0, -1, 0);
                tempVect = tempVect.normalized * Time.deltaTime * CameraSpeed;
                RigidBodyC.MovePosition(transform.position + tempVect);
                //RainParticles.GetComponent<Transform>().Translate(Vector3.down * Time.deltaTime * CameraSpeed, Space.World);
                _lastTranslation = Vector3.down * Time.deltaTime * CameraSpeed;
            }
                
            if(Input.GetKey(KeyCode.Q)) {
                oldPos = RigidBodyC.position;
                Vector3 tempVect = new Vector3(-1, 0, 0);
                tempVect = tempVect.normalized * Time.deltaTime * CameraSpeed;
                RigidBodyC.MovePosition(transform.position + PlayerBody.transform.TransformDirection(tempVect));
                //RainParticles.GetComponent<Transform>().Translate(Vector3.left * Time.deltaTime * CameraSpeed, PlayerBody);
                _lastRelativeToTransform = PlayerBody;
                _lastTranslation = Vector3.left * Time.deltaTime * CameraSpeed;
            }
                
            if(Input.GetKey(KeyCode.D)) {
                oldPos = RigidBodyC.position;
                Vector3 tempVect = new Vector3(1, 0, 0);
                tempVect = tempVect.normalized * Time.deltaTime * CameraSpeed;
                RigidBodyC.MovePosition(transform.position + PlayerBody.transform.TransformDirection(tempVect));
                //RainParticles.GetComponent<Transform>().Translate(Vector3.right * Time.deltaTime * CameraSpeed, PlayerBody);
                _lastRelativeToTransform = PlayerBody;
                _lastTranslation = Vector3.right * Time.deltaTime * CameraSpeed;
            }
                
            if(Input.GetKey(KeyCode.S)) {
                oldPos = RigidBodyC.position;
                Vector3 tempVect = new Vector3(0, 0, -1);
                tempVect = tempVect.normalized * Time.deltaTime * CameraSpeed;
                RigidBodyC.MovePosition(transform.position + PlayerBody.transform.TransformDirection(tempVect));
                //RainParticles.GetComponent<Transform>().Translate(Vector3.back * Time.deltaTime * CameraSpeed, PlayerBody);
                _lastRelativeToTransform = PlayerBody;
                _lastTranslation = Vector3.back * Time.deltaTime * CameraSpeed;
            }

            if(Input.GetKey(KeyCode.Z)) {
                oldPos = RigidBodyC.position;
                Vector3 tempVect = new Vector3(0, 0, 1);
                tempVect = tempVect.normalized * Time.deltaTime * CameraSpeed;
                RigidBodyC.MovePosition(transform.position + PlayerBody.transform.TransformDirection(tempVect));
                //RainParticles.GetComponent<Transform>().Translate(Vector3.forward * Time.deltaTime * CameraSpeed, PlayerBody);
                _lastRelativeToTransform = PlayerBody;
                _lastTranslation = Vector3.forward * Time.deltaTime * CameraSpeed;
            }
        }

        if ((Input.GetKey(KeyCode.Z))||(Input.GetKey(KeyCode.Q))||(Input.GetKey(KeyCode.S))||(Input.GetKey(KeyCode.D))||(Input.GetKey("space"))||(Input.GetKey(KeyCode.LeftShift)))
            followedAgent = null;

        if ((Input.GetMouseButtonUp(0))&&(followedAgent == null))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)) // you can also only accept hits to some layer and put your selectable units in this layer
            {
                if(hit.collider.tag == "Animal")
                {
                    GameObject temp = hit.transform.gameObject;
                    followedAgent = temp.transform.Find("Cam").gameObject; // if using custom type, cast the result to type here
                } 
            }
        }
        else if (followedAgent != null)
        {
            PlayerBody.position = followedAgent.transform.position;
            PlayerBody.rotation = followedAgent.transform.rotation;
        }
    }

}