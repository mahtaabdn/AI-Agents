using UnityEngine;

public class CarGoal : MonoBehaviour
{
    private CarAgent agent = null;
    //private SecondCarAgent secondAgent = null;

    [SerializeField]
    private int goalTag = 0;

    [SerializeField]
    private float goalReward = 0.1f;

    private float rotError = 10;

    // to avoid AI from cheating ;)
    public bool HasCarUsedIt { get; set; } = false;

    public bool HasCarEnteredIt { get; set; } = false;

    public enum GoalType
    {
        Milestone,
        FinalDestination
    }
    [SerializeField]
    private GoalType goalType = GoalType.Milestone;

    public GoalType GoalTypeValue { get { return this.goalType; } }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player" && !HasCarUsedIt)
        {
            agent = collider.transform.GetComponentInChildren<CarAgent>();
            if (GoalTypeValue == GoalType.FinalDestination)
            {
                if (!HasCarEnteredIt && agent.usedFinalGoalReward == false)
                {
                    agent.GivePoints(0.2f);
                    HasCarEnteredIt = true;
                    agent.usedFinalGoalReward = true;
                }
                if ((Mathf.Abs(agent.yRotation) < (90 + rotError)) && (Mathf.Abs(agent.yRotation) > (90 - rotError)))
                {
                    HasCarUsedIt = true;

                    agent.GivePoints(goalReward, true);
                    Debug.Log("Accepted: " + agent.yRotation);
                    Destroy(transform.parent.gameObject);
                }
            }
            else
            {
                if (!HasCarEnteredIt && agent.usedMileStoneReward == false)
                {
                    if(agent.theAgentNumber == goalTag)
                    {
                        Debug.Log("y rot: " + agent.yRotation);
                        agent.GivePoints(goalReward - (Mathf.Abs((agent.yRotation - 90) / 200)));
                        //Debug.Log(goalReward - (Mathf.Abs((agent.yRotation - 90) / 200)));
                        HasCarEnteredIt = true;
                        agent.selectedGoalPositionX = transform.parent.localPosition.x;
                        //agent.selectedGoalPositionY = transform.parent.localPosition.y;
                        agent.selectedGoalPositionZ = transform.parent.localPosition.z;
                        agent.usedMileStoneReward = true;
                    }
                    
                }
            }
        }

      
    }
}