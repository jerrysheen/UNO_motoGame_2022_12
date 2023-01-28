using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoController : MonoBehaviour
{
    public enum MotoState
    {
        Changed = 0,
        Changing = 1,
    }

    public float keyBoardInputMovement;
    public List<GameObject> lanePoints;
    public float changeLaneTime;
    public MotoState motostate;
    public int currentLane = 0;
    void Start()
    {
        motostate = MotoState.Changed;
    }

    // Update is called once per frame
    void Update()
    {
        keyBoardInputMovement = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(keyBoardInputMovement) > 0.01)
        {
            StartChangeLane(keyBoardInputMovement);
        }

        //Debug.Log(keyBoardInputMovement);
    }

    /// <summary>
    /// w is positive while s is negative.
    /// </summary>
    /// <param name="keyboardValue"></param>
    private void StartChangeLane(float keyboardValue)
    {
        // top lane or bottom lane.
        if ((keyboardValue > 0.0f && currentLane == 0) ||
            (keyboardValue < 0.0f && currentLane == lanePoints.Count - 1))
        {
            return;
        }
        if (motostate == MotoState.Changing) return;
        motostate = MotoState.Changing;

        StartCoroutine(MoveToNextLane(keyboardValue, changeLaneTime));
    }

    IEnumerator MoveToNextLane(float keyboardValue, float changeLaneTime)
    {
        float restTime = changeLaneTime;
        int next = keyboardValue > 0 ? currentLane - 1 : currentLane + 1;
        Vector3 currPos = lanePoints[currentLane].transform.position;
        Vector3 nextPos = lanePoints[next].transform.position;
        Vector3 tempPos;
        while (restTime > 0)
        {
            tempPos = Vector3.Lerp(currPos, nextPos, 1 - restTime / changeLaneTime);
            this.transform.position = new Vector3(this.transform.position.x,tempPos.y, this.transform.position.z);
            restTime -= Time.deltaTime;
            yield return null;
        }

        currentLane = next;
        motostate = MotoState.Changed;
    }

    private void FixedUpdate()
    {
        // if (keyBoardInputMovement == 0.0f)
        // {
        //     backWheelController.useMotor = false;
        // }
        // else
        // {
        //     backWheelController.useMotor = true;
        //     JointMotor2D motor = new JointMotor2D() {motorSpeed = keyBoardInputMovement, maxMotorTorque = 100000};
        //     backWheelController.motor = motor;
        // }
        
    }
}
