/*using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Camera.main == null) return;

        // Make the health bar face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        // Keep the health bar upright (ignore X & Z rotation)
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }


}
*/