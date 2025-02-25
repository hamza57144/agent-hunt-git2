using CoverShooter;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    [Tooltip("Number in placement")]
    public int coverNumber ;
    public CharacterMotor[] enemies;
    public GameObject[] enemiesCovers;
    public GameObject helpingPoints;
    public int TotalEnemies { get { return enemies.Length; } }
    bool areEnemiesAlerted;

   
    private void Start()
    {
        AIAlerts.OnEnemyAlert += AIAlerts_OnEnemyAlert;
        CharacterMotor.EnemyDie += CharacterMotor_EnemyDie;
        
    }
    /// <summary>
    /// If one of the enemy in cover point will die,
    /// all enemies in the cover point will get alerted
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CharacterMotor_EnemyDie(object sender, System.EventArgs e)
    {
        
      //  if (!areEnemiesAlerted)
            AlertAllEnemies();
    } 
    /// <summary>
    /// If one of the the enemy got hit, all enemies will get alerted
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AIAlerts_OnEnemyAlert(object sender, System.EventArgs e)
    {
      /*  if (!areEnemiesAlerted)*/
            AlertAllEnemies();
    }
    /// <summary>
    /// Check if enemies in this cover point 
    /// are defeated or not
    /// </summary>
    /// <returns></returns>
    public bool AreEnemiesCleared()
    {
       
        foreach (CharacterMotor enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                return false;
                
            } // Enemy still alive
                

        }
        return true; // All enemies are defeated
        
    }
    /// <summary>
    /// Delete the covers which  block the player and
    /// prevents the player to move 
    /// </summary>
    public void DeleteEnemiesCovers()
    {
        foreach (GameObject enemy in enemiesCovers)
        {
            Destroy(enemy);
        }
    }
    public void AlertAllEnemies()
    {
       
        areEnemiesAlerted =true;
        if (coverNumber == Nav_Movement.Instance.ActiveCoverPoint)
        {
            foreach (CharacterMotor item in enemies)
            {
                item.gameObject.GetComponent<AISight>().enabled = true;              
            }
        }
       
    }
    private void OnDisable()
    {
        AIAlerts.OnEnemyAlert -= AIAlerts_OnEnemyAlert;
        CharacterMotor.EnemyDie -= CharacterMotor_EnemyDie;
    }
    public GameObject GetHelpingPoint()
    {
        if(helpingPoints!=null)
            return helpingPoints;
        else return null;
    }

}
