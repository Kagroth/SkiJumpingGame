using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : SkiJumperState
{
    Vector3 bounds;

    public LandingState(GameObject playerGameObject, StateMachine playerStateMachine) : base(playerGameObject, playerStateMachine)
    {
        bounds = playerGameObject.GetComponent<Collider2D>().bounds.size;
    }

    public override void Init()
    {
        Debug.Log("Podchodze do ladowania");

        RaycastHit2D[] hits2D = Physics2D.RaycastAll(playerGameObject.transform.position, Vector2.down, 1000);
        float hitDistance = 0;

        foreach (RaycastHit2D hit in hits2D)
        {
            if (hit.collider.gameObject.tag.Equals("LandingSlope"))
            {
                hitDistance = hit.distance;
                break;
            }
        }

        playerStateMachine.GetLandingData().SetDistanceToLandingSlope(hitDistance);
        Debug.Log("Wysokość podejscia do ladowania: " + hitDistance);
    }

    public override void HandleUpdate()
    {
        if (Mathf.Abs(playerController.GetPelvisBone().localEulerAngles.z - 340) > 1f)
        {
            Transform pelvisBone = playerController.GetPelvisBone();
            pelvisBone.localRotation = Quaternion.RotateTowards(pelvisBone.localRotation, Quaternion.Euler(0, 0, 340), Time.deltaTime * 100);
        }

        if (Mathf.Abs(playerController.GetFeetBone().localEulerAngles.z - 90) > 1f)
        {
            Transform feetBone = playerController.GetFeetBone();
            feetBone.localRotation = Quaternion.RotateTowards(feetBone.localRotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * 100);
        }

        if (playerGameObject.transform.localEulerAngles.z > 1f)
        {
            playerGameObject.transform.localRotation = Quaternion.RotateTowards(playerGameObject.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 100);
        }
    }

    public override void PhysicsUpdate()
    {
        playerRb.AddForce(Forces.Drag(playerRb.velocity, bounds.y), ForceMode2D.Force);
    }

    public override void HandleLanding()
    {
        LandingData landingData = playerStateMachine.GetLandingData();

        float minHeight = 0;
        float minAngle = 0;

        if (landingData.GetLandingType().Equals(LandingData.BOTH_LEGS))
        {
            minHeight = LandingData.BOTH_LEGS_MIN_HEIGTH;
            minAngle = LandingData.BOTH_LEGS_MIN_ANGLE;
        }
        else if (landingData.GetLandingType().Equals(LandingData.TELEMARK))
        {
            minHeight = LandingData.TELEMARK_MIN_HEIGTH;
            minAngle = LandingData.TELEMARK_MIN_ANGLE;
        }

        /*
            TO DO
            OBLICZENIA DOTYCZACE KATA NACHYLENIA
        */

        SkiJumperState landingResultState = null;

        if (landingData.GetDistance() > minHeight)
        {
            landingResultState = playerController.landedState;
        }
        else
        {
            landingResultState = playerController.fallState;
        }

        playerStateMachine.ChangeState(landingResultState);
    }
}
