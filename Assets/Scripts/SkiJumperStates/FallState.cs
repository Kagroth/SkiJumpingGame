using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FallState : SkiJumperState
{
    private bool enterFallState;

    public FallState(GameObject playerGameObjectToSet, StateMachine playerStateMachineToSet) : base(playerGameObjectToSet, playerStateMachineToSet) {

    }
    public override void Init() {
        Debug.Log("Upadek");
        enterFallState = true;
        playerController.GetSkiJumperBody().SetActive(false);
        playerController.skiJumperRagdoll = GameObject.Instantiate(playerController.GetSkiJumperRagdollPrefab(), playerGameObject.transform.position, playerGameObject.transform.rotation);
        playerController.skiJumperRagdoll.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        
        Rigidbody2D[] rb2d = playerController.skiJumperRagdoll.GetComponentsInChildren<Rigidbody2D>();

        foreach(Rigidbody2D rigidbody in rb2d) {
            rigidbody.velocity = playerRb.velocity;
        }
    }

    public override void HandleUpdate()
    {

    }

    public override void PhysicsUpdate() {
        /* if (enterFallState) {
            playerRb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
            playerRb.AddTorque(-10, ForceMode2D.Impulse);
            enterFallState = false;
        }
        */
        
        
    }
}
