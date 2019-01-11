using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    //Input flags
    [HideInInspector] public float moveDirection;
    [HideInInspector] public float turnDirection;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool interact;
    [HideInInspector] public bool attack1;
    [HideInInspector] public bool attack2;
    [HideInInspector] public bool attack3;
    [HideInInspector] public bool attack4;
    [HideInInspector] public bool minimap;
    private bool lastMinimapClick;
    private bool currentMinimapClick;

    // Use this for initialization
    void Start () {
        minimap = true;
    }

    // Update is called once per frame
    void Update() {
        moveDirection = Input.GetAxis("Vertical");
        turnDirection = Input.GetAxis("Horizontal");

        jump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton4);
        interact = Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.JoystickButton5);

        attack1 = Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.JoystickButton0);
        attack2 = Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton1);
        attack3 = Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.JoystickButton2);
        attack4 = Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.JoystickButton3);

        //Clunky makeshift on off logic here...
        lastMinimapClick = currentMinimapClick;
        currentMinimapClick = Input.GetKey(KeyCode.M) || Input.GetKey(KeyCode.JoystickButton8);

        if (lastMinimapClick && !currentMinimapClick)
        {
            minimap = !minimap;
        }
    }
}
