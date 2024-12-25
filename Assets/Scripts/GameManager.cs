using CoverShooter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance { get; private set; }
    public CharacterMotor player;
    
    [SerializeField] EnemyDisplayManager enemyDisplayManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] ThirdPersonCamera thirdPersonCamera;
    public CharacterMotor PlayerMotor { get { return player; } }
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void PlayerRunning()
    {
     
        gameCanvas.gameObject.SetActive(false);
    }

    public void PlayerStopped()
    {
        gameCanvas.gameObject.SetActive(true);
        if (Nav_Movement.Instance.SetCameraLeft)
        {
            thirdPersonCamera.Horizontal = -18f;
        }
        else
        {
            thirdPersonCamera.Horizontal = 13;
        }

    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public  bool HasScope()
    {
        return player.HasScope();
    }
   

   
    public void HideArrow()
    {
        enemyDisplayManager.gameObject.SetActive(false);
    
    }
}
