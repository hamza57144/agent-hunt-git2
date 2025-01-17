using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using CoverShooter.AI;
using System.Runtime.InteropServices;

public enum Items
{
    player,
    pistols,
    snipers,
}
public class MainMenuManager : MonoBehaviour
{

    #region Main Menu Panels

       [Header("Main Menu Panels"),Space(25)]
        [SerializeField] GameObject mainMenu;
        [SerializeField] List<GameObject> MainMenuPlayers;
        [SerializeField] GameObject playerSelectionPanel;
        [SerializeField] GameObject weaponsSelectionPanel;
        [SerializeField] GameObject levelSelectionPanel;
        [SerializeField] GameObject loadingScreen;
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
    [SerializeField] WeaponData pistolsData;
    [SerializeField] WeaponData snipersData;
    private WeaponData weaponData;
    private WeaponData.Weapon currentWeapon;
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
    [SerializeField] Button WeaponUnlockedBtn;
    [SerializeField] Button weaponLockedBtn;
    [SerializeField] Text weaponPrice;
    private GameObject currentWeaponInstance;
    public Transform weaponSpawnPoint;
    [SerializeField] Sprite weaponSelectedSprite;
    [SerializeField] Sprite weaponDefualtSprite;
    [SerializeField] Image pistolBtn;
    [SerializeField] Image sniperBtn;
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
    [SerializeField] Sprite PlayerUnlockedSprite;
    [SerializeField] Sprite PlayerLockedSprite;
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] TextMeshProUGUI playerHidingText;
    [SerializeField] TextMeshProUGUI playerReloadText;
    [SerializeField] Image playerHealthBar;
    [SerializeField] Image playerHidingBar;
    [SerializeField] Image playerReloadBar;
    [SerializeField] Button buyAgentButtonSelected;
    [SerializeField] Button buyAgentButtonLocked;
    [SerializeField] Text agentPriceText;
    PlayerData.Player currentPlayer;
    [SerializeField] GameObject NotEnoughCoins;
    [SerializeField] Text totalAmmountTex;
    private int currentPlayerIndex;

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
    [Header("Loading Screen"),Space(25)]
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
        weaponData = pistolsData;
        CurrentWeaponindex = GameData.Selected_Pistol_Index;
        currentPlayerIndex  = GameData.UnlockedPlayerIndex;        
        UnlockCursor();
        LevelButtonActivation();
        DisplayWeapons(CurrentWeaponindex,pistols,Items.pistols);
        DisplayPlayer(currentPlayerIndex);
        //PlayerButtonsLockUnlock();

    }
   
    public void AnimationChanger(List<GameObject> animators,int idx)
    {
        
        for (int i=0;i<animators.Count;i++)
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
        audioManager = AudioManager.Instane;
        audioManager.PlayBgMusic(audioManager.isMusicOn);
        // Players.gameObject.transform.position = position2.transform.position;
        DisplayMainMenu();
        playerSelectionPanel.SetActive(false);
        weaponsSelectionPanel.SetActive(false);
        levelSelectionPanel.SetActive(false);
        progressBarRect = progressBar.GetComponent<RectTransform>();
        SpriteChanger(pistolBtn, sniperBtn, weaponSelectedSprite, weaponDefualtSprite);
    }
    void DisplayMainMenu()
    {
        mainMenu.SetActive(true);
        EnableItem(MainMenuPlayers, GameData.SelectedPlayerIndex);

    }
    public void OnSelectPlayerBtnClick()
    {
        totalAmmountTex.text = GameData.Coins.ToString();
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
       if(!IsItemLocked(index,Items.player))
        {
            GameData.SaveSelectedPlayer(index);
        }
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
           DisplayWeapons(index, pistols,Items.pistols);
        else
            DisplayWeapons(index, snipers, Items.snipers);

    }
    public void OnPistolButtonClick()
    {
        weaponData = pistolsData;
        SpriteChanger(pistolBtn, sniperBtn, weaponSelectedSprite, weaponDefualtSprite);
        isPistolPanelOpened=true;
        AudioManager.Instane.PlaySound(SoundName.ButtonClick);
        sniperPanel.SetActive(false);
        pistolPanel.SetActive(true);
        DisplayWeapons(GameData.Selected_Pistol_Index, pistols, Items.pistols);
    }
    public void OnSniperButtonClick()
    {
        weaponData = snipersData;
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
    private bool IsItemLocked(int index,Items item)
    {
       if(item == Items.player)
            return GameData.UnlockedPlayerIndex < index;
       else if(item == Items.snipers)
            return GameData.Unlocked_Gun_Index < index;//+2 bcz defualt unlocked gun index is 2
        else if(item == Items.pistols)
            return GameData.Unlocked_Pistol_Index < index;//+1 bcz defualt unlocked pistol index is 2
        return false;
    }
   
    private void ActivateWeapon()
    {
        weaponAnimator.SetActive(true);
        weaponAnimator.GetComponent<Animator>().SetTrigger("Rotate");
    }
    private void DisplayWeapons(int index,List<GameObject> weapons,Items item)
    {
        currentPlayerIndex = index;
        ButtonChanger(index, item, weaponLockedBtn, WeaponUnlockedBtn);
        //   if(IsPistolLocked(index)) 

        // Get the currently selected weapon
        if (item == Items.snipers)
        {
            SpriteChanger(sniperButtons, sniperUnlockedSprite, sniperLockedSprite, Items.snipers);
        }
        else if (item == Items.pistols)
        {
            SpriteChanger(pistolButtons, pistolUnlockedSprite, pistolLockedSprite, Items.pistols);
        }
        currentWeapon = weaponData.GetWeapon(index);        
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

        // Update the UI elements with the weapon's data
        weaponPrice.text = currentWeapon.price.ToString();
        weaponhealthText.text = currentWeapon.health.ToString() + "%";
        weaponHidingText.text = currentWeapon.hiding.ToString() + "%";
        weaponReloadText.text = currentWeapon.reloadTime.ToString() + "%";

        healthBar.fillAmount = Mathf.Clamp01(currentWeapon.health / 100f);
        hidingBar.fillAmount = Mathf.Clamp01(currentWeapon.hiding / 100f);
        reloadBar.fillAmount = Mathf.Clamp01(currentWeapon.reloadTime / 100f);

      
        
    }
    void EnableItem(List<GameObject> items, int index)
    {
        items[index].SetActive(true);
        for (int i = 0; i < items.Count; i++)
        {
            if (i != index)
            {

                items[i].SetActive(false);
            }

        }
    }
    private void DisplayPlayer(int index)
    {
        /*  if (IsItemLocked(index,Items.player))
          {
              buyAgentButtonSelected.interactable = false;
          }
          else
          {
              buyAgentButtonSelected.interactable = true;
          }*/
        ButtonChanger(index, Items.player,buyAgentButtonLocked, buyAgentButtonSelected);
        SpriteChanger(playerButtons, PlayerUnlockedSprite,PlayerLockedSprite,Items.player);
        /* PlayerData.Player currentPlayer = playerData.playerList[index];*/
        currentPlayer = playerData.GetPlayer(index);
        EnableItem(players, index);
        agentPriceText.text = currentPlayer.price.ToString();
       /* players[index].SetActive(true);
        for (int i = 0; i < players.Count; i++)
        {
            if (i != index)
            {
                
                players[i].SetActive(false);
            }
            
        }*/

        // Update the UI elements with the weapon's data
        playerHealthText.text = currentPlayer.health.ToString() + "%";
        playerHidingText.text = currentPlayer.hiding.ToString() + "%";
        playerReloadText.text = currentPlayer.reloadTime.ToString() + "%";

        playerHealthBar.fillAmount = Mathf.Clamp01(currentPlayer.health / 100f);
        playerHidingBar.fillAmount = Mathf.Clamp01(currentPlayer.hiding / 100f);
        playerReloadBar.fillAmount = Mathf.Clamp01(currentPlayer.reloadTime / 100f);

    }
    public void OnBuyPlayerButtonClick()
    {
      
        if (currentPlayer != null)
        {
            
            if (GameData.Coins >= currentPlayer.price)
            {
                GameData.SpendCoins(currentPlayer.price);
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

        if (currentWeapon != null)
        {
            
            if (GameData.Coins >= currentWeapon.price)
            {
                GameData.SpendCoins(currentWeapon.price);
                if (currentWeapon.isHeavy)
                {
                    GameData.SaveUnlocked_Gun(CurrentWeaponindex);
                    GameData.SaveSelected_Gun(CurrentWeaponindex);
                    DisplayWeapons(CurrentWeaponindex, snipers, Items.snipers);
                }
                else
                {
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
    private void ButtonChanger(int index,Items item,Button LockedBtn,Button UnlockedBtn)
    {
        UnlockedBtn.gameObject.SetActive(!(IsItemLocked(index, item)));
        LockedBtn.gameObject.SetActive(IsItemLocked(index, item));
      
    }
    private void SpriteChanger(Image ThisImage,Image OtherImage, Sprite ThisSprites,Sprite OhterSprite) 
    {
       ThisImage.sprite = ThisSprites;
        OtherImage.sprite = OhterSprite;
    }
}
