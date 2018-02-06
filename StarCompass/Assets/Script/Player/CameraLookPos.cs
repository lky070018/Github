using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookPos : Photon.MonoBehaviour
{
    public InputManager inputManager;
    public CharacterMovement characterMovement;
    public float CameraMoveSpeed = 120.0f;
    public GameObject cameraFollowTar;
    public GameObject cameraFollowTar2;
    public GameObject cam;
    public StateManager stateManager;

    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;

    public float rotY = 0.0f;
    public float rotX = 0.0f;

    public Vector3 targetingpoint;
    public OnPlatformMovement onPlatformMovement;
    // Use this for initialization
    void Start () {

        //rot = transform.localRotation.eulerAngles;
        //rotY = rot.y;
        //rotX = rot.x;
            Cursor.lockState = CursorLockMode.Locked;
            cam = this.transform.GetChild(0).gameObject;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //need big fix, the change of angle of 
        //the player's tranform make the camera go snappinig,
        //need a way to stayble it
        //dont just parent it, it is the same.  

        CameraState();        
        TargetingRay();
    }

    void CameraUpdater(Transform target)
    {
        target.rotation = target.root.rotation;

        transform.root.rotation = Quaternion.Slerp(transform.root.rotation, target.rotation, Time.deltaTime * 20);
        bool single = false;
        if (stateManager.isJump)
        {
            Quaternion localRotation = Quaternion.Euler(-rotX + 135, rotY, 0.0f); 
            transform.localRotation = localRotation;
        }else
        {
            single = false;
            Quaternion localRotation = Quaternion.Euler(-rotX, 0.0f, 0.0f);
            transform.localRotation = localRotation;
        }

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.root.position = Vector3.MoveTowards(transform.root.position, target.position, step);
    }

    void CameraState()
    {

        Transform target = null;
        //rotation
        rotX = inputManager.rotX;
        rotY = inputManager.rotY;
        if (stateManager.onPlatform)
        {
            rotX = Mathf.Clamp(rotX , 0 , 135);
        }
        else
        {
            if (stateManager.isJump)
            {
                rotX = Mathf.Clamp(rotX, 0, 135);
            }
            else
            {
                rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
            }
        }
        //set target to follow
        if (stateManager.inSpace)
        {
            target = cameraFollowTar2.transform;
        }
        else
        {
            target = cameraFollowTar.transform;
        }

        CameraUpdater(target);
    }

    void TargetingRay()
    {
        if (characterMovement != null && cam != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100))
            {
                targetingpoint = hit.point;
            }
            Debug.DrawLine(cam.transform.position, targetingpoint, Color.red);
        }
    }
}
