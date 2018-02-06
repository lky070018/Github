using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : Photon.MonoBehaviour {

    // 已選角色
    public GameObject teamAPlayer1, teamAPlayer2, teamAPlayer3, teamAPlayer4;
    public GameObject teamBPlayer1, teamBPlayer2, teamBPlayer3, teamBPlayer4;
    public GameObject characterMenu;
    public GameObject menu;
    // Use this for initialization
    void Start () {

	}

    // Update is called once per frame
    void Update() {
        teamAPlayer1 = GameObject.Find("fang2(Clone)");
         
	}
}
