using CoverShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BodyPart
{
    body,
    head,
}
public class BodyPartDetection : MonoBehaviour
{
    public BodyPart bodyPart;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Trigger detected Class:BodypartDetectop");
            EnemyManager.instance.EnableAnimator();
            CharacterHealth enemyHealth = GetComponentInParent<CharacterHealth>();
                
            other.gameObject.GetComponent<Bullet>().ShootEnemy();
            enemyHealth.Health = 0f;
            enemyHealth._motor.Die();



        }
    }

}
