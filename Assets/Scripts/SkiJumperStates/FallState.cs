using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : SkiJumperState
{
    private bool enterFallState;

    public FallState(GameObject playerGameObjectToSet, StateMachine playerStateMachineToSet) : base(playerGameObjectToSet, playerStateMachineToSet) {

    }
    public override void Init() {
        enterFallState = true;
    }

    public override void PhysicsUpdate() {
        if (enterFallState) {
            playerRb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
            playerRb.angularVelocity = 0;
            enterFallState = false;
        }
    }
}
