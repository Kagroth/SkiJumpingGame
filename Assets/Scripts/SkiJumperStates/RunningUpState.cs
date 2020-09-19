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
        playerRb.AddForce(Vector3.right * 4, ForceMode2D.Impulse);
    }

    public override void HandleUpdate() {
        if (Input.GetKeyDown("up")) {
            playerStateMachine.ChangeState(playerController.takeOffState);
        }
    }

    public override void PhysicsUpdate() {
        playerRb.AddForce(Vector2.right * 4, ForceMode2D.Force);
        Vector3 drag = -playerRb.velocity.normalized * Vector3.one * 2; 
        Vector3 velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, 0);
        Vector3 gravity = new Vector3(Physics2D.gravity.x, Physics2D.gravity.y, 0);
        playerRb.AddForce(drag);


        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + velocity, Color.blue);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + gravity, Color.yellow);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + drag, Color.red);
    }
}
