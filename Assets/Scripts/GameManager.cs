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
        GameData.LoadSelectedPlayer();
        players[GameData.SelectedPlayerIndex].gameObject.SetActive(true);
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

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
