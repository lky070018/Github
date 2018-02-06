using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpArea : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RedTeamCaptain")
        {
            other.GetComponent<StateManager>().canjump = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RedTeamCaptain")
        {
            other.GetComponent<StateManager>().canjump = false;

        }
    }
}
