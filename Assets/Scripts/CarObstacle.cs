using UnityEngine;

public class CarObstacle : MonoBehaviour
{
    public enum CarObstacleType
    {
        Barrier,
        Tree,
        Car,
        Ground,
        Player
    }

    [SerializeField]
    private CarObstacleType carObstacleType = CarObstacleType.Barrier;

    public CarObstacleType CarObstacleTypeValue { get { return this.carObstacleType; } }

    private CarAgent agent;

    private SecondCarAgent secondAgent;

    void Awake()
    {
        if(CarObstacleTypeValue == CarObstacleType.Barrier)
        {
            agent = transform.parent.parent.parent.parent.GetComponentInChildren<CarAgent>();
            //secondAgent = transform.parent.parent.parent.parent.GetComponentInChildren<SecondCarAgent>();
        }

        if (CarObstacleTypeValue == CarObstacleType.Car)
        {
            agent = transform.parent.parent.GetComponentInChildren<CarAgent>();
            //secondAgent = transform.parent.parent.GetComponentInChildren<SecondCarAgent>();
        }

        if (CarObstacleTypeValue == CarObstacleType.Ground)
        {
            agent = transform.parent.parent.GetComponentInChildren<CarAgent>();
            //secondAgent = transform.parent.parent.GetComponentInChildren<SecondCarAgent>();
        }

        if (CarObstacleTypeValue == CarObstacleType.Player)
        {
            agent = transform.parent.GetComponentInChildren<CarAgent>();
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player")
        {
            //Debug.Log(CarObstacleTypeValue);
            collider.GetComponent<CarAgent>().TakeAwayPoints();
        }

    }
}
