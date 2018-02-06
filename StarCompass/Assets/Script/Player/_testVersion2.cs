using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _testVersion2 : Photon.MonoBehaviour {
    public Animator anim;
    public bool isClimbing;

    bool inPosition;
    bool isLerping;
    float t;
    Vector3 startPos;
    Vector3 targetPos;
    Quaternion startRot;
    Quaternion targetRot;
    public float possitionOffset;
    public float offsetFromPlatform = 0.3f;
    public float speed_multiplier = 0.2f;
    public float climbSpeed = 3;
    public float rotateSpeed = 5f;
    public float inAngleDis = 1;

    public float horizontal;
    public float vertical;

    Transform helper;
    float delta;
    //吸力
    float speed = 1f;
    // 目標playform
    public GameObject TargetPlatform;
    // 場上所有platform
    public GameObject[] Platform;
    // 萬有引力場
    public bool targetDir = false;
    //地上
    static public bool isGround = false;
    static public bool onPlatform = false;

    void Start()
    {
        if (!photonView.isMine)
        {
            this.enabled = false;
        }
        Init();
    }

    private void Init()
    {
        helper = new GameObject().transform;
        helper.name = "Climb helper";
        checkForClimb();
    }

    public void checkForClimb()
    {
        Vector3 origin = transform.position;
        origin.y += 1.4f;
        Vector3 dir = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, 5))
        {
            helper.position = PosWithOffset(origin, hit.point);
            InitForClimb(hit);
        }

    }
    void InitForClimb(RaycastHit hit)
    {
        isClimbing = true;
        helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
        startPos = transform.position;
        targetPos = hit.point + (hit.normal * offsetFromPlatform);
        t = 0;
        inPosition = false;
        anim.CrossFade("climb_idle", 2);
    }

    private void FixedUpdate()
    {
        if (targetDir)
        {
            MagnetFunction();
        }
        if (isGround)
        {
            //  PlayerController();
        }
        FindClosestObj();
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
    void MagnetFunction()
    {
        float dis = Vector3.Distance(transform.position, TargetPlatform.transform.GetChild(0).transform.position);
                 transform.position = Vector3.Lerp(transform.position, TargetPlatform.transform.GetChild(0).position, Time.deltaTime * speed);
              print(TargetPlatform.transform.GetChild(0).name);

     }
    /*  void PlayerController()
      {
          if (Input.GetKey(KeyCode.W))
          {
              transform.Translate((Vector3.left) * 5f * Time.deltaTime);
              print("Hit");
          }

          // 禁空白鍵跳既時侯 target Dir = false 再比力佢望住既方向跳 
          // target dir = TRUE先可以移動
      }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TargetPlatform)
        {
            targetDir = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TargetPlatform)
        {
            targetDir = false;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == TargetPlatform)
        {
            onPlatform = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == TargetPlatform)
        {
            onPlatform = false;
        }
    }
    
    private void Update()
    {
        delta = Time.deltaTime;
        float dis = Vector3.Distance(transform.position, TargetPlatform.transform.position);
        if (dis < 10)
        {
            Tick(delta);
        }
    }
    public void Tick(float delta)
    {
        if (!inPosition)
        {
            GetInPosition();
            return;
        }
        if (!isLerping)
        {

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            Vector3 h = helper.right * horizontal;
            Vector3 v = helper.up * vertical;
            Vector3 moveDir = (h + v).normalized;

            bool canMove = CanMove(moveDir);
            if (!canMove || moveDir == Vector3.zero)
                return;
            t = 0;
            isLerping = true;
            startPos = transform.position;
            // Vector3 tp = helper.position - transform.position;

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
           Vector3 cp = Vector3.Lerp(startPos, targetPos, t);
            transform.position = cp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
        }
    }
    bool CanMove(Vector3 moveDir)
    {
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
        float dis2 = inAngleDis;

        Debug.DrawRay(origin, dir * dis2, Color.blue);
        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            helper.position = PosWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }

        origin += dir * dis2;
        dir = -Vector3.up;

        Debug.DrawRay(origin, dir, Color.yellow);
        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            float angle = Vector3.Angle(helper.up, hit.normal);
            if (angle < 40)
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;

            }
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

            //enable the ik
        }

        Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
        //transform.position = tp;
        //transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
    }
    Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * offsetFromPlatform;
        return target + offset;
    }
}
