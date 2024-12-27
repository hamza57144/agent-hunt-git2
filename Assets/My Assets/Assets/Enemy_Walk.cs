using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Walk : MonoBehaviour
{
    public Transform pointA; // First waypoint
    public Transform pointB; // Second waypoint
    public Transform hidePoint; // Point where enemy will run and hide
    public float detectionRange = 20f; // Range to detect the player
    public float runSpeed = 5f; // Speed of the enemy when running to hide
    public float rotationSpeed = 5f; // Speed of rotation when running
    public Animator anim; // Animator for controlling animations

    public GameObject player; // Reference to the player
    public float speed = 2.0f; // Speed for walking between waypoints
    public float idleTime = 5.0f; // Time to stay idle at point B

    private NavMeshAgent agent;
    private Transform targetPoint;
    private bool isIdle = false; // To check if the enemy is currently idle
    private bool isHiding = false; // To check if the enemy is currently hiding
    private bool playerDetected = false; // To check if the player is detected

    void Start()
    {
        // Set initial target to point B and find player
        targetPoint = pointB;
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = speed; // Set agent speed for waypoint movement
        anim.SetBool("walk", true); // Start walking animation
    }

    void Update()
    {
        // Skip movement if idle or hiding
        if (isIdle || isHiding) return;

        // Check the distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the player is within detection range and the enemy is not already hiding
        if (distanceToPlayer <= detectionRange && !isHiding)
        {
            RunToHidePoint(); // Run to the hide point
        }
        else
        {
            // Move between waypoints if not hiding
            MoveToTarget();

            // Check if the enemy reached the target point
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
            {
                // If at point B, go idle for a while
                if (targetPoint == pointB)
                {
                    StartCoroutine(Idle());
                }
                else
                {
                    // Switch target point to the opposite waypoint
                    targetPoint = pointB;
                }
            }
        }
    }

    // Method to move the enemy to the current target point
    void MoveToTarget()
    {
        agent.SetDestination(targetPoint.position); // Move with NavMeshAgent
    }

    // Coroutine to handle idle behavior at point B
    private IEnumerator Idle()
    {
        isIdle = true; // Set idle state
        anim.SetBool("idle", true);
        anim.SetBool("walk", false);
        yield return new WaitForSeconds(idleTime); // Wait for idle time
        targetPoint = pointA; // Set target back to point A
        isIdle = false; // Reset idle state
        anim.SetBool("idle", false);
        anim.SetBool("walk", true); // Set walking animation back
    }

    // Method to trigger running to the hide point
    void RunToHidePoint()
    {
        if (agent.pathPending) return; // Wait for path calculation to finish

        isHiding = true; // Mark that the enemy is hiding
        playerDetected = true; // Player has been detected

        // Set animation to run
        anim.SetBool("walk", false);
        anim.SetBool("run", true);

        // Move the enemy towards the hide point using NavMeshAgent
        agent.speed = runSpeed; // Change speed for running
        agent.SetDestination(hidePoint.position);

        // Set a stopping distance to avoid jitter at the end
        agent.stoppingDistance = 0.5f;

        // Stop movement once at hide point
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool("run", false);
            anim.SetBool("crouch", true); // Crouch when hiding
            agent.isStopped = true; // Stop the agent
        }
    }

    // Optional: Visualize detection range in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw detection range in the scene view
    }
}
