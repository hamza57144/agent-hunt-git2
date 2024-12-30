using CoverShooter;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{
 
    public CharacterMotor[] enemies;
    public GameObject[] enemiesCovers;
    public int TotalEnemies { get { return enemies.Length; } }

   
    private void Start()
    {
        AIAlerts.OnEnemyAlert += AIAlerts_OnEnemyAlert;
        
    }

    private void AIAlerts_OnEnemyAlert(object sender, System.EventArgs e)
    {
        AllEnemyAlert();
    }

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
    public void DeleteEnemiesCovers()
    {
        foreach (GameObject enemy in enemiesCovers)
        {
            Destroy(enemy);
        }
    }
    public void AllEnemyAlert()
    {
       
        foreach (CharacterMotor item in enemies)
        {
            item.gameObject.GetComponent<AISight>().enabled = true;
        }
    }
    
    
   
}
