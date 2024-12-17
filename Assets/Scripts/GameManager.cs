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
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Nav_Movement.Instance.PlayerStopped += PlayerStopped;
        Nav_Movement.Instance.PlayerRunning += PlayerRunning;
    }

    private void PlayerRunning(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void PlayerStopped(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public  bool HasScope()
    {
        return player.HasScope();
    }
    public CharacterMotor GetPlayer()
    {
        return player;
    }

    public void HideArrow()
    {
        enemyDisplayManager.gameObject.SetActive(false);
    
    }
}
