using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoController : MonoBehaviour
{
    // Start is called before the first frame update
    public WheelJoint2D backWheelController;

    public float speed = 1000;
    public float keyBoardInputMovement;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        keyBoardInputMovement = Input.GetAxisRaw("Vertical") * speed;
    }

    private void FixedUpdate()
    {
        if (keyBoardInputMovement == 0.0f)
        {
            backWheelController.useMotor = false;
        }
        else
        {
            backWheelController.useMotor = true;
            JointMotor2D motor = new JointMotor2D() {motorSpeed = keyBoardInputMovement, maxMotorTorque = 100000};
            backWheelController.motor = motor;
        }
        
    }
}
