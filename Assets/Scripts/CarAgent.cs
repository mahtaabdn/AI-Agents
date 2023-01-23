using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static CarController;
using System.Linq;

public class CarAgent : BaseAgent
{
    public Vector3 originalPosition;

    private BehaviorParameters behaviorParameters;

    private CarController carController;

    private AgentsController agentsController;

    private CarSpots carSpots;

    private GameObject goal;

    private Transform relativePos;
    private Vector3 goalPos;

    public float xPos;

    public float yRotation;

    public bool isParked = false;

    [SerializeField]
    private GameObject otherAgent;

    //private GameObject[] allGoals;
    private int targetID;

    private GameObject[] allGoals;

    private Vector3 relativeTargetPosition;

    private float totalNegative = 0;

    public bool usedFinalGoalReward = false;
    public bool usedMileStoneReward = false;

    public float selectedGoalPositionX;
   // public float selectedGoalPositionY;
    public float selectedGoalPositionZ;

    private float remainTime = 60;

    private int direction = 4;

    [SerializeField]
    public int theAgentNumber = 0;





    // private float[] dist = new float[];

    // private IEnumerable<CarGoal> allGoals;

    void Awake()
    {
        //GameObject[] allGoals;
        //allGoals = GameObject.FindGameObjectsWithTag("goal");

        originalPosition = transform.localPosition;
        behaviorParameters = transform.GetComponentInChildren<BehaviorParameters>();
        carController = transform.GetComponentInChildren<CarController>();
        carSpots = transform.parent.GetComponentInChildren<CarSpots>();
        agentsController = transform.parent.GetComponentInChildren<AgentsController>();
        carSpots.Setup();
        //targetID = agentsController.ReturnGoalID();
        // Debug.Log(targetID);
        //goal = carSpots.carGoal[targetID];
        //targetID = agentsController.ReturnGoalID();
    }

    void Update()
    {
        if(remainTime <= 0)
        {
            Debug.Log("reset timer");
            agentsController.onceSet = false;
            if (!agentsController.onceSet)
            {
                agentsController.ResetParkingLotArea();
                remainTime = 60;
            }
            //EndEpisode();
            StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
        }
        else
        {
            remainTime = remainTime - Time.deltaTime;
        }
       // Debug.Log(remainTime);
    }

    public override void OnEpisodeBegin()
    {
        //Debug.Log("episode begins");
        totalNegative = 0;
        usedFinalGoalReward = false;
        usedMileStoneReward = false;
        remainTime = 60;
        //isParked = false;
        //carSpots.oneAgentSet = false;
        //isParked = false;
        /* if (!agentsController.onceSet)
         {
             agentsController.ResetParkingLotArea();
         }*/
        //agentsController.ResetParkingLotArea();
        //GameObject[] allGoals;
        //allGoals = GameObject.FindGameObjectsWithTag("goal");
        //targetID = agentsController.ReturnGoalID();
        //allGoals = GameObject.FindGameObjectsWithTag("goal");
        //goal = allGoals[targetID];
        //goalPos = goal.transform.localPosition;
        //Debug.Log(goalPos);
        //goal = carSpots.carGoal[targetID];

    }

    /* public void ResetParkingLotArea()
     {
         xPos = Random.Range(-3, 3);
         originalPosition = new Vector3(xPos, originalPosition.y, originalPosition.z);

         transform.localPosition = originalPosition;
         transform.localRotation = Quaternion.identity;
         carController.CarRigidbody.velocity = Vector3.zero;
         // reset which cars show or not show

         isParked = false;
         otherAgent.GetComponent<SecondCarAgent>().ResetParkingLotArea();


         if (carSpots.onceSet == true)
         {
             carSpots.DestroyGoals();
         }

         carSpots.Setup();


     }*/

    /*void Update()
    {
        if (transform.localPosition.y <= 0)
        {
            TakeAwayPoints();
        }
    }*/

    public override void CollectObservations(VectorSensor sensor)
    {
        // 3 observations - x, y, z
        //sensor.AddObservation(transform.localPosition);

        //sensor.AddObservation(carSpots.CarGoal.transform.rotation.y - transform.rotation.y);
        //goal = allGoals[targetID];

        //relativePos = new Vector3(goalPos - transform.localPosition);

        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.rotation.y);

        GameObject[] allGoalss;
        if (theAgentNumber == 1)
        {
            allGoalss = GameObject.FindGameObjectsWithTag("goal");
        }
        else
        {
            allGoalss = GameObject.FindGameObjectsWithTag("secondGoal");
        }
        //allGoals = GameObject.FindGameObjectsWithTag("goal");
        int goalCounter = 0;
        float[] dist = new float[allGoalss.Length];

        foreach (var target in allGoalss)
        {
            dist[goalCounter] = Vector3.Distance(target.transform.localPosition, transform.localPosition);
            goalCounter++;
        }

