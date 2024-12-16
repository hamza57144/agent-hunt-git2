using CoverShooter;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    int totalEnemies=0;
    public CharacterMotor[] enemies;
    public GameObject[] enemiesCovers;
    
    void Awake()
    {
        totalEnemies=enemies.Length;
        
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

    public int TotalEnemies {  get { return totalEnemies; } }
    
   
}
