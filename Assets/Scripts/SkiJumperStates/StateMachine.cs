using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private GameObject controllerGameObject;
    private SkiJumperState currentState;
    
    private LandingData landingType;

    public void ChangeState(SkiJumperState stateToSet) {
        currentState = stateToSet;
        currentState.Init();
    }

    public SkiJumperState CurrentState() {
        return currentState;
    }
    public LandingData GetLandingData() {
        return landingType;
    }

    public void SetLandingData(string type) {
        landingType = new LandingData(type);
    }

    public void HandleUpdate() {
        currentState.HandleUpdate();
    }

    public void PhysicsUpdate() {
        currentState.PhysicsUpdate();
    }
}
