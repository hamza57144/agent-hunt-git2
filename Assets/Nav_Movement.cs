using ControlFreak2.Demos.RPG;
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
    
    public int ActiveCoverPoint { get; set; }
    [SerializeField] LevelManager levelManager;
    public float smoothness = 3f;
    private CoverPoint[] coverPoint;
    private Cover[] covers;
    public NavMeshAgent agent;
    private Animator animator;
    public float stoppingDistance = 0.1f; // Distance threshold to stop at cover
    private int point = 0;
    public  bool isReached;
    private PlayerState state;
    //For optimization
    private GameObject player;
    
    Actor actor;
    public bool isInCoverAnim;
    bool isInCover;
    bool wait = false;
    int totalEnemies = 0;
    [SerializeField] StatgesData stagesData;
    
    #region Properties
        public bool SetCameraLeft { get { return covers[point].LowCover; } }//To set Camera left or right 
    public bool LowCover { get { return true; } }//To set Camera left or right 
    private bool lastCoverPoint { get { return (point == coverPoint.Length - 1); } }
        public int coverDirection { get { return SetCameraLeft ? -1 : 1; } }//Direction for player to look left or right
        public int TotalEnemies { get { return totalEnemies; } }
        public bool PlaySwitchTutorial { get { return GameData.CompletedLevelIndex == 2 && point == 1; } }
        private GameObject helpingPoint { get { return coverPoint[point].GetHelpingPoint(); } }

    #endregion
    private void Awake()
    {
        coverPoint = levelManager.GetCoverPoints();
        covers = levelManager.GetCovers();
        CalculateEnemies();
        
        Instance = this;
    }
    void Start()
    {
        ActiveCoverPoint = 0;
        player = GameManager.instance.PlayerMotor.gameObject;  
        agent = player.GetComponent<NavMeshAgent>();
        animator = player.GetComponent<Animator>();
        actor = player.GetComponent<Actor>();
        foreach (var item in coverPoint)
        {
            foreach(var enemyMotor in item.enemies)
            {
                enemyMotor.isVisible = false;
            }
        }
       if(!stagesData.IsBossLevel(GameData.CompletedLevelIndex))
          MoveToCover(point);
      

    }
   public void StartMoving()
    {
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
      //  if (!agent.pathPending && agent.remainingDistance && 
        if ((Input.GetKeyDown(KeyCode.T) || (coverPoint[point].AreEnemiesCleared()) && !wait)|| BossMove.moveToNext)
        {
            BossMove.moveToNext=false;
            coverPoint[point].DeleteEnemiesCovers();
            wait = true;
            if(!lastCoverPoint)
            {
                Invoke(nameof(SetNextMovePoint), 2.2f);
            
            }
            else
            {
               // Invoke(nameof(DestroyCoverPoint), 2.1f);
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
              //  UpdateAgentRotation();
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
        if(covers[point].gameObject!=null)
        Destroy(covers[point].gameObject);
    }
    public void MoveToCover(int movPoint)
    {
        if (PlaySwitchTutorial)
        {           
            GameManager.instance.EnableSwitchWeaponTutorial();
        }
        ActiveCoverPoint++;
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
    }
    void UpdateAgentRotation()
    {
        // Check if the agent is moving
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            // Calculate the target rotation based on the agent's velocity
            Vector3 movementDirection = agent.velocity.normalized; // Get the normalized direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection); // Create a rotation that looks in the movement direction

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothness);
        }

    }

}
