using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierSlider : MonoBehaviour
{
    public RectTransform rectTransform;
    public float targetDistance = 400f;
    public float speed = 100f;

   
    private Vector2 originalPosition;
    private bool movingToTarget = true;
    private bool isMoving = true;

    [SerializeField] private TextMeshProUGUI outsideTotalCoinsText;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private GameObject pre;
    [SerializeField] private GameObject post;
    [SerializeField] private Text rewardText;

    public static MultiplierSlider Instance;
    [SerializeField]
    private float limit0;
    [SerializeField] private float limit1;
    [SerializeField] private float limit2;
    [SerializeField] private float limit3;
    [SerializeField] private float limit4;

    [SerializeField] private TextMeshProUGUI x2Text;
    [SerializeField] private TextMeshProUGUI x3Text;
    [SerializeField] private TextMeshProUGUI x4Text;
    [SerializeField] private TextMeshProUGUI x5Text;

    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color inactiveColor = Color.white;

    [SerializeField] private float textMinScale = 1f;
    [SerializeField] private float textMaxScale = 1.5f;
    [SerializeField] private float scaleSpeed = 5f;
    int reward = 500;
    void Start()
    {
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform not assigned!");
            return;
        }


        originalPosition = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
       // rewardText.text = $"Reward: {reward}";
        if (PlayerPrefs.GetString("ExR", "no") == "yes")
        {
            Debug.Log("update exed");
            PlayerPrefs.SetString("ExR", "no");
            StopMovementAndCheckPosition();
        }

        if (rectTransform == null || !isMoving) return;


        Vector2 currentPosition = rectTransform.anchoredPosition;


        Vector2 targetPosition = movingToTarget
            ? new Vector2(originalPosition.x + targetDistance, originalPosition.y)
            : originalPosition;


        rectTransform.anchoredPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);


        if (Vector2.Distance(currentPosition, targetPosition) < 0.01f)
        {

            movingToTarget = !movingToTarget;
        }


        float currentX = rectTransform.anchoredPosition.x;
        UpdateTextAppearance(x2Text, currentX > limit0 && currentX < limit1);
        UpdateTextAppearance(x3Text, currentX > limit1 && currentX < limit2);
        UpdateTextAppearance(x4Text, currentX > limit2 && currentX < limit3);
        UpdateTextAppearance(x5Text, currentX > limit3 && currentX < limit4);

    }

    public void ClaimCall()
    {
        isMoving = false;
        // AdmobAdsManager.Instance.ShowRewardedAd(13); 
    }

    public void StopMovementAndCheckPosition()
    {
        float currentX = rectTransform.anchoredPosition.x;

        bool isX2Active = currentX > limit0 && currentX < limit1;
        bool isX3Active = currentX > limit1 && currentX < limit2;
        bool isX4Active = currentX > limit2 && currentX < limit3;
        bool isX5Active = currentX > limit3 && currentX < limit4;

        UpdateTextAppearance(x2Text, isX2Active);
        UpdateTextAppearance(x3Text, isX3Active);
        UpdateTextAppearance(x4Text, isX4Active);
        UpdateTextAppearance(x5Text, isX5Active);

        if (isX2Active)
        {
            int coinss = PlayerPrefs.GetInt("coins");
            PlayerPrefs.SetInt("coins", coinss + 5000 * 1);
            coins.text = "You Received 10,000 Coins";
            outsideTotalCoinsText.text = "10,000 Coins";          
            Debug.Log("coins 2x earned");
        }
        else if (isX3Active)
        {
            int coinss = PlayerPrefs.GetInt("coins");
            PlayerPrefs.SetInt("coins", coinss + 5000 * 2);
            coins.text = "You Received 15,000 Coins";
            outsideTotalCoinsText.text = "15,000 Coins";         
            Debug.Log("coins 3x earned");
        }
        else if (isX4Active)
        {
            int coinss = PlayerPrefs.GetInt("coins");
            PlayerPrefs.SetInt("coins", coinss + 5000 * 3);
            coins.text = "You Received 20,000 Coins";
            outsideTotalCoinsText.text = "20,000 Coins";
           
            Debug.Log("coins 4x earned");
        }
        else if (isX5Active)
        {
            int coinss = PlayerPrefs.GetInt("coins");
            PlayerPrefs.SetInt("coins", coinss + 5000 * 4);
            coins.text = "You Received 25,000 Coins";
            outsideTotalCoinsText.text = "25,000 Coins";
           
            Debug.Log("coins 5x earned");
        }
        else
        {
            Debug.Log("Position is outside the expected range. Current X: " + currentX);
        }

        pre.SetActive(false);
        post.SetActive(true);
    }

    private void UpdateTextAppearance(TextMeshProUGUI text, bool isActive)
    {

        float targetScale = isActive ? textMaxScale : textMinScale;
        Vector3 currentScale = text.rectTransform.localScale;
        Vector3 newScale = Vector3.Lerp(currentScale, Vector3.one * targetScale, Time.deltaTime * scaleSpeed);
        text.rectTransform.localScale = newScale;


        text.color = isActive ? activeColor : inactiveColor;

        float currentX = rectTransform.anchoredPosition.x;

        bool isX2Active = currentX > limit0 && currentX < limit1;
        bool isX3Active = currentX > limit1 && currentX < limit2;
        bool isX4Active = currentX > limit2 && currentX < limit3;
        bool isX5Active = currentX > limit3 && currentX < limit4;

        if (isX2Active)
        {
            buttonText.text = "+500 COINS";
            reward = 500+500;
        }
        if (isX3Active)
        {
            buttonText.text = "+1000 COINS";
            reward = 500 + 1000;
        }
        if (isX4Active)
        {
            buttonText.text = "+1500 COINS";
            reward = 500 + 1500;
        }
        if (isX5Active)
        {
            buttonText.text = "+2000 COINS";
            reward = 500 + 2000;
        }
    }

    IEnumerator delayedLogic()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("exed");
    }
}
