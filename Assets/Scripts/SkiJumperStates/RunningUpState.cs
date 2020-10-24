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
        Vector3 direction = new Vector3(0, -1, 0);
        playerRb.AddForce(direction * 4, ForceMode2D.Impulse);
    }

    public override void HandleUpdate() {
        if (Mathf.Abs(playerController.GetKneeBone().localEulerAngles.z - 60 ) > 1f) {
            Transform kneeBone = playerController.GetKneeBone();
            kneeBone.localRotation = Quaternion.RotateTowards(kneeBone.localRotation, Quaternion.Euler(0, 0, 300), -Time.deltaTime * 100);
        }

        if (Mathf.Abs(playerController.GetPelvisBone().localEulerAngles.z - 210) > 1f) {
            Transform pelvisBone = playerController.GetPelvisBone();
            pelvisBone.localRotation = Quaternion.RotateTowards(pelvisBone.localRotation, Quaternion.Euler(0, 0, 210), Time.deltaTime * 100);
        }

        if (Input.GetKeyDown("up")) {
            playerStateMachine.ChangeState(playerController.takeOffState);
        }
    }

    public override void PhysicsUpdate() {
        Vector3 drag = -playerRb.velocity.normalized * Vector3.one * 2; 
        Vector3 velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, 0);
        Vector3 gravity = new Vector3(Physics2D.gravity.x, Physics2D.gravity.y, 0);

        playerRb.AddForce(Vector2.right * 4, ForceMode2D.Force);        
        playerRb.AddForce(drag);

        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + velocity, Color.blue);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + gravity, Color.yellow);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + drag, Color.red);
    }
}
