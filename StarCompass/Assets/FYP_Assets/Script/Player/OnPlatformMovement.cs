using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlatformMovement : MonoBehaviour {
    Animator anim;
    Rigidbody rig;
    Collider col;
    ClimbTest climbTest;
    CharacterMovement cm;
    InputManager inputManager;
    StateManager stateManager;

    float delta;
    public bool isClimbing;
    //吸力
    float speed = 1f;
    // 目標playform
    public GameObject TargetPlatform;
    // 場上所有platform
    public GameObject[] Platform;
    // 萬有引力場
    public bool targetDir = false;
    //地上
    //public bool onPlatform = false;

    // Animator IK
    public Transform rightIKTarger;
    public float ikWeight;

    float rotY, rotX;

    public Camera camera;


    bool single = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        cm = GetComponent<CharacterMovement>();
        climbTest = GetComponent<ClimbTest>();
        inputManager = GetComponent<InputManager>();
        stateManager = GetComponent<StateManager>();
    }

    void MagnetFunction()
    {
        float dis = Vector3.Distance(transform.position, TargetPlatform.transform.position);
        transform.position = Vector3.Lerp(transform.position, TargetPlatform.transform.position, Time.deltaTime * speed);
    }
    //private void FixedUpdate()
    //{
    //    if (isClimbing)
    //    {
    //        float dis = Vector3.Distance(transform.position, TargetPlatform.transform.position);
    //        if (dis > 3/* && !onPlatform*/)
    //        {
    //            //MagnetFunction();
    //        }
    //        return;
    //    }
        
    //}
    void Update()
    {
        delta = Time.deltaTime;
        //FindClosestObj();
        if (stateManager.inSpace)
        {
            
            if (single)
            {
                if (climbTest.CheckForClimb())
                {
                    stateManager.isJump = false;
                    stateManager.onPlatform = true;
                    rig.isKinematic = true;
                    anim.SetBool("onPlatform", true);
                    single = false;
                    anim.SetBool("OnShootingPose", true);
                    ikWeight = 1f;

                }
                else
                {
                    anim.SetBool("OnShootingPose", false);
                    ikWeight = 0f;
                }

            }

            if (Input.GetKeyDown(KeyCode.Space) && stateManager.onPlatform)
            {
                anim.SetBool("onPlatformJump", true);
                //  col.enabled = true;
                isClimbing = false;
            }

            if (stateManager.isJump)
            {
                anim.SetBool("onPlatform", false);
                stateManager.onPlatform = false;
            }

            if (stateManager.onPlatform)
            {

                Ray ray = new Ray(camera.transform.position,camera.transform.forward);

                rightIKTarger.position = ray.GetPoint(15);
                if (!stateManager.isJump)
                {
                    climbTest.Climb();
                }


                //if (Input.GetButton("Fire2"))
                //{
                //    //float mouseX = Input.GetAxis("Mouse X");
                //    //float mouseY = Input.GetAxis("Mouse Y");
                //    //float clampAngle = 80.0f;
                //    //rotX += mouseX  * Time.deltaTime*0.01f;
                //    //rotY   += mouseY  * Time.deltaTime*0.01f;
                //    //rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
                //float screenX = Screen.width / 2;
                //float screenY = Screen.height / 2f;
                //Ray ray = camera.ScreenPointToRay(new Vector3(screenX, screenY, 0));
                //RaycastHit hit;

                //    //if(Physics.Raycast(ray,out hit))
                //    //{
                //    //    rightIKTarger.transform.position = hit.point;
                //    //    //rightIKTarger.transform.rotation = Quaternion.irection);
                //    //    rightIKTarger.LookAt(hit.point);
                //    //    Debug.DrawRay(transform.position,hit.point,Color.red);
                //    //}

                //    //anim.SetBool("OnShootingPose", true);
                //    //ikWeight = 1;

                //}
                //else
                //{
                //    anim.SetBool("OnShootingPose", false);
                //}
            }
            else
            {
                single = true;
            }
        }
    }

    public GameObject FindClosestObj()
    {
        Platform = GameObject.FindGameObjectsWithTag("platform");
        TargetPlatform = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in Platform)
        {
            Vector3 diff = go.transform.position - position;
            float currDistance = diff.sqrMagnitude;
            if (currDistance < distance)
            {
                TargetPlatform = go;
                distance = currDistance;
            }
        }
        return TargetPlatform;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "PlatformField")
    //    // if (other.gameObject == TargetPlatform)
    //    {
    //        isClimbing = true;
    //        anim.SetBool("onPlatform",true);
    //        anim.SetBool("onPlatformJump", false);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //   if(other.tag == "PlatformField")
    //    {
    //        isClimbing = false;
    //        onPlatform = false;
    //    }
    //}

    public void Flying()
    {
        rig.isKinematic = false;//make the player can be bounce away
        anim.SetBool("onPlatformJump", false);
    }

    public void OnPlatformJumpFail()//did not know what is this for,didnt use
    {
        if (!isClimbing)
        {
            rig.isKinematic = false;
            anim.SetBool("onPlatformJump", false);
        }
    }
    private void OnAnimatorIK()
    {
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightIKTarger.position);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightIKTarger.rotation);
    }
}
