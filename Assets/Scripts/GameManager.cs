using CoverShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance { get; private set; }
    public CharacterMotor player;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
}
