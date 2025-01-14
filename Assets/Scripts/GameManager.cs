using CoverShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  
    public static GameManager instance { get; private set; }
    [SerializeField] GameObject Hand;
    public List<CharacterMotor> players;
    public CharacterMotor Player { get { return players[GameData.SelectedPlayerIndex]; } }
    [SerializeField] EnemyDisplayManager enemyDisplayManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] ThirdPersonCamera thirdPersonCamera;
    [SerializeField] GameObject levelCompleteCanvas;
    [SerializeField] GameObject levelFailCanvas;
    [SerializeField] GameObject[] controllerButtons;
    int ind;
    private int headShot;
    private int bodyShot;
    private float accuracy;
    [SerializeField] TextMeshProUGUI headShotText;
    [SerializeField] TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI healthText;
    
    [SerializeField] GameObject PausePanel;

    private int enemies;
    private int totalShots { get {  return headShot+bodyShot; } }

    private void DisableHand()
    {
        if(Hand != null)
        Hand.SetActive(false);
    }
    private void HideControllerButtons()
    {
        foreach (var item in controllerButtons)
        {
            if (item.gameObject != null)
                item.gameObject.SetActive(false);
        }
    }
    private void ShowControllerButtons()
    {
        foreach (var item in controllerButtons)
        {
            if(item.gameObject!=null)
             item.gameObject.SetActive(true);
        }
    }
    #region To Be replaced later
    public GameObject loadingCanvas;
    // References to UI elements
    public Image progressBar; // Progress bar image   
    public RectTransform bulletImage; // Reference to the bullet image (UI element)
    private float offset = 0.019f;
    // Time to wait before starting to load the scene (e.g., splash screen delay)
    public float waitTime = 3f;

    // Total loading time for the scene (how long it will take to fill the progress bar)
    public float loadingTime = 5f;

    private RectTransform progressBarRect; // RectTransform of the progress bar
    #endregion

    public (Vector3 playerPostion, Vector3 playerRotation) GetPlayerPosition
    {
        get { return (PlayerMotor.gameObject.transform.localPosition, PlayerMotor.gameObject.transform.eulerAngles); }
    }
    public CharacterMotor PlayerMotor
    {
        get
        {
            return players[GameData.SelectedPlayerIndex]; 
        }
    }

    void Awake()
    {
        GameData.LoadGameData();
        ind = GameData.CompletedLevelIndex;
       
        Player.gameObject.SetActive(true);
        instance = this;
    }
    private void Start()
    {
        if (GameData.CompletedLevelIndex == 0)
        {
            Hand.SetActive(true );
        }
        CharacterMotor.OnPlayerDie += CharacterMotor_OnPlayerDie;
        BodyPartHealth.OnBodyShot += BodyPartHealth_OnBodyShot;
        BodyPartHealth.OnHeadShot += BodyPartHealth_OnHeadShot;
        ThirdPersonInput.ButtonDown += ThirdPersonInput_ButtonDown;
        ThirdPersonInput.ButtonUp += ThirdPersonInput_ButtonUp;
        enemies = EnemyManager.instance.enemyCount;
        progressBarRect = progressBar.GetComponent<RectTransform>();
       
    }

    private void ThirdPersonInput_ButtonUp(object sender, EventArgs e)
    {
       
        ShowControllerButtons();
    }

    private void ThirdPersonInput_ButtonDown(object sender, EventArgs e)
    {
        if (GameData.CompletedLevelIndex == 0)
        {
            DisableHand();
        }
        HideControllerButtons();
    }

    private void BodyPartHealth_OnHeadShot(object sender, System.EventArgs e)
    {
        headShot++;
    }

    private void BodyPartHealth_OnBodyShot(object sender, System.EventArgs e)
    {
       bodyShot++;
    }
    public float CalculateAccuracy()
    {
        return (int)Math.Floor((float)totalShots / ThirdPersonInput.totalTries * 100f); 
    }
    private void CharacterMotor_OnPlayerDie(object sender, System.EventArgs e)
    {
        Invoke(nameof(LevelFailed), 2f);
    }

    // Start is called before the first frame update
    public void LevelComplete()
    {
        gameCanvas.SetActive(false);
        levelCompleteCanvas.SetActive(true);
       
        headShotText.text = headShot.ToString()+"/"+enemies.ToString();
        healthText.text = ((Player.GetHealth)/5).ToString("0")+"%";
        accuracyText.text = CalculateAccuracy().ToString()+"%";
        Debug.Log($"GameData.SelectedLevelIndex {GameData.CompletedLevelIndex} and i is {ind} GameManager");
    }
    public void LevelFailed()
    {
        gameCanvas.SetActive(true);
        levelFailCanvas.SetActive(true);
    }
    public void PlayerRunning()
    {

        gameCanvas.gameObject.SetActive(false);
    }

    public void PlayerStopped()
    {
        gameCanvas.gameObject.SetActive(true);
        if (Nav_Movement.Instance.SetCameraLeft)
        {
            thirdPersonCamera.Horizontal = -18f;
        }
        else
        {
            thirdPersonCamera.Horizontal = 13;
        }

    }

    public void HideArrow()
    {
        enemyDisplayManager.gameObject.SetActive(false);
    }
     public void RestartGame()
     {
        levelFailCanvas.SetActive(false);
        loadingCanvas.SetActive(true);
        GameData.SaveCompletedLevel(ind);
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneWithProgress());
    }
    public void OnNextButtonClicked()
    {
        levelCompleteCanvas.SetActive(false );
        loadingCanvas.SetActive(true);

          
        
        
            ind++;
        if (ind < 2)
            GameData.SaveCompletedLevel(ind);
        
        GameData.LoadGameData();
        Debug.Log($"SelectedLevelIndex is {GameData.SelectedPlayerIndex} and  ind is {ind}");
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneWithProgress());
    }
    private IEnumerator LoadSceneWithProgress()
    {
        // Wait for the specified splash screen delay
        yield return new WaitForSeconds(waitTime);

        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneHandler.GameplayScene);

        // Don't allow the scene to activate immediately
        asyncOperation.allowSceneActivation = false;

        // Variable to track the simulated progress
        float simulatedProgress = 0f;

        // Gradually update the progress bar and bullet movement over the set loading time
        while (!asyncOperation.isDone)
        {
            // Increase the simulated progress over time (based on loadingTime)
            simulatedProgress += Time.deltaTime / loadingTime;

            // Ensure progress is capped at 1 (100%)
            simulatedProgress = Mathf.Clamp01(simulatedProgress);

            // Update the progress bar fill amount and progress text
            progressBar.fillAmount = simulatedProgress;          

            // Move the bullet to match the edge of the filled area
            UpdateBulletPosition(simulatedProgress - offset);

            // Once the scene is almost loaded (90%), allow activation
            if (simulatedProgress >= 1f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void UpdateBulletPosition(float progress)
    {
        // Get the width of the progress bar in world units
        float barWidth = progressBarRect.rect.width * progressBarRect.lossyScale.x;

        // Calculate the world position of the bullet
        float bulletWorldX = progressBarRect.position.x - (barWidth / 2) + (progress * barWidth);

        // Add a small offset to decrease the distance between the bullet and the filled bar
        float offset = -5f; // Adjust this value as needed (negative values bring the bullet closer)

        // Update the bullet's position, maintaining its original Y and Z coordinates
        bulletImage.position = new Vector3(bulletWorldX + offset, bulletImage.position.y, bulletImage.position.z);
    }

    
    public void OnPauseButtonClick()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }
    public void OnResumeButtonClick()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }
    /*  public void SetPlayerPosition()
      {
          players[GameData.SelectedPlayerIndex].gameObject.transform.localPosition= GetPlayerPosition.playerPostion;
          players[GameData.SelectedPlayerIndex].gameObject.transform.localRotation = Quaternion.Euler(GetPlayerPosition.playerRotation);
          players[GameData.SelectedPlayerIndex].gameObject.SetActive(true) ;
      }*/
}
