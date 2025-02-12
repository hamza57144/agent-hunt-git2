using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using CoverShooter.AI;
using System.Runtime.InteropServices;
using System;
using System.Reflection;


public enum Items
{
    player,
    pistols,
    snipers,
}
public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }
    #region Main Menu Panels

    [Header("Main Menu Panels"), Space(25)]
    [SerializeField] Text levelNo;
    [SerializeField] GameObject mainMenu;
    [SerializeField] List<GameObject> MainMenuPlayers;
    [SerializeField] GameObject playerSelectionPanel;
    public GameObject weaponsSelectionPanel;
    [SerializeField] GameObject levelSelectionPanel;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Text mainMenuPanelCashText;
    [SerializeField] GameObject inAppCanvas;
    [SerializeField] Text InAppPanelCashText;
    [SerializeField] BossLevelProgress bossLevelProgress;
    [SerializeField] GameObject bossProgressPanel;
    [SerializeField] GameObject PlayersPanel;
    [SerializeField] Image mainMenuBg;
    [SerializeField] StatgesData stagesData;
    [SerializeField] Text locationText;
    [SerializeField] Text targetText;    
    #endregion

    #region Weapons Selection
    [Space(25)]
    [Hamza(1, 1, 1, 7)]
    [Header("Weapon Selection"), Space(25)]

    [SerializeField] List<GameObject> pistols;
    [SerializeField] List<GameObject> snipers;
    [SerializeField] GameObject sniperPanel;
    [SerializeField] GameObject pistolPanel;
    bool isPistolPanelOpened = true;
    public WeaponsData weaponData;
    private WeaponsData.Weapon currentWeapon;
    [SerializeField] TextMeshProUGUI weaponhealthText;
    [SerializeField] TextMeshProUGUI weaponHidingText;
    [SerializeField] TextMeshProUGUI weaponReloadText;
    [SerializeField] Image weaponHealthBar;
    [SerializeField] Image weaponHidingBar;
    [SerializeField] Image weaponReloadBar;
    [SerializeField] List<Button> sniperButtons;
    [SerializeField] List<Button> pistolButtons;    
    [SerializeField] List<Sprite> pistolLockedSprites;
    [SerializeField] List<Sprite> pistolUnlockedSprites;
    [SerializeField] List<Sprite> sniperLockedSprites;
    [SerializeField] List<Sprite> sniperUnlockedSprites;
    [SerializeField] GameObject weaponAnimator;
    [SerializeField] Button WeaponUnlockedBtn;
    [SerializeField] Button weaponLockedBtn;
    [SerializeField] Text weaponPrice;
    private GameObject currentWeaponInstance;
    public Transform weaponSpawnPoint;
    [SerializeField] Sprite weaponSelectedSprite;
    [SerializeField] Sprite weaponDefualtSprite;
    [SerializeField] Image pistolBtn;
    [SerializeField] Image sniperBtn;
    [SerializeField] List<Image> pistolsLockImgages;
    [SerializeField] List<Image> sniperLockImages;
    [SerializeField] Text weaponPanelCashText;
    public static Transform weapon { get; set; }
    /*    public RawImage weaponDisplay; // Raw Image for displaying the Render Texture
        public Transform weaponDisplayParent; // Parent object for spawning weapon prefabs*/
    #endregion

    #region Player Selection
    [Space(25)]
    [Hamza(1, 1, 1, 7)]
    [Header("Player Selection"), Space(25)]


    [SerializeField] PlayerData playerData;
    [SerializeField] List<GameObject> players;
    [SerializeField] List<Button> playerButtons;
    [SerializeField] List<Sprite> PlayerUnlockedSprites;
    [SerializeField] List<Sprite> PlayerLockedSprites;
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] TextMeshProUGUI playerHidingText;
    [SerializeField] TextMeshProUGUI playerReloadText;
    [SerializeField] Image playerHealthBar;
    [SerializeField] Image playerHidingBar;
    [SerializeField] Image playerReloadBar;
    [SerializeField] Button buyAgentButtonSelected;
    [SerializeField] Button buyAgentButtonLocked;
    [SerializeField] Text playerPriceText;
    PlayerData.Player currentPlayer;
    [SerializeField] GameObject NotEnoughCoins;
    [SerializeField] Text playerSelectionPanelCashText;
    private int currentPlayerIndex;
    [SerializeField] List<Image> playerLockImages;
    [SerializeField] Vector3 lockedTextPosition;
    [SerializeField] Vector3 unLockedTextPosition;

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
    [Space(25)]
    [Hamza(1, 1, 1, 7)]
    [Header("Loading Screen"), Space(25)]
    public Image progressBar;
    public RectTransform bulletImage;
    private float offset = 0.019f;
    private RectTransform progressBarRect;
    // Time to wait before starting to load the scene (e.g., splash screen delay)
    public float waitTime = 3f;

    // Total loading time for the scene (how long it will take to fill the progress bar)
    public float loadingTime = 5f;
    #endregion
    private AudioManager audioManager;
    
    private void Awake()
    {
        Instance = this;
        ShowBossProgress(GameData.showBossProgress);
        /*weaponData = pistolsData;*/
        CurrentWeaponindex = GameData.Selected_Pistol_Index;
        currentPlayerIndex = GameData.SelectedPlayerIndex;
        UnlockCursor();
        LevelButtonActivation();
        DisplayWeapons(GameData.Selected_Pistol_Index, pistols, Items.pistols);

        DisplayPlayer(GameData.SelectedPlayerIndex);
        //PlayerButtonsLockUnlock();

    }

    private void ShowBossProgress(bool show)
    {
        bossProgressPanel.SetActive(show);
        PlayersPanel.SetActive(!show);
        mainMenuBg.enabled = show;


    }
    public void AnimationChanger(List<GameObject> animators, int idx)
    {

        for (int i = 0; i < animators.Count; i++)
        {
            animators[i].GetComponent<Animator>().SetInteger(AnimationHandler.MainMenuPlayerAnimation, idx);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            AnimationChanger(players, idx);
            AnimationChanger(MainMenuPlayers, idx);
            ResetIdx();
        }
    }
    int idx = 1;
    public int totalAnimations;
    private void ResetIdx()
    {
        idx++;
        if (idx > totalAnimations) { idx = 1; }
    }
    void UnlockCursor()
    {
        //If cursor get lock due to scene loading,unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Start()
    {
        bossLevelProgress.UpdateBossProgressBar(GameData.CompletedLevelIndex -1);
        audioManager = AudioManager.Instane;
        audioManager.PlayBgMusic(audioManager.isMusicOn);
        // Players.gameObject.transform.position = position2.transform.position;
        DisplayMainMenu();
        playerSelectionPanel.SetActive(false);
        weaponsSelectionPanel.SetActive(false);
        levelSelectionPanel.SetActive(false);
        progressBarRect = progressBar.GetComponent<RectTransform>();
        SpriteChanger(pistolBtn, sniperBtn, weaponSelectedSprite, weaponDefualtSprite);
        MissionDetails(GameData.CompletedLevelIndex);
       /* GameData.CompletedLevelIndex==*/


        /*foreach (var button in backButtons)
        {
            if (button.backButton != null)
            {
                button.backButton.onClick.AddListener(() => SwitchPanels(button.panelToDisable));
            }
        }*/

    }
    void DisplayMainMenu()
    {
        ShowCash(mainMenuPanelCashText);
        mainMenu.SetActive(true);
        EnableItem(MainMenuPlayers, GameData.SelectedPlayerIndex);
      


    } 
    private void MissionDetails(int index)
    {
        levelNo.text = $"Mission:{stagesData.GetStage(index).levelNo}";
        locationText.text = $"{stagesData.GetStage(index).location}";
        targetText.text = $"{stagesData.GetStage(index).totalEnemies}";
    }
    public void OnSelectPlayerBtnClick()
    {
        ShowCash(playerSelectionPanelCashText);
       
        mainMenu.SetActive(false);
        playerSelectionPanel.SetActive(true);
    }

    private void ShowCash(Text cashText)
    {
        cashText.text = GameData.Coins.ToString();
    }
    public void OnSelectWeaponBtnClick()
    {
        ShowCash(weaponPanelCashText);
        mainMenu.SetActive(false);
        weaponsSelectionPanel.SetActive(true);
        // Players.gameObject.transform.position = position1.transform.position;
    }
    public void OnPlayBtnClick()
    {
        audioManager.PlayButtonClickSound();
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
        if (!IsItemLocked(index, Items.player))
        {
            GameData.SaveSelectedPlayer(index);
        }
        currentPlayerIndex = index;
        DisplayPlayer(currentPlayerIndex);
        audioManager.PlayButtonClickSound();

    }
   
    public void SelectWeapon(int index)
    {
        CurrentWeaponindex = index;
        if (isPistolPanelOpened)
            DisplayWeapons(index, pistols, Items.pistols);
        else
            DisplayWeapons(index, snipers, Items.snipers);
        audioManager.PlayButtonClickSound();

    }
    public void OnPistolButtonClick()
    {
        currentWeapon = weaponData.GetWeapon(GameData.Selected_Pistol_Index, Items.pistols);
        // weaponData = pistolsData;
        SpriteChanger(pistolBtn, sniperBtn, weaponSelectedSprite, weaponDefualtSprite);
        isPistolPanelOpened = true;
        AudioManager.Instane.PlaySound(SoundName.ButtonClick);
        sniperPanel.SetActive(false);
        pistolPanel.SetActive(true);
        DisplayWeapons(GameData.Selected_Pistol_Index, pistols, Items.pistols);
    }
    public void OnSniperButtonClick()
    {
        currentWeapon = weaponData.GetWeapon(GameData.Selected_Gun_Index, Items.snipers);
        //  weaponData = snipersData;
        SpriteChanger(sniperBtn, pistolBtn, weaponSelectedSprite, weaponDefualtSprite);
        isPistolPanelOpened = false;
        AudioManager.Instane.PlaySound(SoundName.ButtonClick);
        pistolPanel.SetActive(false);
        sniperPanel.SetActive(true);
        DisplayWeapons(GameData.Selected_Gun_Index, snipers, Items.snipers);
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
        /*  if (CurrentWeaponindex % 2 != 0)
              GameData.SaveSelectedWeapon_Pistol(CurrentWeaponindex);
          else
              GameData.SaveSelectedWeapon_Gun(CurrentWeaponindex);*/
    }
    public void OnUpgragePlayerButtonClick()
    {
        /* GameData.SaveUnlockedPlayer(currentPlayerIndex);*/
    }
    private void LevelButtonActivation()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i <= GameData.CompletedLevelIndex)
            {

                levelButtons[i].interactable = true;
            }

        }
    }
    private void SpriteChanger(List<Button> buttons, List<Sprite> unlockedSprites, List<Sprite> lockedSprites, Items item,int index=0)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            if ((item==Items.player))
            {
                if (!IsItemLocked(i, item))
                {

                    buttonImage.sprite = unlockedSprites[i];
                    buttonImage.SetNativeSize();

                }
                else
                {

                    buttonImage.sprite = lockedSprites[i];
                    buttonImage.SetNativeSize();

                }
            }
            else
            {
                if (i == index)
                {

                    buttonImage.sprite = unlockedSprites[i];
                    buttonImage.SetNativeSize();

                }
                else
                {

                    buttonImage.sprite = lockedSprites[i];
                    buttonImage.SetNativeSize();

                }
            }
            
        }
    }
  /*  private void SpriteChanger(List<Button> buttons, List<Sprite> unlockedSprites, List<Sprite> lockedSprites, Items item,int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            if (i==index)
            {

                buttonImage.sprite = unlockedSprites[i];
                buttonImage.SetNativeSize();

            }
            else
            {

                buttonImage.sprite = lockedSprites[i];
                buttonImage.SetNativeSize();

            }
        }
    }*/
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
    private bool IsItemLocked(int index, Items item)
    {
        switch (item)
        {
            case Items.player:

                return playerData.IsPlayerLocked(index);

            case Items.snipers:
                return weaponData.IsWeaponLocked(index, item);

            case Items.pistols:
                return weaponData.IsWeaponLocked(index, item);

            default:
                return true;
        }
    }

    private void ActivateWeapon()
    {
        weaponAnimator.SetActive(true);
        weaponAnimator.GetComponent<Animator>().SetTrigger("Rotate");
    }

    public void DisplayWeapons(int index, Items item)
    {
        if (item == Items.pistols)
            DisplayWeapons(index, pistols, item);
        else
            DisplayWeapons(index, snipers, item);
    }

    private void SetTextPosition(List<Button> buttons,Items item)
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            Text nameText = buttons[i].GetComponentInChildren<Text>();
          
            if (!IsItemLocked(i, item))
            {              
                nameText.gameObject.transform.localPosition = unLockedTextPosition;
            }
            else
            {
                nameText.gameObject.transform.localPosition=lockedTextPosition;
            }
            
        }
    }
    private void DisplayLockIcon(List<Image> lockImages,Items item)
    {
        for(int i = 0; i < lockImages.Count; i++)
        {
            if(IsItemLocked(i, item))
            {
                lockImages[i].gameObject.SetActive(true);
            }
            else
            {
                lockImages[i].gameObject.SetActive(false);
            }
        } 
    }
    private void DisplayWeapons(int index, List<GameObject> weapons, Items item)
    {
        DisplayLockIcon(pistolsLockImgages, Items.pistols);
        DisplayLockIcon(sniperLockImages, Items.snipers);
        currentWeapon = weaponData.GetWeapon(index, item);

        ButtonChanger(index, item, weaponLockedBtn, WeaponUnlockedBtn);           
        if (item == Items.snipers)
        {
            SpriteChanger(sniperButtons, sniperUnlockedSprites, sniperLockedSprites, item, index);
        }
        else if (item == Items.pistols)
        {
            SpriteChanger(pistolButtons, pistolUnlockedSprites, pistolLockedSprites, item, index);
        }
        //  currentWeapon = weaponData.GetWeapon(index, WeaponType.pistol);        
        //   weaponAnimator.SetActive(false);
        Invoke(nameof(ActivateWeapon), 0.25f);
        EnableItem(weapons, index);
        weapon = weapons[index].transform;

        /* // Instantiate the new weapon prefab at the spawn point's position and rotation
         currentWeaponInstance = Instantiate(currentWeapon.weaponPrefab, weaponSpawnPoint.position, weaponSpawnPoint.rotation);

         // Set the new weapon as a child of the spawn point
         currentWeaponInstance.transform.SetParent(weaponSpawnPoint);

         // Reset the local position, rotation, and scale to ensure it's aligned with the spawn point
         currentWeaponInstance.transform.localPosition = Vector3.zero;
         currentWeaponInstance.transform.localRotation = Quaternion.identity;
         currentWeaponInstance.transform.localScale = Vector3.one;*/
       
        UpdateItemStatsUI(weaponPrice, weaponhealthText, weaponHidingText, weaponReloadText, weaponHealthBar, weaponHidingBar, weaponReloadBar, item, currentWeapon);


    }

    private void UpdateItemStatsUI(Text priceText,TextMeshProUGUI healthText, TextMeshProUGUI hidingText, TextMeshProUGUI reloadingText, Image healthBar, Image hidingBar, Image reloadingBar,Items item,WeaponsData.Weapon weapon=null,PlayerData.Player player=null )
    {
        if(item == Items.player)
        {
            priceText.text = $"{player.price}";
            healthText.text = $"{player.health}%";
            hidingText.text = $"{player.hiding}%";
            reloadingText.text = $"{player.reloadTime}%";

            healthBar.fillAmount = Mathf.Clamp01(player.health / 100f);
            hidingBar.fillAmount = Mathf.Clamp01(player.hiding / 100f);
            reloadingBar.fillAmount = Mathf.Clamp01(player.reloadTime / 100f);
        }
       else
        {
            priceText.text = $"{weapon.price}";
            healthText.text = $"{weapon.health}%";
            hidingText.text = $"{weapon.hiding}%";
            reloadingText.text = $"{weapon.reloadTime}%";

            healthBar.fillAmount = Mathf.Clamp01(weapon.health / 100f);
            hidingBar.fillAmount = Mathf.Clamp01(weapon.hiding / 100f);
            reloadingBar.fillAmount = Mathf.Clamp01(weapon.reloadTime / 100f);
        }
       
    }
    void EnableItem(List<GameObject> items, int index)
    {
        Debug.Log("Index of item is " + index);
        items[index].SetActive(true);
        for (int i = 0; i < items.Count; i++)
        {
            if (i != index)
            {

                items[i].SetActive(false);
            }

        }
    }
    public void DisplayPlayer(int index)
    {
        /*  if (IsItemLocked(index,Items.player))
          {
              buyAgentButtonSelected.interactable = false;
          }
          else
          {
              buyAgentButtonSelected.interactable = true;
          }*/

        ButtonChanger(index, Items.player, buyAgentButtonLocked, buyAgentButtonSelected);
        SpriteChanger(playerButtons, PlayerUnlockedSprites, PlayerLockedSprites, Items.player);
        DisplayLockIcon(playerLockImages, Items.player);
        SetTextPosition(playerButtons,Items.player);
        /* PlayerData.Player currentPlayer = playerData.playerList[index];*/
        currentPlayer = playerData.GetPlayer(index);
        EnableItem(players, index);
        UpdateItemStatsUI(playerPriceText, playerHealthText, playerHidingText, playerReloadText, playerHealthBar, playerHidingBar, playerReloadBar, Items.player,null, currentPlayer);

    }
    public void OnBuyPlayerButtonClick()
    {
        audioManager.PlayButtonClickSound();
        if (currentPlayer != null)
        {

            if (GameData.Coins >= currentPlayer.price)
            {
                GameData.SpendCoins(currentPlayer.price);
                playerData.UnlockPlayer(currentPlayerIndex);
                GameData.SaveUnlockedPlayer(currentPlayerIndex);
                GameData.SaveSelectedPlayer(currentPlayerIndex);
                DisplayPlayer(currentPlayerIndex);
            }
            else
            {
                Debug.Log("You don't have enough coins");
                NotEnoughCoins.SetActive(true);
            }
        }
    }
    public void OnBuyWeaponButtonClick()
    {
        audioManager.PlayButtonClickSound();
        if (currentWeapon != null)
        {

            if (GameData.Coins >= currentWeapon.price)
            {
                GameData.SpendCoins(currentWeapon.price);
                if (currentWeapon.isHeavy)
                {
                    weaponData.UnlockWeaon(CurrentWeaponindex, Items.snipers);
                    GameData.SaveUnlocked_Gun(CurrentWeaponindex);
                    GameData.SaveSelected_Gun(CurrentWeaponindex);

                    DisplayWeapons(CurrentWeaponindex, snipers, Items.snipers);
                }
                else
                {
                    weaponData.UnlockWeaon(CurrentWeaponindex, Items.pistols);
                    GameData.SaveUnlocked_Pistol(CurrentWeaponindex);
                    GameData.SaveSelectedPistol(CurrentWeaponindex);
                    DisplayWeapons(CurrentWeaponindex, pistols, Items.pistols);
                }


            }
            else
            {
                Debug.Log("You don't have enough coins");
                NotEnoughCoins.SetActive(true);
            }
        }
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
        audioManager.PlayButtonClickSound();
    }
    public void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        audioManager.PlayButtonClickSound();
        UnityEditor.EditorApplication.isPlaying = false;
