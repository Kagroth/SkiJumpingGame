﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingState : SkiJumperState
{
    float xSpeed;
    Vector3 bounds;

    public FlyingState(GameObject playerGameObjectToSet, StateMachine playerStateMachineToSet) : base(playerGameObjectToSet, playerStateMachineToSet) {

    }

    public override void Init()
    {
        Debug.Log("Powierzchnia: " + playerGameObject.GetComponent<Collider2D>().bounds.size);
        xSpeed = 2;
        bounds = playerGameObject.GetComponent<SpriteRenderer>().bounds.size;
        Debug.Log("Lecę");
    }

    public override void HandleUpdate()
    {
        // ladowanie na 2 nogi
        if (Input.GetKeyDown("space")) {
            playerStateMachine.SetLandingData(LandingData.BOTH_LEGS);
            playerStateMachine.ChangeState(playerController.landingState);
        }
        // ladowanie telemarkiem
        else if (Input.GetKeyDown("down")) {
            playerStateMachine.SetLandingData(LandingData.TELEMARK);
            playerStateMachine.ChangeState(playerController.landingState);
        }
    }

    public override void PhysicsUpdate() {
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

    public override void HandleLanding() {
        playerStateMachine.ChangeState(playerController.fallState);
    }
}
