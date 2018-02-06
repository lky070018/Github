using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redteam : MonoBehaviour {
    public GameObject blueTeamWin;
    public GameObject backButton;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "character_captainB(Clone)")
        {
            Cursor.lockState = (CursorLockMode.None);
            Cursor.visible = true;
            blueTeamWin.SetActive(true);
            backButton.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
