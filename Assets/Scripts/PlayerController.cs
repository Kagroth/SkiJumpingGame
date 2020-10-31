using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform feetBone;

    [SerializeField]
    private Transform kneeBone;

    [SerializeField]
    private Transform pelvisBone;

    [SerializeField]
    private Transform skisBone;

    [SerializeField]
    private GameObject skiJumperBody;

    [SerializeField]
    private GameObject skiJumperRagdollPrefab;

    [HideInInspector]
    public GameObject skiJumperRagdoll;

    public delegate void SkiJumperStateChange();
    public SkiJumperStateChange skiJumperStartJumpHandler;
    public SkiJumperStateChange skiJumperJumpFinishedHandler;
    public SkiJumperStateChange skiJumperEndJumpHandler;
    
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

    private JumpResult jumpResultData;

    private void Awake()
    {
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

        //judges = new Judge[5];
        
        playerState.ChangeState(waitingForStart);
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPoint.position - new Vector3(0, 1.5f, 0); ;
        transform.rotation = Quaternion.Euler(0, 0, -50);
    }

    void Update()
    {
        if (InputManager.currentInputMode == InputManager.JUMP_FINISHED) {
            if (Input.anyKey) {
                skiJumperEndJumpHandler();
            }
        }

        if (InputManager.currentInputMode != InputManager.SKI_JUMPER) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetSkiJumper();
            return;
        }

        Vector3 cameraPos = Vector3.zero;

        if (playerState.CurrentState() != fallState)
        {
            cameraPos = new Vector3(transform.position.x, transform.position.y, -1);
        }
        else
        {
            cameraPos = new Vector3(skiJumperRagdoll.transform.position.x, skiJumperRagdoll.transform.position.y, -1);
        }

        Camera.main.transform.position = cameraPos;

        playerState.HandleUpdate();
    }

    private void FixedUpdate()
    {
        playerState.PhysicsUpdate();
    }

    public Transform GetIdealTakeOffPoint()
    {
        return idealTakeOffPoint;
    }

    public Transform GetFeetBone()
    {
        return feetBone;
    }

    public Transform GetKneeBone()
    {
        return kneeBone;
    }

    public Transform GetPelvisBone()
    {
        return pelvisBone;
    }

    public Transform GetSkisBone()
    {
        return skisBone;
    }

    public GameObject GetSkiJumperBody()
    {
        return skiJumperBody;
    }

    public GameObject GetSkiJumperRagdollPrefab()
    {
        return skiJumperRagdollPrefab;
    }

    public void ResetSkiJumper() {
        skiJumperBody.SetActive(true);

            if (skiJumperRagdoll.activeInHierarchy)
            {
                skiJumperRagdoll.SetActive(false);
                Destroy(skiJumperRagdoll);
                skiJumperRagdoll = null;
            }

            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            playerState.ChangeState(waitingForStart);
            transform.position = startingPoint.position - new Vector3(0, 1.5f, 0);
            transform.rotation = Quaternion.Euler(0, 0, -50);
            // landingText.text = "Lądowanie:";
            feetBone.localRotation = Quaternion.Euler(0, 0, 90);
            kneeBone.localRotation = Quaternion.Euler(0, 0, 0);
            pelvisBone.localRotation = Quaternion.Euler(0, 0, 0);
            skiJumperStartJumpHandler();
    }

    private float MeasureJumpDistance(Vector3 landedPosition)
    {
        BezierCurve bc = landingSlope.GetComponent<BezierCurve>();

        Vector3[] bezierPoints = bc.GetBezierPoints();

        Vector3 nearestBezierPoint = bezierPoints.OrderBy(bp => Vector3.Distance(landedPosition, bp)).First();

        float jumpDistance = 0;
        float distanceToAdd = 0;
        float decimalPart = 0;

        for (int index = 0; index < bezierPoints.Length - 1; index++)
        {
            jumpDistance += Vector3.Magnitude(bezierPoints[index + 1] - bezierPoints[index]);

            if (nearestBezierPoint == bezierPoints[index + 1])
            {
                if (landedPosition.x > nearestBezierPoint.x)
                {
                    jumpDistance += Vector3.Magnitude(landedPosition - nearestBezierPoint);
                }
                else
                {
                    jumpDistance -= Vector3.Magnitude(landedPosition - nearestBezierPoint);
                }

                break;
            }
        }

        jumpDistance = Mathf.Round(jumpDistance * 100) / 100;

        decimalPart = jumpDistance % 1;

        if (decimalPart >= 0.75f)
        {
            distanceToAdd = 1;
        }
        else if (decimalPart < 0.75f && decimalPart >= 0.25f)
        {
            distanceToAdd = 0.5f;
        }

        jumpDistance = Mathf.Floor(jumpDistance);
        jumpDistance += distanceToAdd;

        return jumpDistance;
    }

    public JumpResult GetJumpResultData() {
        return jumpResultData;
    }
    
    public float GetBestDistance() {
        return bestDistance;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (playerState.CurrentState() == fallState ||
            playerState.CurrentState() == landedState)
        {
            return;
        }

        if (other.gameObject.tag.Equals("LandingSlope"))
        {
            Judge[] judges = new Judge[5];
            Vector2 landedPosition = other.contacts[0].point;
            Vector3 landedPositionV3 = new Vector3(landedPosition.x, landedPosition.y, 0);
            float jumpDistance = 0;
            string landingType = "";
            HillData hd = other.gameObject.transform.parent.GetComponent<Hill>().hillData;
            float distancePoints = 0;
            bool landed = false;
            string landingTypeForJudgeNotes = "";
            float flightTiltChange = 0;
            float stylePoints = 0;
            float jumpPoints = 0;
            
            playerState.GetLandingData().SetLandingPoint(landedPositionV3);
            playerState.CurrentState().HandleLanding();

            jumpDistance = MeasureJumpDistance(landedPositionV3);

            if (jumpDistance > bestDistance && playerState.CurrentState() == landedState)
            {
                bestDistance = jumpDistance;
            }

            if (playerState.CurrentState() == landedState)
            {
                landingType = playerState.GetLandingData().GetLandingType();
            }
            else
            {
                landingType = "upadek";
            }

            distancePoints = CalculateDistancePoints(hd, jumpDistance);

            flightTiltChange = flyingState.GetFlightTiltChange();

            if (playerState.CurrentState() == landedState) {
                landed = true;
                landingTypeForJudgeNotes = playerState.GetLandingData().GetLandingType();
            }

            for(int index = 0; index < judges.Length; index++) {
                judges[index] = new Judge("POL");
                judges[index].CalculateJumpStylePoints(landed, landingTypeForJudgeNotes, flightTiltChange);
            }
            
            // odrzucenie najnizszej i najwyzszej noty
            judges = judges.OrderBy(judge => judge.GetJumpStylePoints()).ToArray();
            judges[0].Reject();
            judges[judges.Length - 1].Reject();

            stylePoints = judges[1].GetJumpStylePoints() + judges[2].GetJumpStylePoints() + judges[3].GetJumpStylePoints();
            
            System.Random rnd = new System.Random();
    
            judges = judges.OrderBy(judge => rnd.Next()).ToArray();

            jumpPoints = stylePoints + distancePoints;

            jumpPoints = Mathf.Clamp(jumpPoints, 0, jumpPoints); // punkty za skok nie moga byc mniejsze niz 0

            jumpResultData = new JumpResult(jumpDistance, judges, jumpPoints);
            
            InputManager.SetInputMode(InputManager.JUMP_FINISHED);
            skiJumperJumpFinishedHandler();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Inrun") && playerState.CurrentState() != flyingState &&
            other.gameObject.tag.Equals("Inrun") && playerState.CurrentState() != takeOffState)
        {
            playerState.ChangeState(fallState);
        }
    }

    private float CalculateDistancePoints(HillData hillData, float jumpDistance) {
        float basePoints = hillData.kPoint > 155 ? 120 : 60;
        float diff = jumpDistance - hillData.kPoint;
        float distancePoints = diff * hillData.pointPerMeter + basePoints;

        return distancePoints;
    }
}
