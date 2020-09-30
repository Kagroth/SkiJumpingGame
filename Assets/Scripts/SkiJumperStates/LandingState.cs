using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : SkiJumperState
{
    public LandingState(GameObject playerGameObject, StateMachine playerStateMachine) : base(playerGameObject, playerStateMachine) {

    }

    public override void Init() {
        Debug.Log("Laduje");
    }

    public override void HandleUpdate() {

    }

    public override void PhysicsUpdate() {

    }
}
