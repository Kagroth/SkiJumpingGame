using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public Transform rotationCenter;

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

    public UnityEngine.UI.Text bestScoreText;
    public UnityEngine.UI.Text lastScoreText;
    public UnityEngine.UI.Text landingText;

    public UnityEngine.UI.Text skiJumperEulerAngles;
    public UnityEngine.UI.Text skiJumperLocalEulerAngles;
    public UnityEngine.UI.Text feetBoneEulerAngles;
    public UnityEngine.UI.Text feetBoneLocalEulerAngles;

    public UnityEngine.UI.Text kneeBoneEulerAngles;
    public UnityEngine.UI.Text kneeBoneLocalEulerAngles;

    public UnityEngine.UI.Text pelvisBoneEulerAngles;
    public UnityEngine.UI.Text pelvisBoneLocalEulerAngles;
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

    private bool rotDir = true;
    private Quaternion target = Quaternion.Euler(0, 0, -170);

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
        /*
        Vector3 vectorToTarget = Vector3.right;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * vectorToTarget;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 10);
        
        Debug.Log("Feet bone eulerAngles: " + feetBone.rotation.eulerAngles);
        Debug.Log("Feet bone localEulerAngles: " + feetBone.localEulerAngles);
        
        Debug.Log("Ski Jumper eulerAngles: " + transform.rotation.eulerAngles);
        Debug.Log("Ski Jumper localEulerAngles: " + transform.localEulerAngles);*/

        /* skiJumperEulerAngles.text = "SJEA: " + transform.rotation.eulerAngles.ToString();
        skiJumperLocalEulerAngles.text = "SJLEA: " + transform.localEulerAngles.ToString();
        feetBoneEulerAngles.text = "FBEA: " + feetBone.rotation.eulerAngles.ToString();
        feetBoneLocalEulerAngles.text = "FBLEA: " + feetBone.localEulerAngles.ToString();
        kneeBoneEulerAngles.text = "KBEA: " + kneeBone.rotation.eulerAngles.ToString();
        kneeBoneLocalEulerAngles.text = "KBLEA: " + kneeBone.localEulerAngles.ToString();
        pelvisBoneEulerAngles.text = "PBEA: " + pelvisBone.rotation.eulerAngles.ToString();
        pelvisBoneLocalEulerAngles.text = "PBLEA: " + pelvisBone.localEulerAngles.ToString(); */

        /*
        float angle = Vector3.Angle(feetBone.up, transform.up);

        float axisY = Input.GetAxisRaw("Vertical");
        float axisX = Input.GetAxis("Horizontal");

        feetBone.Rotate(0, 0, -axisX * Time.deltaTime * 100);
        // transform.Rotate(0, 0, axisY * Time.deltaTime * 100);
        transform.RotateAround(rotationCenter.position, Vector3.forward, Time.deltaTime * axisY * 100);

        if (Input.GetKey(KeyCode.Z)) {
            if (Mathf.Abs(kneeBone.localEulerAngles.z - 60 ) > 1f) {
                // kneeBone.Rotate(0, 0, Time.deltaTime * 20);
                kneeBone.localRotation = Quaternion.RotateTowards(kneeBone.localRotation, Quaternion.Euler(0, 0, 300), -Time.deltaTime * 10);
            }
        }

        if (Input.GetKey(KeyCode.X)) {
            if (Mathf.Abs(pelvisBone.localEulerAngles.z - 210) > 1f) {
                pelvisBone.Rotate(0, 0, -Time.deltaTime * 20);
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.R))
        {
            skiJumperBody.SetActive(true);

            if (skiJumperRagdoll != null)
            {
                Destroy(skiJumperRagdoll);
            }

            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            playerState.ChangeState(waitingForStart);
            transform.position = startingPoint.position - new Vector3(0, 1.5f, 0);
            transform.rotation = Quaternion.Euler(0, 0, -50);
            landingText.text = "Lądowanie:";
            feetBone.localRotation = Quaternion.Euler(0, 0, 90);
            kneeBone.localRotation = Quaternion.Euler(0, 0, 0);
            pelvisBone.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        // Camera.main.transform.position = new Vector3(rotationCenter.position.x, rotationCenter.position.y, -1);
        // Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        Vector3 cameraPos = Vector3.zero;

        if (playerState.CurrentState() != fallState)
        {
            cameraPos = new Vector3(transform.position.x, transform.position.y, -1);
        }
        else
        {
            Debug.Log("UPADEK - POZYCJA RAGDOLLA");
            cameraPos = new Vector3(skiJumperRagdoll.transform.position.x, skiJumperRagdoll.transform.position.y, -1);
            Debug.Log("Pozycja kamery po upadku: " + cameraPos);
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

    private float MeasureJumpDistance(Vector3 landedPosition)
    {
        BezierCurve bc = landingSlope.GetComponent<BezierCurve>();

        Vector3[] bezierPoints = bc.GetBezierPoints();

        Vector3 nearestBezierPoint = bezierPoints.OrderBy(bp => Vector3.Distance(landedPosition, bp)).First();

        float jumpDistance = 0;

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

        float distanceToAdd = 0f;
        float decimalPart = jumpDistance % 1;

        if (decimalPart >= 0.75f)
        {
            distanceToAdd = 1;
        }
        else if (decimalPart < 0.75f && decimalPart >= 0.25f)
        {
            distanceToAdd = 0.5f;
        }

        Debug.Log("Odleglosc skoku: " + jumpDistance);

        jumpDistance = Mathf.Floor(jumpDistance);
        jumpDistance += distanceToAdd;

        Debug.Log("Odleglosc skoku: " + jumpDistance);

        return jumpDistance;
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
            Vector2 landedPosition = other.contacts[0].point;
            Vector3 landedPositionV3 = new Vector3(landedPosition.x, landedPosition.y, 0);

            playerState.GetLandingData().SetLandingPoint(landedPositionV3);
            playerState.CurrentState().HandleLanding();

            float jumpDistance = MeasureJumpDistance(landedPositionV3);
            string landingType = "";

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

            lastScoreText.text = "Ostatni wynik: " + jumpDistance.ToString();
            bestScoreText.text = "Najlepszy wynik: " + bestDistance.ToString();
            landingText.text = "Lądowanie: " + landingType.ToString();

            /*
                noty za styl
                lot - 5
                lądowanie - 5
                upadek - 7
                nieprawidlowa pozycja na odjezdzie - 3 -> moze byc zalezny od momentu podejscia do lądowania
            */

            HillData hd = other.gameObject.transform.parent.GetComponent<Hill>().hillData;
            float basePoints = hd.kPoint > 155 ? 120 : 60;
            float diff = jumpDistance - hd.kPoint;
            float distancePoints = diff * hd.pointPerMeter + basePoints;

            Debug.Log("Punkty za odległość: " + distancePoints);

            float stylePoints = 0;
            float landingPoints = 0;

            if (landingType == LandingData.TELEMARK)
            {
                landingPoints = 5;
            }
            else if (landingType == LandingData.BOTH_LEGS)
            {
                landingPoints = 3;
            }
            else
            {
                landingPoints = 0;
            }

            Debug.Log("Punkty za lądowanie: " + landingPoints);

            float flightTiltChange = flyingState.GetFlightTiltChange();
            float flightPoints = CalculateFlightStylePoints(flightTiltChange);

            stylePoints = flightPoints + landingPoints;

            if (playerState.CurrentState() == landedState) {
                stylePoints += 7;
            }

            stylePoints += 3; // tymczasowo, patrz komentarz wyżej

            Debug.Log("Całkowita zmiana kątów w trakcie lotu: " + flightTiltChange);
            Debug.Log("Punkty za lot: " + flightPoints);
            Debug.Log("<color=green>Końcowa nota za skok: " + stylePoints + "</color>");
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

    private float CalculateFlightStylePoints(float flightTiltChange)
    {
        float flightPoints = 0;

        if (flightTiltChange < 15)
        {
            flightPoints = 5;
        }
        else if (flightTiltChange >= 15 && flightTiltChange < 30)
        {
            flightPoints = 4.5f;
        }
        else if (flightTiltChange >= 30 && flightTiltChange < 60)
        {
            flightPoints = 4;
        }
        else if (flightTiltChange >= 60 && flightTiltChange < 90)
        {
            flightPoints = 3.5f;
        }
        else if (flightTiltChange >= 90 && flightTiltChange < 120)
        {
            flightPoints = 3;
        }
        else if (flightTiltChange >= 120 && flightTiltChange < 150)
        {
            flightPoints = 2.5f;
        }
        else if (flightTiltChange >= 150 && flightTiltChange < 180)
        {
            flightPoints = 2;
        }
        else if (flightTiltChange >= 180 && flightTiltChange < 210)
        {
            flightPoints = 1.5f;
        }
        else if (flightTiltChange >= 210 && flightTiltChange < 240)
        {
            flightPoints = 1;
        }
        else if (flightTiltChange >= 240 && flightTiltChange < 360)
        {
            flightPoints = 0.5f;
        }
        else
        {
            flightPoints = 0;
        }

        return flightPoints;
    }
}
