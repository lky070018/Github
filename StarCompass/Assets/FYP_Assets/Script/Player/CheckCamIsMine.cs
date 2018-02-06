using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCamIsMine : Photon.MonoBehaviour {
    public CameraLookPos cameraLookPos;
    public CameraCollision cameraCollision;
    public Camera camera;
	// Use this for initialization
	void Start () {
        if (photonView.isMine)
        {
            cameraLookPos.enabled = true;
            cameraCollision.enabled = true;
            camera.enabled = true;
        }
	}

}
