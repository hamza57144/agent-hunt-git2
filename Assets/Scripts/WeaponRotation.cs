using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponRotation : MonoBehaviour
{
    public Transform targetObject { get
        {
            return MainMenuManager.weapon;
        } }
    public float rotationSpeed = 100f; // Speed of rotation during drag
    public float maxInertia = 50f; // Maximum allowable inertia value
    public float inertiaDecay = 5f; // How quickly inertia slows down
    public float defaultRotationSpeed = 10f; // Speed of default rotation

    private bool isDragging = false;
    private float previousMouseX;
    private float dragVelocity;
    private float inertia;

    // Method to call on Pointer Down (start drag)
    public void OnBeginDrag()
    {
        isDragging = true;
        previousMouseX = Input.mousePosition.x;
        dragVelocity = 0f; // Reset velocity
    }

    // Method to call on Drag (while dragging)
    public void OnDrag()
    {
        if (isDragging && targetObject != null)
        {
            float currentMouseX = Input.mousePosition.x;
            float deltaX = currentMouseX - previousMouseX;

            // Rotate the target object
            targetObject.Rotate(Vector3.up, -deltaX * rotationSpeed * Time.deltaTime);

            // Calculate drag velocity
            dragVelocity = deltaX / Time.deltaTime;

            // Update the previous mouse position
            previousMouseX = currentMouseX;
        }
    }

    // Method to call on Pointer Up (end drag)
    public void OnEndDrag()
    {
        isDragging = false;

        // Clamp the drag velocity to avoid excessive inertia
        dragVelocity = Mathf.Clamp(dragVelocity, -maxInertia, maxInertia);
        inertia = dragVelocity; // Store the clamped velocity for inertia
    }

    private void Update()
    {
        if (isDragging || targetObject == null)
        {
            return; // Stop default behavior while dragging
        }

        if (Mathf.Abs(inertia) > 0.01f) // Apply inertia if active
        {
            targetObject.Rotate(Vector3.up, -inertia * Time.deltaTime);

            // Decay the inertia over time
            inertia = Mathf.Lerp(inertia, 0f, inertiaDecay * Time.deltaTime);
        }
        else // Resume default rotation
        {
            targetObject.Rotate(Vector3.up, defaultRotationSpeed * Time.deltaTime);
        }
    }
}