#else
            
            Application.Quit();
#endif
    }
    public void OpenMoreGames()
    {
        audioManager.PlayButtonClickSound();
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.topgamesinc.evony&pcampaignid=merch_published_cluster_promotion_battlestar_browse_all_games"); // Open the URL
    }
    public void RateGame()
    {
        audioManager.PlayButtonClickSound();
#if UNITY_ANDROID
        // Open the Play Store page for your app (use your actual package name)
        Application.OpenURL(playStoreURL);
#else
            Debug.Log("Rate Us is only available on Android platform.");
#endif
    }
    private void ButtonChanger(int index, Items item, Button LockedBtn, Button UnlockedBtn)
    {
        UnlockedBtn.gameObject.SetActive(!(IsItemLocked(index, item)));
        LockedBtn.gameObject.SetActive(IsItemLocked(index, item));

    }
    private void SpriteChanger(Image ThisImage, Image OtherImage, Sprite ThisSprites, Sprite OhterSprite)
    {
        ThisImage.sprite = ThisSprites;
        OtherImage.sprite = OhterSprite;
    }
    public void  OnStoreButtonClick(GameObject panel)
    {
        inAppCanvas.SetActive(true);
        panel.SetActive(false);
        audioManager.PlayButtonClickSound();
        ShowCash(InAppPanelCashText);
       
    }
    
    public void OnBackButtonClick(GameObject panel)
    {
        audioManager.PlayButtonClickSound();
        panel.SetActive(false);
        DisplayMainMenu();
    }
    
}
