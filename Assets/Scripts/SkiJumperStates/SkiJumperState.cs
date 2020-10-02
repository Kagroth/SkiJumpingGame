using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiJumperState
{
    /* Possible states:
        - WaitingForStart
        - RunningUp
        - TakeOff
        - Flying
        - Landing
        - Landed
        - Fall
    */
    protected GameObject playerGameObject;
    protected StateMachine playerStateMachine;

    protected Rigidbody2D playerRb;
    protected PlayerController playerController;

    public SkiJumperState(GameObject playerGameObjectToSet, StateMachine playerStateMachineToSet) {
        playerGameObject = playerGameObjectToSet;
        playerStateMachine = playerStateMachineToSet;
        playerRb = playerGameObject.GetComponent<Rigidbody2D>();
        playerController = playerGameObject.GetComponent<PlayerController>();
    }

    public virtual void Init() {

    }

    public virtual void HandleUpdate() {

    }

    public virtual void PhysicsUpdate() {

    }

    public virtual void HandleLanding() {
        
    }
}
