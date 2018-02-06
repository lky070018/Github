using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGun : MonoBehaviour {
    public GameObject Player;
    public CharacterMovement characterMovement;
    public GameObject bullet;
    public Transform shootPos;
    private float shootSpeed = 3000;
    public OnPlatformMovement onPlatformMovement;
    public StateManager stateManager;
    private void Start()
    {
        characterMovement = Player.GetComponent<CharacterMovement>();
        onPlatformMovement = Player.GetComponent<OnPlatformMovement>();
        stateManager = Player.GetComponent<StateManager>();
    }
    private void Update()
    {
        Shooting();
    }
    void Shooting()
    {
        //Vector3 forward = transform.InverseTransformDirection(shootPos.forward);
        if (Input.GetButtonDown("Fire1") && characterMovement.energy > 0 && stateManager.inSpace)
        {
            characterMovement.energy -= 1;
            GameObject bullets = null;
            bullets = PhotonNetwork.Instantiate("Bullet", shootPos.position, Quaternion.LookRotation(-characterMovement.dir), 0);
            //  bullets = Instantiate(bullet, shootPos.position, bullet.transform.localRotation);
            bullets.GetComponent<Rigidbody>().AddForce(characterMovement.dir * shootSpeed * Time.deltaTime);
           // bullets.transform.rotation = Quaternion.LookRotation(-characterMovement.dir);

        }
        

    }

}
