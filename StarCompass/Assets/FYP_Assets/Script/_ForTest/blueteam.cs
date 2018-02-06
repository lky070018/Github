using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueteam : MonoBehaviour {

    public GameObject redTeamWin;
    public GameObject backButton;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "character_captain2(Clone)")
        {
            Cursor.lockState = (CursorLockMode.None);
            Cursor.visible = true;
            redTeamWin.SetActive(true);
            backButton.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
