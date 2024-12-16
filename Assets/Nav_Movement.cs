using CoverShooter;
using CoverShooter.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    Following,
    Reached,

}

public class Nav_Movement : MonoBehaviour
{
    public static Nav_Movement Instance {  get; private set; }
    public float smoothness = 3f;
    public CoverPoint[] coverPoint;
    public GameObject[] cover;
    private NavMeshAgent agent;
    private Animator animator;
    public float stoppingDistance = 0.1f; // Distance threshold to stop at cover
    private int point = 0;
    private bool isReached;
    private PlayerState state;
    CharacterMotor characterMotor;
    Actor actor;
    public /*static*/ bool isInCoverAnim;
    bool isInCover;
    bool wait = false;
    int totalEnemies = 0;

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
        MoveToCover(point);
        actor = GetComponent<Actor>();
        

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

      

        if ((Input.GetKeyDown(KeyCode.T) || coverPoint[point].AreEnemiesCleared()) && !wait)
        {
            coverPoint[point].DeleteEnemiesCovers();
            wait = true;
            Invoke(nameof(SetNextMovePoint), 2.2f);
            //  SetNextMovePoint();
        }
        if (!isInCover)
        {
            if (!agent.pathPending && agent.remainingDistance <= stoppingDistance && !isReached)
            {
                // Stop movement and transition to cover position
                EnterCoverPosition();
            }
            else
            {
                UpdateAnimatorParameters();
                UpdateAgentRotation();
            }

        }
        else
        {

            UpdateAnimatorParameters();
            UpdateAgentRotation();
        }
    }

    void SetNextMovePoint()
    {
        wait = false;
        Destroy(cover[point].gameObject);
        actor.InputLeaveCover();
        // Destroy(coverPoint[point].gameObject);
        isInCoverAnim = false;
        // animator.SetBool("ISInCover", false);
        if (point < coverPoint.Length - 1)
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


    void MoveToCover(int movPoint)
    {

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
        // Trigger cover animation (assuming there's a parameter or trigger)
        animator.SetBool("ISInCover", true); // Or use a trigger if preferred
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

    public int TotalEnemies {  get { return totalEnemies; } }
   
   
}
