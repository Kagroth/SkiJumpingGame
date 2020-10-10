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
        Vector3 takeOffPos = playerGameObject.transform.position;
        Vector3 idealTakeOffPos = playerGameObject.GetComponent<PlayerController>().GetIdealTakeOffPoint().position;

        float difference = Mathf.Abs(takeOffPos.x - idealTakeOffPos.x);
        
        takeOffStr = maxTakeOffStrength - difference;
        takeOffDir = maxTakeOffDirection - difference;

        takeOffStr = Mathf.Clamp(takeOffStr, minTakeOffStrength, maxTakeOffStrength);
        takeOffDir = Mathf.Clamp(takeOffDir, minTakeOffDirection, maxTakeOffDirection);

        Debug.Log("Róznica: " + difference);
        Debug.Log("Sila wybicia: " + takeOffStr);
        Debug.Log("Kierunek wybicia: " + takeOffDir);
            
        Vector3 takeOffForce = new Vector3(1, 1, 0).normalized * takeOffStr;
        
        playerRb.AddForce(takeOffForce, ForceMode2D.Impulse);

        playerStateMachine.ChangeState(playerController.flyingState);

    }

    public override void HandleUpdate() {
      
    }

    public override void PhysicsUpdate() {

    }
}
