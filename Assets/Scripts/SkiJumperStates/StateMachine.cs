using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private GameObject controllerGameObject;
    private SkiJumperState currentState;

    public void ChangeState(SkiJumperState stateToSet) {
        currentState = stateToSet;
        currentState.Init();
    }

    public void HandleUpdate() {
        currentState.HandleUpdate();
    }

    public void PhysicsUpdate() {
        currentState.PhysicsUpdate();
    }
}
