using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Photon.MonoBehaviour {
    protected Animator playerAnim;
    [SerializeField]
    static public bool inSpace = false;

    public float hAxis;
    public float vAxis;
    // Use this for initialization
    [SerializeField]
    private float moveSpeed;
    public Rigidbody rb;
    bool isJumpFromPlat;
    public float mouseRot;
    public float rotSpeed = 120;
    public float PlayerDir;

    public CameraLookPos cameraLookPos;
    public CameraCollision cameraCollision;
    public Camera camera;
    public GameObject UI;

    void Start () {
        //Screen.lockCursor = true;
        rb = this.GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        if (!photonView.isMine)
        {
            this.enabled = false;
            cameraLookPos.enabled = false;
            cameraCollision.enabled = false;
        }
        else
        {
            UI.SetActive(true);
            camera.enabled = true;
        }

    }


    private void FixedUpdate()
    {
        if (inSpace)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _testVersion2.onPlatform)
            {
                playerAnim.SetBool("jumpPlatform", true);
            }
        }


    }
    // Update is called once per frame
    void Update () {
        movePlayerOnGround();
        playerAnimation();
    }
    private void movePlayerOnGround()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        if (!inSpace)
        {
            Vector3 movement = new Vector3(hAxis, 0, vAxis);
            transform.Translate(movement * moveSpeed * Time.deltaTime);

/*            mouseRot = Input.GetAxis("Mouse X");
            PlayerDir += mouseRot * rotSpeed * Time.deltaTime;
            Quaternion localRotation = Quaternion.Euler(0.0f, PlayerDir, 0.0f);
            transform.rotation = localRotation;
            */
        }
        else
        {// 阻力
            rb.drag = 0.3f;
        }

        mouseRot = Input.GetAxis("Mouse X");
        PlayerDir += mouseRot * rotSpeed * Time.deltaTime;
        Quaternion localRotation = Quaternion.Euler(0.0f, PlayerDir, 0.0f);
        transform.rotation = localRotation;

    }
    // 可行 可以望住CAM 方向跳
    // 可能要將佢變做public 獨立camera先可以放 network
    // public camera cam  >>> cam.tran.forword ***
    public void readyToJumpTest(float jumpSpeed2)
    {
        rb.AddForce(Camera.main.transform.forward* jumpSpeed2);
        playerAnim.SetBool("jumpPlatform", false);
        this.GetComponent<_testVersion2>().targetDir = false;

    }
    public void readyToJump(float jumpSpeed)
    {
        rb.AddForce(transform.forward * jumpSpeed);
        playerAnim.SetBool("jumpPlatform", false);


    }
    public void jumpFail()
    {
        if (!inSpace)
        {
            playerAnim.SetBool("isJump", false);
        }
        else
        {

        }
    }
    void playerAnimation()
    {
        if(vAxis >= 0.1f ||hAxis>=0.1f||hAxis<=-0.1f)
        {
            playerAnim.SetBool("isWalk",true);
        }
        else
        {
            playerAnim.SetBool("isWalk", false);
        }
        if (vAxis <= -0.1f)
        {
            playerAnim.SetBool("isBack", true);
        }
        else { playerAnim.SetBool("isBack", false); 
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
        
            playerAnim.SetBool("isJump", true);
                 }
        if (this.GetComponent<_testVersion2>().targetDir)
        {
            playerAnim.SetBool("isLand", true);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Field")
        {
            inSpace = true;
            rb.useGravity = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Field")
        {
            inSpace = false;
            rb.useGravity = true;
        }
    }


}
