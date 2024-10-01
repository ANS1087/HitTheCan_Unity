using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public Transform ProjectilePrefab;
    public Transform DrawFrom;
    public Transform DrawTo;
    public Transform AngleCalc;
    public SlingShotString slingshotString;

    public int NrDrawIncrements = 10;
    public float shotForce = 35f;
    
    public LineRenderer trajectoryLine;
    private Transform currentProjectile;

    private bool isDrawing = false;

    public float scoreTimer = 0f;  
    public float scoreThresholdTime = 5f;  
    public int targetScore = 150;

    public float KeyMoveSpeed = 1f;

    public UIManager uiManager; 
    public InputManager InputM;
    public ScoreManager scoreM;

    bool GameIsOver= false;
    void Start()
    {
        uiManager.SetShotsLeft(); // Initialize UI with shots left
        trajectoryLine.positionCount = NrDrawIncrements;
    }

    void Update()
    {
        HandleGrabInput();
        
        UpdateTrajectoryPrediction();

        if (GameIsOver==false)
        {
            CheckForGameOver();
        }
        
        VHangle();
    }


    void HandleGrabInput()
    {
        if (InputM.grab && !isDrawing) // Grab
        {
            if (uiManager.shotsLeft > 0)
            {
                StartDrawing();
            }

        }

        if (isDrawing)
        {
            DrawSlingshot();
        }

        if (!InputM.grab && isDrawing) // Release
        {
            ReleaseAndShoot();
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        currentProjectile = Instantiate(ProjectilePrefab, DrawFrom.position, Quaternion.identity, transform);
        slingshotString.CenterPoint = currentProjectile;

    }

    void DrawSlingshot()
    {
        float dragDistance = Vector3.Distance(InputM.initialPosition, InputM.currentPosition);
        float maxDistance = Vector3.Distance(DrawFrom.position, DrawTo.position);
        float t = Mathf.Clamp01(dragDistance / maxDistance);
        currentProjectile.position = Vector3.Lerp(DrawFrom.position, DrawTo.position, t);
        uiManager.SetDistance(maxDistance * t);
    }
        

    void ReleaseAndShoot()
    {
        if (currentProjectile == null)
        {
            Debug.LogError("currentProjectile is null in ReleaseAndShoot");
            return;
        }

        isDrawing = false;
        currentProjectile.parent = null;
        
        Rigidbody projectileRigidBody = currentProjectile.GetComponent<Rigidbody>();
        if (projectileRigidBody != null)
        {
            projectileRigidBody.isKinematic = false;
            Vector3 direction = (DrawFrom.position - currentProjectile.position);
            projectileRigidBody.AddForce(direction * shotForce, ForceMode.Impulse);
        }

        uiManager.SetDistance(0f);

        slingshotString.CenterPoint = DrawFrom;

        DestroySelf destroySelfComponent = currentProjectile.GetComponent<DestroySelf>();
        if (destroySelfComponent != null)
        {
            destroySelfComponent.TriggerDestroy();
        }

        currentProjectile = null;

        // Decrement shots left and update the UI
        uiManager.shotsLeft--;
        uiManager.SetShotsLeft();
    }
    void UpdateTrajectoryPrediction()
    {
        if (isDrawing && currentProjectile != null)
        {
            Vector3 startPosition = currentProjectile.position;
            Vector3 initialVelocity = (DrawFrom.position - currentProjectile.position) * shotForce / currentProjectile.GetComponent<Rigidbody>().mass;
            Vector3[] trajectoryPoints = new Vector3[NrDrawIncrements];
            trajectoryPoints[0] = startPosition;

            for (int i = 1; i < NrDrawIncrements; i++)
            {
                float simulationTime = i * Time.fixedDeltaTime * 2; // Multiplying by 2 for better spread
                Vector3 displacement = initialVelocity * simulationTime + 0.5f * Mathf.Pow(simulationTime, 2) * Physics.gravity;
                Vector3 drawPoint = startPosition + displacement;
                trajectoryPoints[i] = drawPoint;
            }

            trajectoryLine.positionCount = trajectoryPoints.Length;
            trajectoryLine.SetPositions(trajectoryPoints);
            trajectoryLine.enabled = true;
        }
        else
        {
            trajectoryLine.enabled = false;
        }
    }

    public void CheckForGameOver()
    {
        if (uiManager.shotsLeft <= 0)
        {
            uiManager.Shots.gameObject.SetActive(false);
            uiManager.NoShots.gameObject.SetActive(true);
            GameOver();
        }
        else
        {
            // Check if the score is Target Score for the specified time
            if (scoreM.score == targetScore)
            {
                scoreTimer += Time.deltaTime;
                if (scoreTimer >= scoreThresholdTime)
                {
                    GameOver();
                }
            }
            else
            {
                scoreTimer = 0f;  // Reset timer if score changes
            }
        }
    }
    void GameOver()
    {
        Debug.Log("Game Over!");
        uiManager.SetShotsLeft(); // Update the UI to reflect no. of shots left
        uiManager.FinalScore.gameObject.SetActive(true);
        uiManager.GameOver.gameObject.SetActive(true);
        GameIsOver=true;
    }
    void VHangle()
    {
        AngleCalc.position = new Vector3(
            DrawFrom.position.x,
            DrawFrom.position.y,
            DrawTo.position.z
        );

        float VAngle = CalculateAngle(new Vector3(DrawFrom.position.x, DrawTo.position.y,DrawTo.position.z), DrawFrom.position, AngleCalc.position);
        float HAngle = CalculateAngle(new Vector3(DrawTo.position.x,DrawFrom.position.y,DrawTo.position.z), DrawFrom.position, AngleCalc.position);
        uiManager.SetVAngle(VAngle);
        uiManager.SetHAngle(HAngle);
    }
    float CalculateAngle(Vector3 a, Vector3 b, Vector3 c)
    {
        // Create vectors from b to a and b to c
        Vector3 vectorBA = a - b;
        Vector3 vectorBC = c - b;

        // Calculate the angle between these two vectors
        float angle = Vector3.Angle(vectorBA, vectorBC);
        return angle;
    }
}
