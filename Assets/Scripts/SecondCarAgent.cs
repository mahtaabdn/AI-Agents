using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static CarController;
using System.Linq;

public class SecondCarAgent : BaseAgent
{
    public Vector3 originalPosition;

    private BehaviorParameters behaviorParameters;

    private CarController carController;

    private CarSpots carSpots;

    private CarGoal goal;

    private Vector3 relativePos;

    private float xPos;

    public float yRotation;

    private bool isParked = false;

    //private bool isCalledOnce = false;

    [SerializeField]
    private GameObject otherAgent;


    // private float[] dist = new float[];

    // private IEnumerable<CarGoal> allGoals;

    void Awake()
    {

        originalPosition = transform.localPosition;
        behaviorParameters = transform.GetComponentInChildren<BehaviorParameters>();
        carController = transform.GetComponentInChildren<CarController>();
        carSpots = transform.parent.GetComponentInChildren<CarSpots>();
    }

    public override void OnEpisodeBegin()
    {


    }

    public void ResetParkingLotArea()
    {

        xPos = Random.Range(-3, 3);
        originalPosition = new Vector3(xPos, originalPosition.y, originalPosition.z);

        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        carController.CarRigidbody.velocity = Vector3.zero;
        
        // reset which cars show or not show
        if (carSpots.onceSet == true)
        {
            carSpots.DestroyGoals();
        }
        
        isParked = false;
        //otherAgent.GetComponent<CarAgent>().ResetParkingLotArea();
        

    }

    
    public override void CollectObservations(VectorSensor sensor)
    {
        

        sensor.AddObservation(transform.localPosition);

        
    }
    
    public override void OnActionReceived(float[] vectorAction)
    {
        var direction = 0;
        if (!isParked)
        {
            direction = Mathf.FloorToInt(vectorAction[0]);
        }
        //Debug.Log("Direction: " + direction);
        if (!isParked)
        {
            DistancePunishment();
        }
        
        //Debug.Log("distance"+dist);

        switch (direction)
        {
            //case 0: // idle
              //  carController.CurrentDirection = Direction.Idle;

                //break;
            case 1: // forward
                carController.CurrentDirection = Direction.MoveForward;

                break;
            case 2: // backward
                carController.CurrentDirection = Direction.MoveBackward;

                break;
            case 3: // turn left
                carController.CurrentDirection = Direction.TurnLeft;

                break;
            case 4: // turn right
                carController.CurrentDirection = Direction.TurnRight;

                break;
        }
        yRotation = transform.localRotation.eulerAngles.y;
        yRotation = yRotation % 180;
    }

    public void GivePoints(float amount = 1.0f, bool isFinal = false)
    {
        AddReward(amount);

        if (isFinal)
        {
            carSpots.numberofParkedAgets++;
            isParked = true;
        }
        if (carSpots.numberofParkedAgets >= carSpots.numberofAgents)
        {
            EndEpisode();
            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
        }
    }

    public void TakeAwayPoints()
    {
        AddReward(-0.1f);
        if (GetCumulativeReward() <= -1)
        {
            EndEpisode();
            StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 2;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[0] = 3;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut[0] = 4;
        }
    }


    //private IEnumerable<CarGoal> allGoals;

    private void DistancePunishment()
    {
        GameObject[] allGoals;
        allGoals = GameObject.FindGameObjectsWithTag("goal");
        int goalCounter = 0;
        float[] dist = new float[allGoals.Length];

        foreach (var target in allGoals)
        {
            if (target.GetComponent<CarGoal>().HasCarUsedIt == false)
            {
                dist[goalCounter] = Vector3.Distance(target.transform.localPosition, transform.localPosition);

            }
            else
            {
                dist[goalCounter] = 10000;
            }
            goalCounter++;
        }
        AddReward(-dist.Min() / 10000);
    }
}
