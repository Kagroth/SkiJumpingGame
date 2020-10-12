using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : SkiJumperState
{
    private bool enterFallState;

    public FallState(GameObject playerGameObjectToSet, StateMachine playerStateMachineToSet) : base(playerGameObjectToSet, playerStateMachineToSet) {

    }
    public override void Init() {
        Debug.Log("Upadek");
        enterFallState = true;
        playerController.GetSkiJumperBody().SetActive(false);
        playerController.skiJumperRagdoll = GameObject.Instantiate(playerController.GetSkiJumperRagdollPrefab(), playerGameObject.transform.position, playerGameObject.transform.rotation, playerGameObject.transform);
        
        GameObject ragdoll = playerController.skiJumperRagdoll;

        Rigidbody2D[] rb2d = ragdoll.GetComponentsInChildren<Rigidbody2D>();

        foreach(Rigidbody2D rb in rb2d) {
            rb.velocity = playerRb.velocity;
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
