using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTest : MonoBehaviour
{
    public InputManager inputManager;
    public CharacterMovement characterMovement;
    public Animator anim;
    Rigidbody rig;

    public bool inPosition;
    public bool isLerping;
    float post;
    public float t;
    public Vector3 startPos;
    Vector3 targetPos;
    public Quaternion startRot;
    Quaternion targetRot;
    public float possitionOffset;
    public float offsetFromWall = 0.4f;
    public float speed_mutiplier = 0.2f;
    public float climbSpeed = 3;
    public float rotateSpeed = 5;

    public float rayTowardsMoveDir = 0.5f;
    public float rayForwardTowardsWall = 0.5f;

    public float hor;
    public float vert;

    public float rotY;
    public float sensitivityY = 200f;
    public float delta;
    Transform helper;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        characterMovement = GetComponent<CharacterMovement>();
        helper = new GameObject().transform;
        helper.name = "Climb helper";

    }

    // Update is called once per frame
    void Update()
    {
        delta = Time.deltaTime;
    }

    public bool CheckForClimb()
    {
        Vector3 origin = transform.position;
        origin.y += 0.3f;
        Vector3 dir = transform.forward;
        RaycastHit hit;

        Debug.DrawRay(origin, transform.forward * 3.5f, Color.white);
        if (Physics.Raycast(origin, dir, out hit, 3.5f))
        {
            helper.position = PositionWithOffset(origin, hit.point);
            InitForClimb(hit);
            return true;
        }
        else
        {
            startPos = transform.position;
            targetPos = transform.position;
            return false;
        }

    }

    void InitForClimb(RaycastHit hit)
    {
        helper.rotation = Quaternion.LookRotation(-hit.normal,transform.up);
        helper.rotation = Quaternion.Euler(helper.eulerAngles.x, helper.eulerAngles.y, rotY);
        startPos = transform.position;
        targetPos = hit.point + (hit.normal * offsetFromWall);
        t = 0;
        inPosition = false;
        rig.useGravity = false;
    }

    public void Tick()
    {

        if (!inPosition)
        {

            GetInPosition();
            return;
        }

        if (!isLerping)
        {            
            hor = inputManager.horizontal;
            vert = inputManager.vertical;

            Vector3 h = helper.right * hor;
            Vector3 v = helper.up * vert;
            Vector3 moveDir = (h + v).normalized;

            bool canMove = CanMove(moveDir);
            if (!canMove || moveDir == Vector3.zero)
                return;

            t = 0;
            isLerping = true;
            startPos = transform.position;
            targetPos = helper.position;

        }
        else
        {
            t += delta * climbSpeed;
            if (t > 1)
            {
                t = 1;
                isLerping = false;
            }

            Vector3 cp = Vector3.Lerp(startPos, targetPos, t );
            transform.position = cp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);

        }

    }

    public bool CanMove(Vector3 moveDir)
    {
        //can't control the rotation when the player is on top or on the bottom face of the mesh,    
        //i think it is the (-hit.normal) and player input is trying to overwrite each other,
        //buy why only happen on top or bottom?
        
        Vector3 origin = transform.position;

        float dis = possitionOffset;
        Vector3 dir = moveDir;
        Debug.DrawRay(origin, dir * dis, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            return false;
        }

        origin += moveDir * dis;
        dir = helper.forward;
        float dis2 = rayForwardTowardsWall;

        Debug.DrawRay(origin, dir * dis2, Color.blue);
        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            helper.position = PositionWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            helper.rotation = Quaternion.Euler(helper.eulerAngles.x, helper.eulerAngles.y, rotY);
            return true;
        }

        origin += dir * dis2;
        dir = -moveDir;

        Debug.DrawRay(origin, dir, Color.yellow);
        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            helper.position = PositionWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            helper.rotation = Quaternion.Euler(helper.eulerAngles.x, helper.eulerAngles.y, rotY);
            return true;
        }
        return false;
    }

    void GetInPosition()
    {
        t += delta;
        if (t > 1)
        {
            t = 1;
            inPosition = true;

        }

        Vector3 tp = Vector3.Lerp(startPos, targetPos, t * climbSpeed);
        transform.position = tp;
        transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
    }

    Vector3 PositionWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * offsetFromWall;
        return target + offset;
    }

    public void Climb()
    {
        rotY = -inputManager.rotY;

        Tick();
        helper.rotation = Quaternion.Euler(helper.eulerAngles.x, helper.eulerAngles.y, rotY);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, helper.eulerAngles.z);
    }
}

