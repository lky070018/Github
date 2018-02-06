using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour {
    public GameObject MainGun;
    public GameObject HookGun;
    public GameObject SubWeapons;

	// Use this for initialization
	void Start () {
        MainGun.SetActive(true);
        HookGun.SetActive(false);
        SubWeapons.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        WeaponList();

    }

    void WeaponList()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MainGun.SetActive(true);
            HookGun.SetActive(false);
            SubWeapons.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MainGun.SetActive(false);
            HookGun.SetActive(true);
            SubWeapons.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MainGun.SetActive(false);
            HookGun.SetActive(false);
            SubWeapons.SetActive(true);
        }
    }
}
