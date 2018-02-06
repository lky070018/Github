using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerIsMine : Photon.MonoBehaviour {
    public InputManager inputManager;
    public StateManager stateManager;
    public OnPlatformMovement onPlatformMovement;
    public CharacterMovement characterMovement;
    public ClimbTest climbTest;
    public PlayerWeapons playerWeapons;
    public ShootGun shootGun;
    public HookGun hookGun;
    private void Start()
    {
        if (!photonView.isMine)
        {
            inputManager.enabled = false;
            stateManager.enabled = false;
            onPlatformMovement.enabled = false;
            characterMovement.enabled = false;
            climbTest.enabled = false;
            playerWeapons.enabled = false;
            hookGun.enabled = false;
            shootGun.enabled = false;

        }
    }
}
