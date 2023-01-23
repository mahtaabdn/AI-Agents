using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CarObstacle;

public class CarSpots : MonoBehaviour
{
    [SerializeField]
    private GameObject carGoalPrefab;

    [SerializeField]
    private GameObject secondCarGoalPrefab;

    private IEnumerable<CarObstacle> parkedCars;

    public GameObject[] carGoal = null;

    public CarGoal CarGoal { get; private set; }

    private bool isUnique = true;

    public int goalsNumber = 1;

    public bool onceSet = false;

    //public bool oneAgentSet = false;

    [SerializeField]
    private int minGoalsNumber = 1;

    [SerializeField]
    private int maxGoalsNumber = 2;

    [SerializeField]
    public int numberofAgents = 2;

    public int numberofParkedAgets = 0;

    private GameObject[] allGoals;

    public void Setup()
    {
        DestroyGoals();
        goalsNumber = Random.Range(minGoalsNumber, maxGoalsNumber);

        numberofParkedAgets = 0;
        //goalsNumber = Random.Range(minGoalsNumber, maxGoalsNumber);
        carGoal = new GameObject[goalsNumber];
        int[] carTohide = new int[goalsNumber];
        parkedCars = GetComponentsInChildren<CarObstacle>(true)
            .Where(c => c.CarObstacleTypeValue == CarObstacleType.Car);

        int parkedCarsCount = parkedCars.Count();

        for(int i = 0; i < goalsNumber; i ++)
        {
            isUnique = false;
            while(isUnique == false)
            {
                carTohide[i] = Random.Range(1, parkedCarsCount);
                isUnique = true;
                for (int j = 0; j < i; j++)
                {
                    if(carTohide[j] == carTohide[i])
                    {
                        isUnique = false;
                    }
                }
            }
        }

        int carCounter = 0;

        foreach (var car in parkedCars)
        {
            car.gameObject.SetActive(true);

            for (int i = 0; i < goalsNumber; i++)
            {
                if (carCounter == carTohide[i])
                {
                    car.gameObject.SetActive(false);
                    if(i < (goalsNumber/2))
                    {
                        carGoal[i] = Instantiate(carGoalPrefab, Vector3.zero, Quaternion.identity);
                    }
                    else
                    {
                        carGoal[i] = Instantiate(secondCarGoalPrefab, Vector3.zero, Quaternion.identity);
                    }

                    CarGoal = carGoal[i].transform.GetComponentInChildren<CarGoal>();
                    CarGoal.HasCarUsedIt = false;
                    CarGoal.HasCarEnteredIt = false;
                    carGoal[i].transform.parent = transform.parent;

                    if (carCounter <= 4)
                    {
                        carGoal[i].transform.position = car.gameObject.transform.position + new Vector3(-1.3f, 0,0);
                    }

                    if (carCounter > 4)
                    {
                        carGoal[i].transform.position = car.gameObject.transform.position + new Vector3(1.3f, 0, 0);
                        carGoal[i].transform.Rotate(0,180f,0,Space.World);
                    }

                    
                }
            }
            carCounter++;
        }
        onceSet = true;
    }

    public void DestroyGoals()
    {

        foreach(var target in carGoal)
        {
            Destroy(target);
        }

    }

}
