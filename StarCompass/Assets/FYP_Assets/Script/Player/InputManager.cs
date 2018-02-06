using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public float horizontal;
    public float vertical;
    public float mouseX;
    public float mouseY;
    public float rotX;
    public float rotY;
    public float inputSensitivityX = 200f;
    public float inputSensitivityY = 150f;
    public StateManager stateManger;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        PlayerInput();

	}

    void PlayerInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseX * inputSensitivityX * Time.deltaTime;
        rotX += mouseY * inputSensitivityY * Time.deltaTime;
        if (!stateManger.inSpace)
        {
            if (rotX < -80)
            {
                rotX = -80;
            }
            else if (rotX > 80)
            {
                rotX = 80;
            }
        }
        else
        {
            if(rotX < -360)
            {
                rotX = -360;
            }
            else if(rotX > 360)
            {
                rotX = 360;
            }
        }

    }
}
