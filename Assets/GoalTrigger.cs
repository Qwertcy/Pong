using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public bool isLeftGoal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GameManager.Instance.GoalScored(isLeftGoal);
        }
    }
}
