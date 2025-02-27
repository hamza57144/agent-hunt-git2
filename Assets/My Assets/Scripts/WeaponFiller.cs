using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponFiller : MonoBehaviour
{
    public static WeaponFiller Instance { get; private set; }
    public Image weaponFillImage;
    public Image bgImage;
    public Canvas weaponCanvas;    
    public GameObject buttonsPanel;   

    private int progressLevel = 0; 
    private float fillStep = 1f / 3f; 
    [SerializeField] float fillSpeed;
    [SerializeField] GameManager gameManager;
    [SerializeField] Text progressText;
    public Image glowingImage;
    private float rotationSpeed = 30f;
    [SerializeField] WeaponsData weaponData;
    public GameObject claimNowTutorial;
    private void Awake()
    {
        Instance = this;
        weaponFillImage.sprite = weaponData.GetWeapon(GameData.Unlocked_Pistol_Index + 1, Items.pistols).fillSprite;
        bgImage.sprite = weaponData.GetWeapon(GameData.Unlocked_Pistol_Index + 1, Items.pistols).bgSprite;

    }
    private void Start()
    {
        progressLevel =  (GameData.CompletedLevelIndex % 3) + 1;
        weaponFillImage.fillAmount = (progressLevel-1) * fillStep;
        progressText.text = $"{weaponFillImage.fillAmount * 100}%";
        weaponCanvas.gameObject.SetActive(false);
        EnableButtonsPanel(false);
        if (GameData.CompletedLevelIndex < 4)
        {
            claimNowTutorial.SetActive(true);
        }
        else
        {
            claimNowTutorial.SetActive(false);
        }
    }
    private void Update()
    {
       if(glowingImage.gameObject.activeInHierarchy)
        glowingImage.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    public void OnLevelComplete()
    {           

        float targetFill = progressLevel * fillStep; // 0.33, 0.66, 1.0
        StartCoroutine(FillWeapon(targetFill));       
    }

    private IEnumerator FillWeapon(float targetFill)
    {
        weaponCanvas.gameObject.SetActive(true);
        float startFill = weaponFillImage.fillAmount;
        float duration = fillSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            weaponFillImage.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed / duration);
            progressText.text = $"{weaponFillImage.fillAmount * 100:0}%";
            elapsed += Time.deltaTime;
            yield return null;
        }

        weaponFillImage.fillAmount = targetFill;
        yield return new WaitForSeconds(1f); // Show for a moment
        SwitchPanels(progressLevel == 3);
     

    }


    private void SwitchPanels(bool enabled)
    {
        EnableButtonsPanel(enabled);

        if(!enabled)
        {
            weaponCanvas.gameObject.SetActive(false);
            gameManager.EnableGameCompleteCanvas();
        }

    }
    public void OnLoseItButtonClick()
    {
        
       SwitchPanels(false);
    }  
    private void EnableButtonsPanel(bool enable)
    {
        buttonsPanel.SetActive(enable);
        
    }
}
