using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private float delayBeforeLoading = 3f; // Optional delay in seconds
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Scene name
    [SerializeField] private Slider progressBar; // Reference to the UI slider
    [SerializeField] private float simulatedLoadingTime = 5f; // Total time for the slider to fill

    private async void Start()
    {
        // Optional: Wait for the delay before starting the loading process
        await Task.Delay((int)(delayBeforeLoading * 1000));

        // Load the main menu scene asynchronously
        await LoadMainMenuWithSimulatedProgress();
    }

    private async Task LoadMainMenuWithSimulatedProgress()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenuSceneName);
        asyncLoad.allowSceneActivation = false; // Prevent automatic activation

        float elapsedTime = 0f;

        while (!asyncLoad.isDone)
        {
            // Simulate progress based on time
            elapsedTime += Time.deltaTime;
            float simulatedProgress = Mathf.Clamp01(elapsedTime / simulatedLoadingTime);

            // Update the slider
            progressBar.value = simulatedProgress;

            Debug.Log($"Simulated Progress: {simulatedProgress}");

            // Allow the scene to activate when the slider is full
            if (simulatedProgress >= 1f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            await Task.Yield(); // Wait for the next frame
        }
    }
}
