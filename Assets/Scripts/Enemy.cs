using CoverShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     
  
   
  
    public float CheckHealth()
    {

            CharacterHealth health = GetComponentInParent<CharacterHealth>();
            return health.GetHealth;
        
    }
}
