using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOffState : SkiJumperState
{
    private float maxTakeOffStrength = 6;
    private float maxTakeOffDirection = 4;

    private float minTakeOffStrength = 1;
    private float minTakeOffDirection = 1;

    private float takeOffStr = 0;
    private float takeOffDir = 0;

    public TakeOffState(GameObject playerGameObject, StateMachine playerStateMachine) : base(playerGameObject, playerStateMachine) {
        
    }

    public override void Init() {
        Debug.Log("Wybicie");
        playerAnimator.SetBool("takeOff", true);
        Vector3 takeOffPos = playerGameObject.transform.position;
        Vector3 idealTakeOffPos = playerGameObject.GetComponent<PlayerController>().GetIdealTakeOffPoint().position;

        // float difference = Vector3.Distance(takeOffPos, idealTakeOffPos);
        float difference = Mathf.Abs(takeOffPos.x - idealTakeOffPos.x);
        
        takeOffStr = maxTakeOffStrength - difference;
        takeOffDir = maxTakeOffDirection - difference;

        takeOffStr = Mathf.Clamp(takeOffStr, minTakeOffStrength, maxTakeOffStrength);
        takeOffDir = Mathf.Clamp(takeOffDir, minTakeOffDirection, maxTakeOffDirection);

        Debug.Log("Róznica: " + difference);
        Debug.Log("Sila wybicia: " + takeOffStr);
        Debug.Log("Kierunek wybicia: " + takeOffDir);
    }

    public override void HandleUpdate() {
    
    }

    public override void PhysicsUpdate() {
        // Vector3 takeOffForce = Vector3.up * takeOffStr + Vector3.right * takeOffDir;
        Vector3 takeOffForce = new Vector3(1, 1, 0).normalized * takeOffStr;
        playerRb.AddForce(takeOffForce, ForceMode2D.Impulse);
        playerStateMachine.ChangeState(playerController.flyingState);
    }
}
