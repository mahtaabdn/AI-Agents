using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] carPrefabs;

    private CarAgent[] carAgent;

    public bool onceSet = false;

    public int[] goalID;

    private bool isUniqueID = false;

    private int IDogGoal = 0;

    [SerializeField]
    private int numberofAgents = 2;

    // Start is called before the first frame update
    public void Setup()
    {
        carAgent = new CarAgent[numberofAgents];
       
    }

    public void ResetParkingLotArea()
    {
       
        onceSet = true;
        IDogGoal = 0;
        int counter = 0;
        carAgent = new CarAgent[numberofAgents];
        foreach (var car in carPrefabs)
        {
            //Debug.Log(counter);
            carAgent[counter] = car.transform.GetComponentInChildren<CarAgent>();
            carAgent[counter].EndEpisode();


            carAgent[counter].xPos = Random.Range(-3, 3);
            carAgent[counter].originalPosition = new Vector3(carAgent[counter].xPos, carAgent[counter].originalPosition.y, carAgent[counter].originalPosition.z);

            carAgent[counter].transform.localPosition = carAgent[counter].originalPosition;
            carAgent[counter].transform.localRotation = Quaternion.identity;
            car.transform.GetComponentInChildren<CarController>().CarRigidbody.velocity = Vector3.zero;
           
            carAgent[counter].isParked = false;
            counter++;
        }


        transform.parent.GetComponentInChildren<CarSpots>().Setup();


    }

    public int ReturnGoalID()
    {
        //Debug.Log(goalID.Length);
        int Outputnumber = IDogGoal;
        IDogGoal++;
        return Outputnumber;
    }
}
