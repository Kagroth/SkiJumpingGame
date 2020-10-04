using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private GameObject controllerGameObject;
    private SkiJumperState currentState;
    
    private LandingData landingType;

    public StateMachine() {
        landingType = new LandingData();
    }

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

    public void SetLandingType(string type) {
        landingType.SetLandingType(type);
    }

    public void HandleUpdate() {
        currentState.HandleUpdate();
    }

    public void PhysicsUpdate() {
        currentState.PhysicsUpdate();
    }
}
