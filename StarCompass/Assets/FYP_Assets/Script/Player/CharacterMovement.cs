using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    float horizontal;
    float vertical;
    public Vector3 velocity;
    Rigidbody rb;
    Animator anim;
    Ray ray;

    public float moveSpeed = 4;
    
    public float jumpSpeed;

    public CameraLookPos cLookPos;
    InputManager inputManager;
    StateManager stateManager;
    public float rotY = 0.0f;
    public float rotX = 0.0f;
    OnPlatformMovement onPlatformMovement;
    public Vector3 dir;
    public int energy = 10;
    // test base
    public Collider col;
    bool colTimer = false;
    float colTime = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //Cursor.lockState = CursorLockMode.Locked;
        onPlatformMovement = GetComponent<OnPlatformMovement>();
        inputManager = GetComponent<InputManager>();
        stateManager = GetComponent<StateManager>();
        
    }
    private void Update()
    {
        if (colTimer)
        {
            colTime += Time.deltaTime;
            if(colTime > 1f)
            {
                col.enabled = true;
                colTime = 0;
                colTimer = false;
            }
        }
        dir = cLookPos.targetingpoint - transform.position;
        //Debug for test
        if (cLookPos != null)
        {
            Vector3 dir2 = cLookPos.targetingpoint - transform.position;
            Debug.DrawRay(transform.position, dir2 * 10, Color.red);
        }
        velocity = rb.velocity;
        if (!stateManager.inSpace)
        {
            Movement();
            PlayerRotation();

        }
        else
        {
          /* 跳既時侯個ROTATE
           * 
           * if (isJump)
            {
                transform.eulerAngles = -camera.transform.eulerAngles;
            }*/
            
           // this.enabled = false;
        }
    }

    void Movement()
    {
        horizontal = inputManager.horizontal;
        vertical = inputManager.vertical;

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        

        anim.SetFloat("MoveLR",horizontal);
        anim.SetFloat("MoveFB", vertical);
        
        if (Input.GetKeyDown(KeyCode.Space) && stateManager.canjump)
        {
            anim.SetBool("Jump", true);
        }
    }
    public void JumpEvents()
    {
        if (cLookPos != null)
        {
            col.enabled = false;
            colTimer = true;

            rb.drag = 0.01f;
            transform.rotation = cLookPos.transform.rotation;
            //transform.rotation = Quaternion.Lerp(transform.rotation, cLookPos.transform.rotation,10*Time.deltaTime);
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.velocity = dir * jumpSpeed * 20 * Time.deltaTime;
            stateManager.isJump = true;
            if (energy < 5)
            {
                energy += 1;
            }
            
        }

    }
    public void JumpFail()
    {
        stateManager.isJump = false;
        if (!stateManager.inSpace)
        {
            anim.SetBool("Jump", false);
            rb.useGravity = true;
            rb.Sleep();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WaitingRoom")
        {
            stateManager.inSpace = false;
            rb.useGravity = true;
            anim.SetBool("Jump", false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "WaitingRoom")
        {
            stateManager.inSpace = false;
            rb.useGravity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "WaitingRoom")
        {
            stateManager.inSpace = true;
            rb.useGravity = false;
        }
    }
    public void PlayerRotation()
    {
        rotY = inputManager.rotY;
        rotX = inputManager.rotX;

        Quaternion localRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = localRotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            rb.drag += 1f;
        }
        if(collision.gameObject.tag == "Wall")
        {
            ContactPoint cp = collision.contacts[0];
            // calculate with addition of normal vector
            // myRigidbody.velocity = oldVel + cp.normal*2.0f*oldVel.magnitude;

            // calculate with Vector3.Reflect
            rb.velocity = Vector3.Reflect(velocity, cp.normal);

            // bumper effect to speed up ball
            rb.velocity += cp.normal * 2.0f;
        }
    }
}
