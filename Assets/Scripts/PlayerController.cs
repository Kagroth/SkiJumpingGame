using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public UnityEngine.UI.Text bestScoreText;
    public UnityEngine.UI.Text lastScoreText;
    public UnityEngine.UI.Text landingText;

    private float bestDistance = 0;
    private Rigidbody2D playerRb;
    Transform startingPoint;
    Transform idealTakeOffPoint;
    GameObject landingSlope;
    private StateMachine playerState;

    // All possible states of player
    public WaitingForStartState waitingForStart;
    public RunningUpState runningUpState;
    public TakeOffState takeOffState;
    public FlyingState flyingState;

    public LandingState landingState;

    public LandedState landedState;

    public FallState fallState;

    private void Awake() {
        playerRb = GetComponent<Rigidbody2D>();
        startingPoint = GameObject.FindGameObjectWithTag("StartingPoint").GetComponent<Transform>();
        idealTakeOffPoint = GameObject.FindGameObjectWithTag("IdealTakeOffPoint").GetComponent<Transform>();
        landingSlope = GameObject.FindGameObjectWithTag("LandingSlope");
        
        playerState = new StateMachine();
        waitingForStart = new WaitingForStartState(this.gameObject, playerState);
        runningUpState = new RunningUpState(this.gameObject, playerState);
        takeOffState = new TakeOffState(this.gameObject, playerState);
        flyingState = new FlyingState(this.gameObject, playerState);
        landingState = new LandingState(this.gameObject, playerState);
        landedState = new LandedState(this.gameObject, playerState);
        fallState = new FallState(this.gameObject, playerState);

        playerState.ChangeState(waitingForStart);
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPoint.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            playerState.ChangeState(waitingForStart);
            transform.position = startingPoint.position;
            transform.rotation = Quaternion.identity;
            landingText.text = "Lądowanie:";
            return;
        }

        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        playerState.HandleUpdate();
    }

    private void FixedUpdate() {
        playerState.PhysicsUpdate();
    }

    public Transform GetIdealTakeOffPoint() {
        return idealTakeOffPoint;
    }

    private float MeasureJumpDistance(Vector3 landedPosition) {
        BezierCurve bc = landingSlope.GetComponent<BezierCurve>();

        Vector3[] bezierPoints = bc.GetBezierPoints();

        Vector3 nearestBezierPoint = bezierPoints.OrderBy(bp => Vector3.Distance(landedPosition, bp)).First();
        
        float jumpDistance = 0;

        for (int index = 0; index < bezierPoints.Length - 1; index++) {
            jumpDistance += Vector3.Magnitude(bezierPoints[index + 1] - bezierPoints[index]);

            if (nearestBezierPoint == bezierPoints[index + 1])  {
                if (landedPosition.x > nearestBezierPoint.x) {
                    jumpDistance += Vector3.Magnitude(landedPosition - nearestBezierPoint);
                }
                else {
                    jumpDistance -= Vector3.Magnitude(landedPosition - nearestBezierPoint);
                }

                break;
            }
        }

        jumpDistance = Mathf.Round(jumpDistance * 100) / 100; 
            
        float distanceToAdd = 0f;
        float decimalPart = jumpDistance % 1;
            
        if (decimalPart >= 0.75f) {
            distanceToAdd = 1;
        }
        else if (decimalPart < 0.75f && decimalPart >= 0.25f) {
            distanceToAdd = 0.5f;
        }

        Debug.Log("Odleglosc skoku: " + jumpDistance);

        jumpDistance = Mathf.Floor(jumpDistance);
        jumpDistance += distanceToAdd;
            
        Debug.Log("Odleglosc skoku: " + jumpDistance);

        return jumpDistance;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (playerState.CurrentState() == fallState ||
            playerState.CurrentState() == landedState) {
            return;
        }

        if (other.gameObject.tag.Equals("LandingSlope")) {            
            Vector2 landedPosition = other.contacts[0].point;
            Vector3 landedPositionV3 = new Vector3(landedPosition.x, landedPosition.y, 0);
            
            playerState.GetLandingData().SetLandingPoint(landedPositionV3);
            playerState.CurrentState().HandleLanding();

            float jumpDistance = MeasureJumpDistance(landedPositionV3);
            string landingType = "";
            
            if (jumpDistance > bestDistance && playerState.CurrentState() == landedState) {
                bestDistance = jumpDistance;                
            }

            if (playerState.CurrentState() == landedState) {
                landingType = playerState.GetLandingData().GetLandingType();
            }
            else {
                landingType = "upadek";
            }

            lastScoreText.text = "Ostatni wynik: " + jumpDistance.ToString();
            bestScoreText.text = "Najlepszy wynik: " + bestDistance.ToString();
            landingText.text = "Lądowanie: " + landingType.ToString();
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag.Equals("Inrun") && playerState.CurrentState() != flyingState) {
            playerState.ChangeState(fallState);
        }
    }
}
