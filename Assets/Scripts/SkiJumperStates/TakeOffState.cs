using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOffState : SkiJumperState
{
    public TakeOffState(GameObject playerGameObject, StateMachine playerStateMachine) : base(playerGameObject, playerStateMachine) {
        
    }

    public override void Init() {
        Debug.Log("Wybicie");
    }

    public override void HandleUpdate() {
    
    }

    public override void PhysicsUpdate() {
        playerRb.AddForce(Vector3.up * Random.Range(8, 12), ForceMode2D.Impulse);
        playerRb.AddForce(Vector3.right * Random.Range(5, 10), ForceMode2D.Impulse);
        playerStateMachine.ChangeState(playerController.flyingState);
    }
}
