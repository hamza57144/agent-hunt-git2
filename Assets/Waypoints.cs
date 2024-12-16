using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Waypoint[] Points; // Array of waypoints

    // Class to store individual waypoint data
    [System.Serializable]
    public class Waypoint
    {
        public Vector3 Position;  // Position of the waypoint
        public bool Run;          // Should the AI run to this waypoint?
        public float Pause;       // How long to pause at the waypoint
    }

    void OnDrawGizmos()
    {
        if (Points == null || Points.Length < 2)
            return;

        Gizmos.color = Color.green;

        // Draw lines between waypoints in the editor
        for (int i = 0; i < Points.Length - 1; i++)
        {
            Gizmos.DrawLine(Points[i].Position, Points[i + 1].Position);
        }

        // Draw a line from the last waypoint to the first to complete the loop
        Gizmos.DrawLine(Points[Points.Length - 1].Position, Points[0].Position);
    }

}

