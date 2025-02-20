using CoverShooter;
using UnityEngine;
using UnityEngine.AI;

public class BossMove : MonoBehaviour
{
    
    public Cover[] waypoints; // Assign in Inspector
    public float[] healthThresholds; // Define health values for movement
    public CharacterMotor boss;
    private int currentWaypointIndex = 0;
   



    void Update()
    {
        Debug.Log("Health of enemy is " + boss.GetHealth);
        if (currentWaypointIndex < healthThresholds.Length && boss.GetHealth <= healthThresholds[currentWaypointIndex])
        {
            MoveToNextPoint();
        }
    }

    void MoveToNextPoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Destroy(waypoints[currentWaypointIndex].gameObject);
            currentWaypointIndex++;
            boss.gameObject.GetComponent<AICover>().takeCover(waypoints[currentWaypointIndex], waypoints[currentWaypointIndex].gameObject.transform.position,2,1);
        }
    }
}
