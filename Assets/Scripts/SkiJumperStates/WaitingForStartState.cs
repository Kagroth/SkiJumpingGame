using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForStartState : SkiJumperState
{
    public WaitingForStartState(GameObject playerGameObject, StateMachine stateMachine) : base(playerGameObject, stateMachine) {
    
    }
    public override void Init()
    {
        base.Init();
        Debug.Log("Oczekuje na start");
        playerRb.isKinematic = true;
    }

    public override void HandleUpdate() {
        if (Input.GetKeyDown("space")) {
            Debug.Log("Rozpoczynam najazd");
            playerStateMachine.ChangeState(playerController.runningUpState);
        }
    }
}
