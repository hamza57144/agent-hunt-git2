using UnityEngine;
using System.Collections;

public class CameraFocusController : MonoBehaviour
{
    public Transform enemy;        // Assign in Inspector
    public Camera cam;             // Assign Camera in Inspector

    public float zoomOutSize = 7f;    // Camera zoom size
    public float transitionSpeed = 2f; // Speed of movement
    public float cameraHeight = -3.5f; // Much lower camera position (waist/knee level)
    public float startDistance = 2f;   // Closer to enemy at start
    public float zoomOutDistance = 5f; // How far back the camera moves
    public BossLevel bossLevel;
    [SerializeField] GameObject bossPanel;

    void Start()
    {
      
        cam.transform.position = enemy.position + enemy.forward * startDistance + Vector3.up * cameraHeight;
        cam.transform.LookAt(enemy.position + Vector3.up * 0.5f); // Look slightly above for a natural angle       
        StartCoroutine(ZoomOutInFrontOfEnemy());
    }

    IEnumerator ZoomOutInFrontOfEnemy()
    {
        float time = 0;
        float startSize = cam.orthographicSize;
        Vector3 startPosition = cam.transform.position;

        // Position in front of the enemy at neck level
        Vector3 targetPosition = enemy.position + enemy.forward * zoomOutDistance + Vector3.up * cameraHeight;

        while (time < 1)
        {
            time += Time.deltaTime * transitionSpeed;
            cam.orthographicSize = Mathf.Lerp(startSize, zoomOutSize, time);
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, time);

            // Look slightly downward to align with the neck instead of the head
            cam.transform.LookAt(enemy.position + Vector3.up * 1.2f);

            yield return null;
        }
        EnableBossPanel(true);
        Invoke(nameof(StartMoiving), 3f);
    }

    public void StartMoiving()
    {
        EnableBossPanel(false);
        cam.enabled = false;
        bossLevel.StartMoving();
    }
    private void EnableBossPanel(bool enable)
    {
        bossPanel.SetActive(enable);
    }
}
