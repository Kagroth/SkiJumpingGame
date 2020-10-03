using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : SkiJumperState
{
    public LandingState(GameObject playerGameObject, StateMachine playerStateMachine) : base(playerGameObject, playerStateMachine) {

    }

    public override void Init() {
        Debug.Log("Podchodze do ladowania");

        RaycastHit2D[] hits2D = Physics2D.RaycastAll(playerGameObject.transform.position, Vector2.down, 1000);
        float hitDistance = 0;

        foreach (RaycastHit2D hit in hits2D) {
            if (hit.collider.gameObject.tag.Equals("LandingSlope")) {
                hitDistance = hit.distance;
                break;
            }
        }

        playerStateMachine.GetLandingData().SetDistanceToLandingSlope(hitDistance);
        Debug.Log("Wysokość podejscia do ladowania: " + hitDistance);
    }

    public override void HandleUpdate() {

    }

    public override void PhysicsUpdate() {

    }
}
