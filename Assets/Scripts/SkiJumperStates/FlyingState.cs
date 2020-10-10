﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingState : SkiJumperState
{
    float xSpeed;

    float rotationSpeed;

    Vector3 bounds;
    Transform feetBone;
    Transform kneeBone;
    Transform pelvisBone;
    Transform skisBone;

    float bodyToSkisTilt; // nachylenie skoczka względem nart
 
    bool isAnimationEnter = true;

    public FlyingState(GameObject playerGameObjectToSet, StateMachine playerStateMachineToSet) : base(playerGameObjectToSet, playerStateMachineToSet)
    {

    }

    public override void Init()
    {
        Debug.Log("Powierzchnia: " + playerGameObject.GetComponent<Collider2D>().bounds.size);
        xSpeed = 2;
        rotationSpeed = 100;
        feetBone = playerController.GetFeetBone();
        kneeBone = playerController.GetKneeBone();
        pelvisBone = playerController.GetPelvisBone();
        skisBone = playerController.GetSkisBone();

        bounds = playerGameObject.GetComponent<Collider2D>().bounds.size;
        Debug.Log("Lecę");
        isAnimationEnter = true;
    }

    public override void HandleUpdate()
    {
        if (isAnimationEnter) {
            Debug.Log("Animacja runningUp -> flying");

            if (Mathf.Abs(playerController.GetFeetBone().localEulerAngles.z - 45) > 1f)
            {
                playerController.GetFeetBone().localRotation = Quaternion.Euler(playerController.GetFeetBone().localEulerAngles + Vector3.forward * Time.deltaTime);
                // playerController.GetFeetBone().Rotate(0, 0, Time.deltaTime * 100);
                Transform feetBone = playerController.GetFeetBone();
                feetBone.localRotation = Quaternion.RotateTowards(feetBone.localRotation, Quaternion.Euler(0, 0, 315), Time.deltaTime * 150);
            }

            if (playerController.GetKneeBone().localEulerAngles.z > 1f)
            {
                //playerController.GetKneeBone().Rotate(0, 0, -Time.deltaTime * 100);
                Transform kneeBone = playerController.GetKneeBone();
                kneeBone.localRotation = Quaternion.RotateTowards(kneeBone.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 100);
            }

            if (Mathf.Abs(playerController.GetPelvisBone().localEulerAngles.z - 330) > 1f)
            {
                // playerController.GetPelvisBone().Rotate(0, 0, -Time.deltaTime * 100);
                Transform pelvisBone = playerController.GetPelvisBone();
                pelvisBone.localRotation = Quaternion.RotateTowards(pelvisBone.localRotation, Quaternion.Euler(0, 0, 330), Time.deltaTime * 150);
            }

            if (Mathf.Abs(playerController.GetFeetBone().localEulerAngles.z - 45) <= 1f &&
                playerController.GetKneeBone().localEulerAngles.z <= 1f &&
                Mathf.Abs(playerController.GetPelvisBone().localEulerAngles.z - 330) <= 1f) {
                    isAnimationEnter = false;
            }
        }
        else 
        {
            Debug.Log("Flying, mozna kontrolowac skoczka");

            float axisY = Input.GetAxisRaw("Mouse Y");
            float axisX = Input.GetAxis("Horizontal");

            feetBone.Rotate(0, 0, axisY * Time.deltaTime * rotationSpeed);
            playerGameObject.transform.Rotate(0, 0, axisX * Time.deltaTime * rotationSpeed);

            bodyToSkisTilt = feetBone.localEulerAngles.z;

            if (bodyToSkisTilt > 45 && bodyToSkisTilt < 90)
            {
                playerGameObject.transform.Rotate(0, 0, Time.deltaTime * rotationSpeed * bodyToSkisTilt / 4);
            }
            else if (bodyToSkisTilt < 20 || bodyToSkisTilt > 300)
            {
                playerGameObject.transform.Rotate(0, 0, -Time.deltaTime * rotationSpeed * bodyToSkisTilt / 4);
            }
        }
        

        // przechylanie całego skoczka w przypadku zbytniego wychylenia lub odchylenia
        

        /* ograniczenie wychylenia skoczka - prototype
        if (feetBone.rotation.eulerAngles.z > 90) {
            if (axis > 0) {
                feetBone.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (axis < 0) {
                feetBone.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        */

        // ladowanie na 2 nogi
        if (Input.GetKeyDown("space"))
        {
            playerStateMachine.SetLandingType(LandingData.BOTH_LEGS);
            playerStateMachine.ChangeState(playerController.landingState);
            //playerAnimator.SetBool("landing", true);
        }
        // ladowanie telemarkiem
        else if (Input.GetKeyDown("down"))
        {
            playerStateMachine.SetLandingType(LandingData.TELEMARK);
            playerStateMachine.ChangeState(playerController.landingState);
            //playerAnimator.SetBool("landing", true);
        }
    }

    public override void PhysicsUpdate()
    {
        Vector3 liftForce = Forces.Lift(playerRb.velocity, bounds.y);

        Vector3 dragForce = Forces.Drag(playerRb.velocity, bounds.y);

        Vector3 velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, 0);
        Vector3 gravity = new Vector3(Physics2D.gravity.x, Physics2D.gravity.y, 0);

        Vector3 finalForce = liftForce + dragForce;

        playerRb.AddForce(finalForce, ForceMode2D.Force);

        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + liftForce, Color.green);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + dragForce, Color.red);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + velocity, Color.blue);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + gravity, Color.yellow);
    }

    public override void HandleLanding()
    {
        playerStateMachine.ChangeState(playerController.fallState);
    }
}
