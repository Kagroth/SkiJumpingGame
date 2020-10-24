using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedState : SkiJumperState
{
    bool brake;

    Vector3 bounds;

    public LandedState(GameObject playerGameObject, StateMachine playerStateMachine) : base(playerGameObject, playerStateMachine) {
        brake = true;
        bounds = playerGameObject.GetComponent<Collider2D>().bounds.size;
    }

    public override void Init() {
        Debug.Log("Skok ustany - " + playerStateMachine.GetLandingData().GetLandingType());
    }

    public override void HandleUpdate() {
        if (playerRb.velocity.magnitude > 0.1f) {
            brake = true;
        }
        else {
            brake = false;
        }
    }

    public override void PhysicsUpdate() {
        if (brake) {
            playerRb.AddForce(Forces.Drag(playerRb.velocity, bounds.y) + Vector3.left);
        }
        else {
            playerRb.velocity = Vector3.zero;
        }
    }
}
