using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingState : SkiJumperState
{
    float xSpeed;
    Vector3 bounds;

    public FlyingState(GameObject playerGameObjectToSet, StateMachine playerStateMachineToSet) : base(playerGameObjectToSet, playerStateMachineToSet) {

    }

    public override void Init()
    {
        Debug.Log("Powierzchnia: " + playerGameObject.GetComponent<Collider2D>().bounds.size);
        xSpeed = 2;
        bounds = playerGameObject.GetComponent<SpriteRenderer>().bounds.size;
        Debug.Log("Lecę");
    }

    public override void PhysicsUpdate() {
        float liftCoefficient = 0.03f;
        float airDensity = 1.2f;
        float dragCoefficient = 0.028f;

        // float liftValue = playerRb.velocity.magnitude * playerRb.velocity.magnitude / 2 * liftCoefficient * airDensity * bounds.x * bounds.y;
        // Vector3 liftForce = Vector2.Perpendicular(playerRb.velocity.normalized) * liftValue;
        Vector3 liftForce = playerRb.velocity.magnitude * playerRb.velocity.magnitude * 0.5f * liftCoefficient * airDensity * Vector2.Perpendicular(playerRb.velocity.normalized) * bounds.y;
        
        /*Debug.Log("Lift value: " + liftForce.magnitude);
        Debug.Log("Lift force: " + liftForce);
        Debug.Log("Velocity: " + playerRb.velocity);
        Debug.Log("Velocity direction: " + playerRb.velocity.normalized);
        Debug.Log("Perpendicular to velocity direction: " + Vector2.Perpendicular(playerRb.velocity.normalized));
        Debug.Log("Velocity magnitude: " + playerRb.velocity.magnitude);
        */
        // Vector2 drag2D = playerRb.velocity.magnitude * playerRb.velocity.magnitude / 2 * dragCoefficient * airDensity * playerRb.velocity.normalized * bounds.x * bounds.y;
        
        // Vector2 drag2D = playerRb.velocity * playerRb.velocity / 2 * dragCoefficient * airDensity * playerRb.velocity.normalized * (-1);
        Vector2 drag2D = playerRb.velocity.magnitude * playerRb.velocity.magnitude * dragCoefficient * airDensity * 0.5f * -playerRb.velocity.normalized * bounds.y;
        Vector3 drag = new Vector3(drag2D.x, drag2D.y, 0);
        Vector3 velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, 0);
        Vector3 gravity = new Vector3(Physics2D.gravity.x, Physics2D.gravity.y, 0);
        Vector3 finalForce = liftForce + drag;

        playerRb.AddForce(finalForce, ForceMode2D.Force);        

        /*
        Debug.Log("Drag value: " + drag2D.magnitude);
        Debug.Log("Drag force: " + drag);
        Debug.Log("gravity: " + Physics2D.gravity);
        */
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + liftForce, Color.green);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + drag, Color.red);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + velocity, Color.blue);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + gravity, Color.yellow);
    }
}
