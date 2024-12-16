using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public GameObject player; // Assign your player GameObject in the Inspector
    public float moveSpeed = 5f; // Speed at which the player will move
    private Vector3 targetPosition; // Target position for the player
    Animator anim;

    private void Start()
    {
        // Set an initial target position (optional)
        targetPosition = new Vector3(2f, 0f, 0f);
        StartCoroutine(MovePlayerToTarget(targetPosition));
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check for input to set a new target point
        if (ControlFreak2.CF2Input.GetMouseButtonDown(0)) // Left mouse button
        {
            SetTargetPoint();
        }
    }

    public void SetTargetPoint()
    {
        // Get the world position of the mouse click
        Ray ray = Camera.main.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition);
        RaycastHit hit;

        // Perform a raycast to find where the player should move
        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point; // Set the target position to the hit point
            StartCoroutine(MovePlayerToTarget(targetPosition));
        }
    }

    private IEnumerator MovePlayerToTarget(Vector3 targetPosition)
    {

        float distanceThreshold = 0.1f; // How close is "close enough" to stop moving

        // Set the animation to moving
        anim.SetBool("isMoving", true);

        // Move towards the target position until close enough
        while (Vector3.Distance(player.transform.position, targetPosition) > distanceThreshold)
        {
            Vector3 direction = (targetPosition - player.transform.position).normalized; // Calculate direction
            player.transform.position += direction * moveSpeed * Time.deltaTime; // Move the player

            yield return null; // Wait for the next frame
        }

        // Stop the animation
        anim.SetBool("isMoving", false);
        Debug.Log("Player reached the target position.");
    }
}