        float minVal = dist.Min();
        int index = System.Array.IndexOf(dist,minVal);

        sensor.AddObservation(allGoalss[index].transform.localPosition - gameObject.transform.localPosition);
        //TakeAwayPoints(-((dist.Min() / 10000f) + 0.0001f));

        //sensor.AddObservation(relativePos);
        //Debug.Log(relativePos);

        //relativeTargetPosition = transform.InverseTransformDirection(goalPos - transform.localPosition);
        //sensor.AddObservation(goalPos.x);
        //sensor.AddObservation(goalPos.y);
        // sensor.AddObservation(goalPos.z);

        //int goalID = Random.Range(0, allGoals.Length -1 );
        //float[] dist = new float[allGoals.Length];

        //relativePos = goal.transform.localPosition - transform.localPosition;

        //sensor.AddObservation(carController.CarRigidbody.velocity.x);
        //sensor.AddObservation(carController.CarRigidbody.velocity.z);
    }

    //private Vector3 relativeTargetPosition = transform.InverseTransformDirection(goal.transform.localPosition - transform.localPosition);
    //sensor.AddObservation(relativeTargetPosition);

    // 3 observations - x, y, z
    // sensor.AddObservation(carSpots.CarGoal.transform.localPosition);

    // 3 observations - x, y, z
    // sensor.AddObservation(carController.CarRigidbody.velocity);
    //}

    public override void OnActionReceived(float[] vectorAction)
    {
        direction = 4;
        //remainTime = remainTime - 1;
        if (!isParked)
        {
            direction = Mathf.FloorToInt(vectorAction[0]);
            DistancePunishment(usedMileStoneReward);
        }
        if(isParked)
        {
            direction = 4;
        }
            //Debug.Log("Direction: " + direction);
            //Debug.Log("distance"+dist);

        switch (direction)
        {
            //case 4: // idle
                //   carController.CurrentDirection = Direction.Idle;

                //  break;
            case 0: // forward
                carController.CurrentDirection = Direction.MoveForward;

                break;
            case 1: // backward
                carController.CurrentDirection = Direction.MoveBackward;

                break;
            case 2: // turn left
                carController.CurrentDirection = Direction.TurnLeft;

                break;
            case 3: // turn right
                carController.CurrentDirection = Direction.TurnRight;

                break;

            case 4: // idle
                carController.CurrentDirection = Direction.Idle;

                break;
        }
        
        //yRotation = transform.localRotation.eulerAngles.y;
        yRotation = Mathf.Abs(transform.localRotation.eulerAngles.y) % 180;
        
    }

    public void GivePoints(float amount = 1.0f, bool isFinal = false)
    {
        AddReward(amount);

        if (isFinal)
        {
            carSpots.numberofParkedAgets++;
            isParked = true;
            //Debug.Log(isParked);

        }
        //Debug.Log(carSpots.numberofParkedAgets);
        if (carSpots.numberofParkedAgets >= carSpots.numberofAgents)
        {
            Debug.Log("Both are Parked");
            agentsController.onceSet = false;
            if (!agentsController.onceSet)
            {
                agentsController.ResetParkingLotArea();
            }
            //EndEpisode();
            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
        }
    }
     
    public void TakeAwayPoints(float amount = -0.05f)
    {
        AddReward(amount);
        totalNegative = totalNegative + amount;
        //Debug.Log(totalNegative);
        
        if (totalNegative <= -3f)
        {
            agentsController.onceSet = false;
            if (!agentsController.onceSet)
            {
                agentsController.ResetParkingLotArea();
            }
            //EndEpisode();
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

    private void DistancePunishment(bool TouchmiddleGoal = false)
    {
        if(TouchmiddleGoal == false)
        {
            GameObject[] allGoals;
            if(theAgentNumber == 1)
            {
                allGoals = GameObject.FindGameObjectsWithTag("goal");
            }
            else
            {
                allGoals = GameObject.FindGameObjectsWithTag("secondGoal");
            }
            //allGoals = GameObject.FindGameObjectsWithTag("goal");
            int goalCounter = 0;
            float[] dist = new float[allGoals.Length];

            foreach (var target in allGoals)
            {
                dist[goalCounter] = Vector3.Distance(target.transform.localPosition, transform.localPosition);
                goalCounter++;
            }
            TakeAwayPoints(-((dist.Min() / 10000f) + 0.0001f));
            
        }

       else
        {
            float distanceCalc = Mathf.Sqrt(Mathf.Pow(selectedGoalPositionX - transform.localPosition.x, 2) + Mathf.Pow(selectedGoalPositionZ - transform.localPosition.z, 2));
            TakeAwayPoints(-((distanceCalc / 10000f) + 0.0001f));
        }
       
        //TakeAwayPoints(-1 / 100);
    }
}
