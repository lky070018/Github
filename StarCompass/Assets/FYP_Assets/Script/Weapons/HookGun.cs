using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookGun : MonoBehaviour {
    public GameObject Player;
    Rigidbody rb;
    public Camera camera;
    public GameObject Hook;
    public int force;
    public Transform shootSpot;
    public LineRenderer LR;
    bool isShoot;
    bool isFly;
    Vector3 hookPos;
    public Collider col;
   // public CharacterMovement characterMovement;
    //射出時間
    public float CDtime;
    private bool coolingTime;
    public OnPlatformMovement onPlatformMovement;
    public StateManager stateManager;
    public CharacterMovement characterMovement;
    private void Start()
    {
        characterMovement = Player.GetComponent<CharacterMovement>();
        col = GetComponent<Collider>();
        Physics.IgnoreLayerCollision(8,8);
        rb = this.GetComponent<Rigidbody>();
        onPlatformMovement = Player.GetComponent<OnPlatformMovement>();
        stateManager = Player.GetComponent<StateManager>();
        // Physics.IgnoreCollision(Player.GetComponent<Collider>(), GetComponent<Collider>());
      //  characterMovement = Player.GetComponent<CharacterMovement>();
    }
    private void Update()
    {
        if(Physics.Raycast(transform.position,transform.forward))
        if (characterMovement.energy > 0 &&  stateManager.inSpace)
        {
            shootEnemy();
        }
    }

    void Aim()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(new Vector3(screenX, screenY));

        if(Physics.Raycast(ray,out hit))
        {
            transform.LookAt(hit.point);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "platform")
        {
            hookPos = transform.position;
            isFly = true;
            CDtime = 5;
            isShoot = false;
        }
    }
    void shootEnemy()
    {

        if (Input.GetButtonDown("Fire1") && !isFly && !coolingTime)
        {
            LR.enabled = true;
            isShoot = true;
            col.enabled = true;
            coolingTime = true;
            CDtime = 0;
        }
        if (isShoot)
        {
            Vector3 forward = transform.InverseTransformDirection(transform.forward);
            //  Vector3 dir = characterMovement.dir - transform.position;
            Vector3 dir = Player.transform.position - transform.position;
            //transform.Translate(forward * force * Time.deltaTime);
            transform.Translate(characterMovement.dir * Time.deltaTime);
            transform.LookAt(characterMovement.dir);
            
            LR.SetPosition(0, shootSpot.position);
            LR.SetPosition(1, Hook.transform.position);
            CDtime += Time.deltaTime;
        }
        else
        {
            LR.enabled = false;
        }
        if (CDtime >= 3)
        {
            LR.SetPosition(1, shootSpot.position);
            LR.SetPosition(0, Hook.transform.position);
            transform.position = Vector3.Lerp(transform.position, shootSpot.position, 10f * Time.deltaTime);
        }
        if(CDtime >= 4)
        {
            isShoot = false;
            coolingTime = false;
        }
        if (isFly)
        {
            transform.position = hookPos;
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, transform.position, 10 * Time.deltaTime);
            Player.transform.LookAt(transform.position);
            float dis = Vector3.Distance(Player.transform.position, transform.position);
            if (dis <= 2)
            {
                isFly = false;
                transform.position = shootSpot.position;
            }
        }

    }

}
