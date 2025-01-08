using CoverShooter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  
    public static GameManager instance { get; private set; }
    public List<CharacterMotor> players;
    [SerializeField] EnemyDisplayManager enemyDisplayManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] ThirdPersonCamera thirdPersonCamera;
    [SerializeField] GameObject levelCompleteCanvas;
    [SerializeField] GameObject levelFailCanvas;
    int ind;
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
        players[GameData.SelectedPlayerIndex].gameObject.SetActive(true);
        instance = this;
    }
    private void Start()
    {
        CharacterMotor.OnPlayerDie += CharacterMotor_OnPlayerDie;
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
