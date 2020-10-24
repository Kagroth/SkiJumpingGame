using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingState : SkiJumperState
{
    float rotationSpeed;

    Vector3 bounds;
    Transform feetBone;
    Transform kneeBone;
    Transform pelvisBone;
    Transform skisBone;

    float bodyToSkisTilt; // nachylenie skoczka względem nart
    float skiJumperTilt; // nachylenie skoczka względem zeskoku
 
    bool isAnimationEnter = true;

    float flightTiltChange = 0;

    public FlyingState(GameObject playerGameObjectToSet, StateMachine playerStateMachineToSet) : base(playerGameObjectToSet, playerStateMachineToSet)
    {

    }

    public override void Init()
    {
        rotationSpeed = 100;
        flightTiltChange = 0;
        feetBone = playerController.GetFeetBone();
        kneeBone = playerController.GetKneeBone();
        pelvisBone = playerController.GetPelvisBone();
        skisBone = playerController.GetSkisBone();

        bounds = playerGameObject.GetComponent<Collider2D>().bounds.size;
        Debug.Log("Powierzchnia: " + bounds);
        Debug.Log("Lecę");
        isAnimationEnter = true;
}

    public override void HandleUpdate()
    {
        if (isAnimationEnter) {
            Debug.Log("Animacja runningUp -> flying");

            if (Mathf.Abs(playerController.GetFeetBone().localEulerAngles.z - 45) > 1f)
            {
                playerController.GetFeetBone().localRotation = Quaternion.Euler(playerController.GetFeetBone().localEulerAngles + Vector3.forward * Time.deltaTime);
                Transform feetBone = playerController.GetFeetBone();
                feetBone.localRotation = Quaternion.RotateTowards(feetBone.localRotation, Quaternion.Euler(0, 0, 315), Time.deltaTime * 150);
            }

            if (playerController.GetKneeBone().localEulerAngles.z > 1f)
            {
                Transform kneeBone = playerController.GetKneeBone();
                kneeBone.localRotation = Quaternion.RotateTowards(kneeBone.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 100);
            }

            if (Mathf.Abs(playerController.GetPelvisBone().localEulerAngles.z - 330) > 1f)
            {
                Transform pelvisBone = playerController.GetPelvisBone();
                pelvisBone.localRotation = Quaternion.RotateTowards(pelvisBone.localRotation, Quaternion.Euler(0, 0, 330), Time.deltaTime * 150);
            }

            if (Mathf.Abs(playerController.GetFeetBone().localEulerAngles.z - 45) <= 1f &&
                playerController.GetKneeBone().localEulerAngles.z <= 1f &&
                Mathf.Abs(playerController.GetPelvisBone().localEulerAngles.z - 330) <= 1f) {
                    isAnimationEnter = false;
            }
        }
        else 
        {
            Debug.Log("Flying, mozna kontrolowac skoczka");

            float axisY = Input.GetAxisRaw("Mouse Y");
            float axisX = Input.GetAxis("Horizontal");

            float mouseMoveRotation = axisY * Time.deltaTime * rotationSpeed;
            float keysDownRotation = -axisX * Time.deltaTime * rotationSpeed;

            if (mouseMoveRotation != 0 || keysDownRotation != 0) {
                flightTiltChange += Mathf.Abs(mouseMoveRotation);
                flightTiltChange += Mathf.Abs(keysDownRotation);
            }

            feetBone.Rotate(0, 0, mouseMoveRotation);
            feetBone.Rotate(0, 0, keysDownRotation);

            bodyToSkisTilt = feetBone.localEulerAngles.z;
            skiJumperTilt = playerGameObject.transform.localEulerAngles.z;

            if (bodyToSkisTilt > 60 && bodyToSkisTilt < 90)
            {
                float skiJumperTiltChange = Time.deltaTime * rotationSpeed;
                flightTiltChange += Mathf.Abs(skiJumperTiltChange);
                playerGameObject.transform.Rotate(0, 0, skiJumperTiltChange);
            }
            else if (bodyToSkisTilt < 20 || bodyToSkisTilt > 300)
            {
                float skiJumperTiltChange = -Time.deltaTime * rotationSpeed;
                flightTiltChange += Mathf.Abs(skiJumperTiltChange);
                playerGameObject.transform.Rotate(0, 0, skiJumperTiltChange);
            }
        }        

        /* ograniczenie wychylenia skoczka - prototype
        if (feetBone.rotation.eulerAngles.z > 90) {
            if (axis > 0) {
                feetBone.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (axis < 0) {
                feetBone.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        */

        // ladowanie na 2 nogi
        if (Input.GetKeyDown("space"))
        {
            playerStateMachine.SetLandingType(LandingData.BOTH_LEGS);
            playerStateMachine.ChangeState(playerController.landingState);
        }
        // ladowanie telemarkiem
        else if (Input.GetKeyDown("down"))
        {
            playerStateMachine.SetLandingType(LandingData.TELEMARK);
            playerStateMachine.ChangeState(playerController.landingState);
        }
    }

    public override void PhysicsUpdate()
    {
        float bodyToSkisCoeff = BodyToSkisCoefficientDecorator(CalculateTiltCoefficient(bodyToSkisTilt, 30));
        float skiJumperTiltCoeff = SkiJumperCoefficientDecorator(CalculateTiltCoefficient(skiJumperTilt, 0));

        Vector3 liftForce = Forces.Lift(playerRb.velocity, bounds.y) * bodyToSkisCoeff * skiJumperTiltCoeff;
        Vector3 dragForce = Forces.Drag(playerRb.velocity, bounds.y);
        Vector3 velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, 0);
        Vector3 gravity = new Vector3(Physics2D.gravity.x, Physics2D.gravity.y, 0);

        Vector3 finalForce = liftForce + dragForce;

        playerRb.AddForce(finalForce, ForceMode2D.Force);

        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + liftForce, Color.green);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + dragForce, Color.red);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + velocity, Color.blue);
        Debug.DrawLine(playerGameObject.transform.position, playerGameObject.transform.position + gravity, Color.yellow);
    }

    public override void HandleLanding()
    {
        playerStateMachine.ChangeState(playerController.fallState);
    }

    private float CalculateTiltCoefficient(float tilt, float idealAngle) {
        float coeff = 0;

        if (tilt > 180 + idealAngle) {
            coeff = 360 - tilt + idealAngle;
        }
        else {
            coeff = tilt - idealAngle;
        }

        coeff = Mathf.Abs(coeff); 

        return coeff;
    }

    private float BodyToSkisCoefficientDecorator(float coefficient) {
        float coeff = (1 - coefficient / 180) - 2 * (coefficient / 180); 
        coeff = Mathf.Clamp(coeff, 0, 1);

        return coeff;
    }

    private float SkiJumperCoefficientDecorator(float coefficient) {
        float coeff = (1 - coefficient / 180);       
        coeff = Mathf.Clamp(coeff, 0, 1);

        return coeff;
    }

    public float GetFlightTiltChange() {
        return flightTiltChange;
    }
}
