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
    public List<CharacterMotor> players;
    public CharacterMotor Player { get { return players[GameData.SelectedPlayerIndex]; } }
    [SerializeField] EnemyDisplayManager enemyDisplayManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] ThirdPersonCamera thirdPersonCamera;
    [SerializeField] GameObject levelCompleteCanvas;
    [SerializeField] GameObject levelFailCanvas;
    int ind;
    private int headShot;
    private int bodyShot;
    private float accuracy;
    [SerializeField] TextMeshProUGUI headShotText;
    [SerializeField] TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI healthText;

    private int totalShots { get {  return headShot+bodyShot; } }
   
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
        ind = GameData.CompletedLevelIndex;
        GameData.LoadGameData();
        Player.gameObject.SetActive(true);
        instance = this;
    }
    private void Start()
    {
        CharacterMotor.OnPlayerDie += CharacterMotor_OnPlayerDie;
        BodyPartHealth.OnBodyShot += BodyPartHealth_OnBodyShot;
        BodyPartHealth.OnHeadShot += BodyPartHealth_OnHeadShot;

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
        ind++;
        GameData.SaveCompletedLevel(ind);
        headShotText.text = headShot.ToString()+"/"+EnemyManager.instance.enemyCount;
        healthText.text = Player.GetHealth.ToString("0")+"%";
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     }
  /*  public void SetPlayerPosition()
    {
        players[GameData.SelectedPlayerIndex].gameObject.transform.localPosition= GetPlayerPosition.playerPostion;
        players[GameData.SelectedPlayerIndex].gameObject.transform.localRotation = Quaternion.Euler(GetPlayerPosition.playerRotation);
        players[GameData.SelectedPlayerIndex].gameObject.SetActive(true) ;
    }*/
}
