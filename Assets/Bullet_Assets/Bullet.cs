using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform visualTransform;

    private Transform hitTransform;
    private bool isEnemyShot;
    private float shootingForce;
    private Vector3 direction;
    private Vector3 hitPoint;
    [SerializeField] private GameObject blood;



    public void Launch(float shootingForce, Transform hitTransform, Vector3 hitPoint)
    {
        direction = (hitPoint - transform.position).normalized;
        isEnemyShot = false;
        this.hitTransform = hitTransform;
        this.shootingForce = shootingForce;
        this.hitPoint = hitPoint;
    }

    private void Update()
    {
        Move();
        Rotate();
      //  CheckDistanceToEnemy();
    }

    private void Move()
    {
        transform.Translate(direction * shootingForce * Time.deltaTime, Space.World);
    }

    public void InstantiateBlood()
    {
        var bloodEffect = Instantiate(blood);
        bloodEffect.transform.position = hitPoint;
    }

    private void Rotate()
    {
        visualTransform.Rotate(Vector3.forward, 1200 * Time.deltaTime, Space.Self);
    }

    public void ShootEnemy()
    {
        isEnemyShot = true;
       // Rigidbody shotRB = hitTransform.GetComponent<Rigidbody>();
       // enemy.OnEnemyShot(transform.forward, shotRB);
    }

    public float GetBulletSpeed()
    {
        return shootingForce;
    }

	internal Transform GetHitEnemyTransform()
	{
        return hitTransform;
	}
}