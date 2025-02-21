using CoverShooter;
using UnityEngine;
using UnityEngine.AI;

public class BossMove : MonoBehaviour
{
    
    public Cover[] bossCovers; // Assign in Inspector
    public float[] healthThresholds; // Define health values for movement
    public CharacterMotor boss;
    private int currentWaypointIndex = 0;
    private bool hasMoved = false;
    private CoverPoint[] coverPoint;
    public LevelManager levelManager;
    public static bool moveToNext;

    private void Start()
    {
        coverPoint = levelManager.GetCoverPoints();
        
    }
  
    void Update()
    {
       
        if (currentWaypointIndex < healthThresholds.Length && boss.GetHealth <= healthThresholds[currentWaypointIndex] )
        {
          
            if(!hasMoved) 
             MoveToNextPoint();
           hasMoved = true;
           
        }
    }

    void MoveToNextPoint()
    {
        
        if (currentWaypointIndex < bossCovers.Length)
        {
            Destroy(bossCovers[currentWaypointIndex].gameObject);
           
            currentWaypointIndex++;
            Debug.Log("Move");
            boss.gameObject.GetComponent<AICover>().takeCover(bossCovers[currentWaypointIndex], bossCovers[currentWaypointIndex].gameObject.transform.position,1,1);
            boss.GetComponent<BaseBrain>().enabled = false;
           /* if (currentWaypointIndex!=1)*/
              Invoke(nameof(MovePlayer),1.5f);
        }
    }
    private void MovePlayer()
    {
        moveToNext = true;
        Invoke(nameof(EnableBrain), 4f);
    }

    private void EnableBrain()
    {
        boss.GetComponent<BaseBrain>().enabled = true;
    }
}
