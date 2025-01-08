using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50, 0); // Rotate around the Y-axis


    void Update()
    {
        
    
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
