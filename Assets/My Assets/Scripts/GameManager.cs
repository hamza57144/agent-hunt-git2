using CoverShooter;
using JetBrains.Annotations;
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

    [SerializeField] GameObject environment;
    public static GameManager instance { get; private set; }
    [SerializeField] int levelCompleteReward;
    public LevelManager levelManager;
    [SerializeField] GameObject aimTutorial;
    [SerializeField] GameObject switchWeaponTutorial;
    [SerializeField] GameObject fireButton;
    public List<CharacterMotor> players;
    public CharacterMotor Player { get { return players[GameData.SelectedPlayerIndex]; } }
    [SerializeField] EnemyDisplayManager enemyDisplayManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] ThirdPersonCamera thirdPersonCamera;
    [SerializeField] GameObject levelCompleteCanvas;   
    [SerializeField] Text levelCompleteCashText;
    [SerializeField] GameObject levelFailCanvas;
    [SerializeField] Text levelFailedCashText;
    [SerializeField] GameObject[] controllerButtons;
    int ind;
    private int headShot;
    private int bodyShot;
    private float accuracy;
    [SerializeField] TextMeshProUGUI headShotText;
    [SerializeField] TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI healthText;
    
    [SerializeField] GameObject PausePanel;
    [SerializeField] Image healthBarFill;
  

    private int enemies;    
    private int totalShots { get {  return headShot+bodyShot; } }
    public int amo;
    [SerializeField] TextMeshProUGUI bulletsText;
    private AudioManager audioManager;
    [SerializeField] BossLevelProgress bossLevelProgress;
    [SerializeField] WeaponFiller weaponFiller;
    [SerializeField] WeaponsData weaponsData;
    [SerializeField] StatgesData stagesData;
    [SerializeField] GameObject bossLevelThings;
    
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

    public bool IsBossLevel()
    {
        return stagesData.IsBossLevel(GameData.CompletedLevelIndex);
    }

    void Awake()
    {
        Time.timeScale = 1f;     
       
        ind = GameData.CompletedLevelIndex;
       
        Player.gameObject.SetActive(true);
        instance = this;
        bossLevelThings.SetActive(stagesData.IsBossLevel(GameData.CompletedLevelIndex));
    }
    private void Start()
    {
        environment.SetActive(true );
        EnableTutorial(aimTutorial, GameData.CompletedLevelIndex == 0);       
        CharacterMotor.OnPlayerDie += CharacterMotor_OnPlayerDie;
        BodyPartHealth.OnBodyShot += BodyPartHealth_OnBodyShot;
        BodyPartHealth.OnHeadShot += BodyPartHealth_OnHeadShot;
        ThirdPersonInput.ButtonDown += ThirdPersonInput_ButtonDown;
        ThirdPersonInput.ButtonUp += ThirdPersonInput_ButtonUp;
        ThirdPersonInput.SwitchWeaponButtonClicked += ThirdPersonInput_SwitchWeaponButtonClicked;
        enemies = EnemyManager.instance.enemyCount;
        progressBarRect = progressBar.GetComponent<RectTransform>();
        amo = levelManager.GetLevel.bullets;
        audioManager = AudioManager.Instane;


    }

    private void ThirdPersonInput_SwitchWeaponButtonClicked(object sender, EventArgs e)
    {
        if (GameData.CompletedLevelIndex == 2)
        {
            EnableTutorial(switchWeaponTutorial,false);
            EnableTutorial(fireButton, true);


        }
    }

    private void Update()
    {
        
        if (Player != null && healthBarFill != null)
        {
            healthBarFill.fillAmount = Player.GetHealth / 500f; 
        }
        Amo();
        
    }
    public void EnableTutorial(GameObject tutorial,bool enable)
    {
        tutorial.SetActive(enable);
    } 
    public void EnableSwitchWeaponTutorial()
    {
        EnableTutorial(switchWeaponTutorial,true);
        EnableTutorial(fireButton, false);
    }
    void ShowCash(Text text)
    {
        text.text = GameData.Coins.ToString();
    }
    private void ThirdPersonInput_ButtonUp(object sender, EventArgs e)
    {
        amo--;
        ShowControllerButtons();
    }

    private void ThirdPersonInput_ButtonDown(object sender, EventArgs e)
    {
      
        if (GameData.CompletedLevelIndex == 0)
        {
            EnableTutorial(aimTutorial, false);
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
    public  void EnableGameCompleteCanvas()
    {
        levelCompleteCanvas.SetActive(true);
        ind++;
        GameData.SaveCompletedLevel(ind);
        bossLevelProgress.UpdateBossProgressBar((ind - 1)%7);//Boss level come after each 7 levels
        /*if (GameData.CompletedLevelIndex == 3)
        {
            GameData.SaveCompletedLevel(0);
        }*/
    }
    // Start is called before the first frame update
    public void LevelComplete()
    {
        GameData.showBossProgress = true;
        GameData.AddCoins(500);
        DisableCrossHair(false);
        if (!weaponsData.AreAllWeaponsUnlocked(Items.pistols))
        {
            weaponFiller.OnLevelComplete();
        }
        else
        {
            EnableGameCompleteCanvas();
        }       
        gameCanvas.SetActive(false);       
        ShowCash(levelCompleteCashText);
        audioManager.PlayBgMusic(audioManager.isMusicOn);
        headShotText.text = $"{headShot}/{enemies}";        
        healthText.text = ((Player.GetHealth)/5).ToString("0")+"%";
        accuracyText.text =$"{CalculateAccuracy()}%";     
        

        /*if (GameData.CompletedLevelIndex <= 5)
        {
            for(int i = 0; i < GameData.CompletedLevelIndex; i++)
            {
                Debug.Log($"i is {i} and levelCompleteIndex is {GameData.CompletedLevelIndex}");
                bossProgressImages[i].color = Color.green;
            }
        }*/

        
    }  

    public void LevelFailed()
    {
        Time.timeScale = 0f;
        gameCanvas.SetActive(false);
        ShowCash(levelFailedCashText);
        audioManager.PlayBgMusic(audioManager.isMusicOn);
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
        StartCoroutine(LoadSceneWithProgress(SceneHandler.GameplayScene));
    }
    public void ResumeGame()
    {
        levelCompleteCanvas.SetActive(false);
        loadingCanvas.SetActive(true);
        GameData.SaveCompletedLevel(ind-1);
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneWithProgress(SceneHandler.GameplayScene));
    }
    public void OnNextButtonClicked()
    {
        if (GameData.CompletedLevelIndex == levelManager.totalLevels)
        {
            GameData.SaveCompletedLevel(0);
        }
        DisableCrossHair(false);
        levelCompleteCanvas.SetActive(false );
        loadingCanvas.SetActive(true);           
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneWithProgress(SceneHandler.MainMenu));
    }
    public void OnHomeBtnClick()
    {
        DisableCrossHair(false);
        levelCompleteCanvas.SetActive(false);
        loadingCanvas.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneWithProgress(SceneHandler.MainMenu));
    }
    private IEnumerator LoadSceneWithProgress(string levelName)
    {
        // Wait for the specified splash screen delay
        yield return new WaitForSeconds(waitTime);

        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName);

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

    private void OnDisable()
    {
        CharacterMotor.OnPlayerDie -= CharacterMotor_OnPlayerDie;
        BodyPartHealth.OnBodyShot -= BodyPartHealth_OnBodyShot;
        BodyPartHealth.OnHeadShot -= BodyPartHealth_OnHeadShot;
        ThirdPersonInput.ButtonDown -= ThirdPersonInput_ButtonDown;
        ThirdPersonInput.ButtonUp -= ThirdPersonInput_ButtonUp;
        ThirdPersonInput.SwitchWeaponButtonClicked -= ThirdPersonInput_SwitchWeaponButtonClicked;
    }
    public void OnPauseButtonClick()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        audioManager.PlayBgMusic(audioManager.isMusicOn);
    }
    public void OnResumeButtonClick()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        audioManager.PlayBgMusic(false);
    }
    private void Amo()
    {
        
          //  Player.gameObject.GetComponent<ThirdPersonInput>().enabled = !(amo<=0);
           bulletsText.text = $"{amo.ToString()}/{levelManager.GetLevel.bullets}";
        
    }
    void DisableCrossHair(bool enable)
    {
        thirdPersonCamera.gameObject.GetComponent<Crosshair>().enabled = enable;
    }
    public void OnClaimWeaponButtonClick()
    {
        weaponsData.UnlockWeaon(GameData.Unlocked_Pistol_Index+1, Items.pistols);
        GameData.SaveUnlocked_Pistol(GameData.Unlocked_Pistol_Index + 1);
        GameData.SaveSelectedPistol(GameData.Unlocked_Pistol_Index + 1);
        weaponFiller.OnLoseItButtonClick();
    }
    /*  public void SetPlayerPosition()
      {
          players[GameData.SelectedPlayerIndex].gameObject.transform.localPosition= GetPlayerPosition.playerPostion;
          players[GameData.SelectedPlayerIndex].gameObject.transform.localRotation = Quaternion.Euler(GetPlayerPosition.playerRotation);
          players[GameData.SelectedPlayerIndex].gameObject.SetActive(true) ;
      }*/
}
