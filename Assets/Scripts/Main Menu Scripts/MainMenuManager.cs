using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playerSelectionPanel;
    [SerializeField] GameObject weaponsSelectionPanel;
    [SerializeField] GameObject levelSelectionPanel;
    [SerializeField] GameObject loadingScreen;

    /// <summary>
    /// Temporay using this approach for loading Scenes,will replace this later and manage scene loading by static classes
    /// and may be a separate scene for loading if needed
    /// </summary>
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    // Time to wait before starting to load the scene (e.g., splash screen delay)
    public float waitTime = 3f;

    // Total loading time for the scene (how long it will take to fill the progress bar)
    public float loadingTime = 5f;

    private void Awake()
    {
        UnlockCursor();
    }
    void UnlockCursor()
    {
        //If cursor get lock due to scene loading,unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Start()
    {
        mainMenu.SetActive(true);
        playerSelectionPanel.SetActive(false);
        weaponsSelectionPanel.SetActive(false);
        levelSelectionPanel.SetActive(false);
    }
    public void OnSelectPlayerBtnClick()
    {
        mainMenu.SetActive(false);
        playerSelectionPanel.SetActive(true);
    }
    public void OnSelectWeaponBtnClick()
    {
        mainMenu.SetActive(false);
        weaponsSelectionPanel.SetActive(true);
    }
    public void OnSelectLevelBtnClick()
    {
        mainMenu.SetActive(false);
        levelSelectionPanel.SetActive(true);
    }
    public void SelectPlayer(int index)
    {
        loadingScreen.SetActive(true);
        playerSelectionPanel.SetActive(false );

        GameData.SaveSelectedPlayer(index);
        
        StartCoroutine(LoadSceneWithProgress());

    }
    private IEnumerator LoadSceneWithProgress()
    {
        // Wait for the specified splash screen delay
        yield return new WaitForSeconds(waitTime);

        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneHandler.GamePlayScene);

        // Don't allow the scene to activate immediately
        asyncOperation.allowSceneActivation = false;

        // Variable to track the simulated progress
        float simulatedProgress = 0f;

        // Gradually update the progress bar over the set loading time
        while (!asyncOperation.isDone)
        {
            // Increase the simulated progress over time (based on loadingTime)
            simulatedProgress += Time.deltaTime / loadingTime;

            // Ensure progress is capped at 1 (100%)
            simulatedProgress = Mathf.Clamp01(simulatedProgress);

            // Update the progress bar and text
            progressBar.value = simulatedProgress;
            progressText.text = Mathf.RoundToInt(simulatedProgress * 100) + "%";

            // Once the scene is almost loaded (90%), allow activation
            if (simulatedProgress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
