using CoverShooter;
using CoverShooter.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    Following,
    Reached,

}

public class Nav_Movement : MonoBehaviour
{
    public static Nav_Movement Instance { get; private set; }
    public float smoothness = 3f;
    public CoverPoint[] coverPoint;
    public Cover[] cover;
    private NavMeshAgent agent;
    private Animator animator;
    public float stoppingDistance = 0.1f; // Distance threshold to stop at cover
    private int point = 0;
    private bool isReached;
    private PlayerState state;

    CharacterMotor characterMotor;
    Actor actor;
    public bool isInCoverAnim;
    bool isInCover;
    bool wait = false;
    int totalEnemies = 0;
    #region Properties
        public bool SetCameraLeft { get { return cover[point].OpenLeft; } }//To set Camera left or right 
        private bool lastCoverPoint { get { return (point == coverPoint.Length - 1); } }
        public int coverDirection { get { return SetCameraLeft ? -1 : 1; } }//Direction for player to look left or right
        public int TotalEnemies { get { return totalEnemies; } }
    #endregion
    private void Awake()
    {
        CalculateEnemies();
        Instance = this;
    }
    void Start()
    {
       
        characterMotor = GetComponent<CharacterMotor>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        actor = GetComponent<Actor>();
        foreach (var item in coverPoint)
        {
            foreach(var enemyMotor in item.enemies)
            {
                enemyMotor.isVisible = false;
            }
        }
        MoveToCover(point);

    }
    void CalculateEnemies()
    {
        foreach (CoverPoint coverPoint in coverPoint)
        {
            totalEnemies += coverPoint.TotalEnemies;
            

        }
    }
    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.T) || coverPoint[point].AreEnemiesCleared()) && !wait )
        {
            coverPoint[point].DeleteEnemiesCovers();
            wait = true;
            if(!lastCoverPoint)
                Invoke(nameof(SetNextMovePoint), 2.2f);
            else
            {
                Invoke(nameof(DestroyCoverPoint), .1f);
                ThirdPersonInput.isUsingWeapon=false;
            }
               
          
            //  SetNextMovePoint();
        }
        if (!isInCover)
        {
            if (!agent.pathPending && agent.remainingDistance <= stoppingDistance && !isReached)
            {
                GameManager.instance.PlayerStopped();
                // Stop movement and transition to cover position
                EnterCoverPosition();
            }
            else
            {
                GameManager.instance.PlayerRunning();
                UpdateAnimatorParameters();
                UpdateAgentRotation();
            }

        }
      /*  else
        {

            UpdateAnimatorParameters();
            UpdateAgentRotation();
         
        }*/
    }

    void SetNextMovePoint()
    {
        wait = false;
        DestroyCoverPoint();
        actor.InputLeaveCover();
        // Destroy(coverPoint[point].gameObject);
        isInCoverAnim = false;
        // animator.SetBool("ISInCover", false);
        if (!lastCoverPoint)
        {

            point++;
            agent.enabled = true;
            agent.isStopped = false;
            //agent.SetDestination(coverPoint[point].position);

            // agent.destination = coverPoint[point].gameObject.transform.position;
            MoveToCover(point);


        }
        else
        {
          
            Debug.Log("All cover points reached.");
        }
        isReached = false;

    }

    void DestroyCoverPoint()
    {
        Destroy(cover[point].gameObject);
    }
    void MoveToCover(int movPoint)
    {
        foreach (var enemyMotor in coverPoint[movPoint].enemies)
        {
            enemyMotor.isVisible = true;
        }
        agent.enabled = true;
        agent.isStopped = false;
        isInCover = false;
        if (coverPoint[movPoint] != null)
        {

            agent.SetDestination(coverPoint[movPoint].gameObject.transform.position);
          
        }

    }


    void UpdateAnimatorParameters()
    {
       
        // Calculate movement speed and direction
        float movementSpeed = agent.velocity.magnitude;
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        float movementX = localVelocity.x;
        float movementZ = localVelocity.z;
        // Update animator parameters
        animator.SetFloat("MovementSpeed", movementSpeed * smoothness);
        animator.SetFloat("MovementX", movementX * smoothness);
        animator.SetFloat("MovementZ", movementZ * smoothness);
        
    }

    void EnterCoverPosition()
    {
       
        
        // Stop the agent
        agent.isStopped = true;
        isReached = true;
        isInCover = true;
        isInCoverAnim = true;
        agent.enabled = false;
        // Reset movement parameters to stop animations
        animator.SetFloat("MovementSpeed", 0f);
        animator.SetFloat("MovementX", 0f);
        animator.SetFloat("MovementZ", 0f);
        actor.InputTakeCover();
        /*// Trigger cover animation (assuming there's a parameter or trigger)
        animator.SetBool("ISInCover", true); // Or use a trigger if preferred*/
    }
    void UpdateAgentRotation()
    {
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            // Rotate the agent to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothness);
        }
    }

    public bool GetCover() { return isInCoverAnim; }

   
   
   
}
