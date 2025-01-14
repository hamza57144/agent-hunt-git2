using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public enum Items
{
    player,
    pistols,
    snipers,
}
public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] List<GameObject> MainMenuPlayers;
    [SerializeField] GameObject playerSelectionPanel;
    [SerializeField] GameObject weaponsSelectionPanel;
    [SerializeField] GameObject levelSelectionPanel;
    [SerializeField] GameObject loadingScreen;
    public RectTransform bulletImage;
    public static Transform weapon { get; set; }
   
    [Header("Weapon Selection")]
    #region Weapons Selection
    [SerializeField] List<GameObject> pistols;
    [SerializeField] List<GameObject> snipers;
    [SerializeField] GameObject sniperPanel;
    [SerializeField] GameObject pistolPanel;
    bool isPistolPanelOpened;
    [SerializeField] WeaponData weaponData;
    [SerializeField] TextMeshProUGUI weaponhealthText;     
    [SerializeField] TextMeshProUGUI weaponHidingText;
    [SerializeField] TextMeshProUGUI weaponReloadText;
    [SerializeField] Image healthBar;        
    [SerializeField] Image hidingBar;         
    [SerializeField] Image reloadBar;
    [SerializeField] List<Button> sniperButtons;
    [SerializeField] List<Button> pistolButtons;
    [SerializeField] Sprite pistolLockedSprite;
    [SerializeField] Sprite pistolUnlockedSprite;
    [SerializeField] Sprite sniperLockedSprite;
    [SerializeField] Sprite sniperUnlockedSprite;
    [SerializeField] GameObject weaponAnimator;
    private GameObject currentWeaponInstance;
    public Transform weaponSpawnPoint;
    /*    public RawImage weaponDisplay; // Raw Image for displaying the Render Texture
        public Transform weaponDisplayParent; // Parent object for spawning weapon prefabs*/
    #endregion
    [Header("Player Selection")]
    #region Player Selection
    [Tooltip("Player Selection")]
    private int currentPlayerIndex;
    [SerializeField] PlayerData playerData;
    [SerializeField] List<GameObject> players;
    [SerializeField] List<Button> playerButtons;
    [SerializeField] Sprite PlayerUnlockedSprite;
    [SerializeField] Sprite PlayerLockedSprite;
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] TextMeshProUGUI playerHidingText;
    [SerializeField] TextMeshProUGUI playerReloadText;
    [SerializeField] Image playerHealthBar;
    [SerializeField] Image playerHidingBar;
    [SerializeField] Image playerReloadBar;
    [SerializeField] Button UpgradePlayerButton;

    public string playStoreURL = "https://play.google.com/store/apps/details?id=com.topgamesinc.evony&pcampaignid=merch_published_cluster_promotion_battlestar_browse_all_games";
    /*[SerializeField] List<Button> gunButtons;
    [SerializeField] GameObject weaponAnimator;*/

    #endregion
    public List<Button> levelButtons;
    public static int CurrentWeaponindex { get; private set; }
    #region Reminder
    /// <summary>
    /// it's not a best approach for loading scene, temporary using this approach for loading gameplay,
    /// will replace this later and manage scene loading by static classes
    /// and may be a separate scene for loading scene  if needed
    /// </summary>
    #endregion
    #region Loading Bar
    public Image progressBar;   
    private float offset = 0.019f;
    private RectTransform progressBarRect;
    // Time to wait before starting to load the scene (e.g., splash screen delay)
    public float waitTime = 3f;

    // Total loading time for the scene (how long it will take to fill the progress bar)
    public float loadingTime = 5f;
    #endregion

    private void Awake()
    {
        CurrentWeaponindex = GameData.SelectedWeapon_Pistol_Index;
        currentPlayerIndex  = GameData.UnlockedPlayerIndex;
        GameData.LoadGameData();
        UnlockCursor();
        LevelButtonActivation();
        DisplayWeapons(CurrentWeaponindex,pistols,Items.pistols);
        DisplayPlayer(GameData.SelectedPlayerIndex);
        //PlayerButtonsLockUnlock();

    }
   
       
   
    void UnlockCursor()
    {
        //If cursor get lock due to scene loading,unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Start()
    {
        // Players.gameObject.transform.position = position2.transform.position;
        DisplayMainMenu();
        playerSelectionPanel.SetActive(false);
        weaponsSelectionPanel.SetActive(false);
        levelSelectionPanel.SetActive(false);
        progressBarRect = progressBar.GetComponent<RectTransform>();
    }
    void DisplayMainMenu()
    {
        mainMenu.SetActive(true);
        MainMenuPlayers[GameData.UnlockedPlayerIndex].SetActive(true);
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
       // Players.gameObject.transform.position = position1.transform.position;
    }
    public void OnPlayBtnClick()
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneWithProgress());
    }
    public void SelectPlayer(int index)
    {
        
        /*if (IsPlayerLocked(index))
        {
            GameData.SaveUnlockedPlayer(index);
        }*/
        currentPlayerIndex=index;
        DisplayPlayer(index);
       
       
    }
    /// <summary>
    /// For pistol buttons, give index odd and for snipers, assign even index
    /// </summary>
    /// <param name="index"></param>

    [Tooltip("For pistol buttons, give index odd and for snipers, assign even index")]
    public void SelectWeapon(int index)
    {
        CurrentWeaponindex = index;
        if(isPistolPanelOpened)
           DisplayWeapons(CurrentWeaponindex+1,pistols,Items.pistols);
        else
            DisplayWeapons(CurrentWeaponindex + 1, snipers, Items.snipers);

    }
    public void OnPistolButtonClick()
    {
        AudioManager.Instane.PlaySound(SoundName.ButtonClick);
        sniperPanel.SetActive(false);
        pistolPanel.SetActive(true);
        DisplayWeapons(GameData.SelectedWeapon_Pistol_Index, pistols, Items.pistols);
    }
    public void OnSniperButtonClick()
    {
        AudioManager.Instane.PlaySound(SoundName.ButtonClick);
        pistolPanel.SetActive(false);
        sniperPanel.SetActive(true);       
        DisplayWeapons(GameData.SelectedWeapon_Gun_Index, snipers, Items.snipers);
    }
    public void SelectLevel(int index)
    {
        levelSelectionPanel.SetActive(false);
        loadingScreen.SetActive(true);
        GameData.SaveSelectedLevel(index);
        StartCoroutine(LoadSceneWithProgress());
    }
    public void OnUpgrageWeaponButtonClick()
    {
        AudioManager.Instane.PlaySound(SoundName.ButtonClick);
        if (CurrentWeaponindex % 2 != 0)
            GameData.SaveSelectedWeapon_Pistol(CurrentWeaponindex);
        else
            GameData.SaveSelectedWeapon_Gun(CurrentWeaponindex);
    }
    public void OnUpgragePlayerButtonClick()
    {
        GameData.SaveUnlockedPlayer(currentPlayerIndex);
    }
    private void LevelButtonActivation()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if(i <= GameData.CompletedLevelIndex)
            {
               
                levelButtons[i].interactable = true;
            }
             
        }
    }
    private void SpriteChanger(List<Button> buttons,Sprite unlockedSprite,Sprite lockedSprite,Items item)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            if (!IsItemLocked(i,item)) 
            {
   
                buttonImage.sprite = unlockedSprite;
                buttonImage.SetNativeSize();

            }
            else
            {
               
                buttonImage.sprite = lockedSprite;
                buttonImage.SetNativeSize();

            }

        }
    }
   /* private void PlistolButtonsSpriteChange()
    {
        for (int i = 0; i < pistolButtons.Count; i++)
        {
            Image buttonImage = pistolButtons[i].GetComponent<Image>();
            if (!IsItemLocked(i,Items.pistols))
            {

                buttonImage.sprite = unlockedSprite;
                buttonImage.SetNativeSize();

            }
            else
            {

                buttonImage.sprite = lockedSprite;
                buttonImage.SetNativeSize();



            }

        }
    }*/
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
           /* progressText.text = Mathf.RoundToInt(simulatedProgress * 100) + "%";*/

            // Move the bullet to match the edge of the filled area
            UpdateBulletPosition(simulatedProgress - offset);

            // Once the scene is almost loaded (90%), allow activation
            if (simulatedProgress >= 0.9f)
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
    private bool IsItemLocked(int index,Items status)
    {
       if(status == Items.player)
            return GameData.UnlockedPlayerIndex < index;
       else if(status == Items.snipers)
            return GameData.SelectedWeapon_Gun_Index < index+2;
        else if(status == Items.pistols)
            return GameData.SelectedWeapon_Pistol_Index < index;
       return false;
    }
    private bool IsPistolLocked(int index)
    {
        return GameData.SelectedWeapon_Pistol_Index < index;
    }
    private bool IsSniperLocked(int index)
    {
        return GameData.SelectedWeapon_Gun_Index < index;
    }
    private void ActivateWeapon()
    {
        weaponAnimator.SetActive(true);
        weaponAnimator.GetComponent<Animator>().SetTrigger("Rotate");
    }
    private void DisplayWeapons(int index,List<GameObject> weapons,Items item)
    {
     //   if(IsPistolLocked(index)) 

        // Get the currently selected weapon
       /* if(item==Items.snipers)
        {
            SpriteChanger(sniperButtons,sniperUnlockedSprite,sniperLockedSprite,Items.pistols);
        }
        else if (item == Items.pistols)
        {
            SpriteChanger(sniperButtons, pistolUnlockedSprite, pistolLockedSprite, Items.snipers);
        }*/
        WeaponData.Weapon currentWeapon = weaponData.weaponList[index];
        weaponAnimator.SetActive(false);
        Invoke(nameof(ActivateWeapon), 0.25f);

        weapons[index].SetActive(true);
        weapon = weapons[index].transform;
        for (int i = 0; i < weapons.Count; i++)
        {
            if(i!=index)
            {
                weapons[i].SetActive(false);
            }
        }

        /* // Instantiate the new weapon prefab at the spawn point's position and rotation
         currentWeaponInstance = Instantiate(currentWeapon.weaponPrefab, weaponSpawnPoint.position, weaponSpawnPoint.rotation);

         // Set the new weapon as a child of the spawn point
         currentWeaponInstance.transform.SetParent(weaponSpawnPoint);

         // Reset the local position, rotation, and scale to ensure it's aligned with the spawn point
         currentWeaponInstance.transform.localPosition = Vector3.zero;
         currentWeaponInstance.transform.localRotation = Quaternion.identity;
         currentWeaponInstance.transform.localScale = Vector3.one;*/

        // Update the UI elements with the weapon's data
        weaponhealthText.text = currentWeapon.health.ToString() + "%";
        weaponHidingText.text = currentWeapon.hiding.ToString() + "%";
        weaponReloadText.text = currentWeapon.reloadTime.ToString() + "%";

        healthBar.fillAmount = Mathf.Clamp01(currentWeapon.health / 100f);
        hidingBar.fillAmount = Mathf.Clamp01(currentWeapon.hiding / 100f);
        reloadBar.fillAmount = Mathf.Clamp01(currentWeapon.reloadTime / 100f);

      
        
    }

    private void DisplayPlayer(int index)
    {
        if (IsItemLocked(index,Items.player))
        {
            UpgradePlayerButton.interactable = false;
        }
        else
        {
            UpgradePlayerButton.interactable = true;
        }
        SpriteChanger(playerButtons, PlayerUnlockedSprite,PlayerLockedSprite,Items.player);
        PlayerData.Player currentPlayer = playerData.playerList[index];
        players[index].SetActive(true);
        for (int i = 0; i < players.Count; i++)
        {
            if (i != index)
            {
                
                players[i].SetActive(false);
            }
            
        }

        // Update the UI elements with the weapon's data
        playerHealthText.text = currentPlayer.health.ToString() + "%";
        playerHidingText.text = currentPlayer.hiding.ToString() + "%";
        playerReloadText.text = currentPlayer.reloadTime.ToString() + "%";

        playerHealthBar.fillAmount = Mathf.Clamp01(currentPlayer.health / 100f);
        playerHidingBar.fillAmount = Mathf.Clamp01(currentPlayer.hiding / 100f);
        playerReloadBar.fillAmount = Mathf.Clamp01(currentPlayer.reloadTime / 100f);

    }
  /*  private void GunButtonActivation()
    {
        for (int i = 0; i < gunButtons.Count; i++)
        {
            if (i <= GameData.SelectedWeapon_Pistol_Index)
            {

                gunButtons[i].interactable = true;
            }

        }
    }*/
    public void OnPlayerSelectionBackButtonClick()
    {
        playerSelectionPanel.SetActive(false);
        DisplayMainMenu();
       
    }
    public void OnQuitButtonClick()
    {
#if UNITY_EDITOR
       
        UnityEditor.EditorApplication.isPlaying = false;
#else
            
            Application.Quit();
#endif
    }
    public void OpenMoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.topgamesinc.evony&pcampaignid=merch_published_cluster_promotion_battlestar_browse_all_games"); // Open the URL
    }
    public void RateGame()
    {
#if UNITY_ANDROID
        // Open the Play Store page for your app (use your actual package name)
        Application.OpenURL(playStoreURL);
#else
            Debug.Log("Rate Us is only available on Android platform.");
#endif
    }
}
