using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : Photon.MonoBehaviour {
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;
    public float smoothTime = 0.8f;
    public float colDis = 0.87f;

    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update () {
        Vector3 destiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if(Physics.Linecast(transform.parent.position,destiredCameraPos,out hit))
        {
            
            distance = Mathf.Clamp((hit.distance * colDis), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;

        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
	}
}
