using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningUpState : SkiJumperState
{
    public RunningUpState(GameObject playerGameObject, StateMachine stateMachine) : base(playerGameObject, stateMachine) {

    }
    public override void Init()
    {
        base.Init();
        Debug.Log("Jade");
        playerRb.isKinematic = false;
        Vector3 direction = new Vector3(1, -1, 0);
        playerRb.AddForce(direction * 2, ForceMode2D.Impulse);
    }

    public override void HandleUpdate() {
        if (Input.GetKeyDown("up")) {
            playerStateMachine.ChangeState(playerController.takeOffState);
        }
    }

    public override void PhysicsUpdate() {
        playerRb.AddForce(Vector2.right * 10 * playerRb.drag, ForceMode2D.Force);
    }
}
