using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedState : SkiJumperState
{
    bool brake;
    public LandedState(GameObject playerGameObject, StateMachine playerStateMachine) : base(playerGameObject, playerStateMachine) {

    }

    public override void Init() {
        brake = true;
        Debug.Log("I wyladowal");
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
            playerRb.AddForce(Forces.Drag(playerRb.velocity, playerGameObject.GetComponent<SpriteRenderer>().bounds.size.y) + Vector3.left);
        }
        else {
            playerRb.velocity = Vector3.zero;
        }
    }
}
