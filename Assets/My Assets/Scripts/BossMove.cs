using CoverShooter;
using UnityEngine;
using UnityEngine.AI;

public class BossMove : MonoBehaviour
{
    public static BossMove Instance {  get; private set; }
    public Cover[] bossCovers; // Assign in Inspector  
    public int bossCount { get { return bossCovers.Length; } }
    public CharacterMotor boss;
    private int currentWaypointIndex = 0;
    private bool hasMoved = false;
    private CoverPoint[] coverPoint;
    public LevelManager levelManager;
    public static bool moveToNext;
    public GameObject playerCover;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        coverPoint = levelManager.GetCoverPoints();
        boss.GetComponent<BaseBrain>().enabled = false;
       

    }
    public  void Move(int point)
    {
        /* if(bossCovers[point].gameObject!=null)
           Destroy(bossCovers[point].gameObject);*/
        boss.gameObject.GetComponent<AISight>().enabled = true;
        boss.GetComponent<BaseBrain>().enabled = false;
        boss.gameObject.GetComponent<AICover>().takeCover(bossCovers[point], bossCovers[point].gameObject.transform.position, -10, 2);
        Invoke(nameof(MovePlayer), 3.5f);
    }
  

    void MoveToNextPoint()
    {
        
        if (currentWaypointIndex < bossCovers.Length)
        {
          //  Destroy(bossCovers[currentWaypointIndex].gameObject);
           
            currentWaypointIndex++;           
            boss.gameObject.GetComponent<AICover>().takeCover(bossCovers[currentWaypointIndex], bossCovers[currentWaypointIndex].gameObject.transform.position,1,1);
            boss.GetComponent<BaseBrain>().enabled = false;
           /* if (currentWaypointIndex!=1)*/
              Invoke(nameof(MovePlayer),4.5f);
        }
    }
    private void MovePlayer()
    {
        moveToNext = true;
        Invoke(nameof(EnableBrain), 7f);
    }

    private void EnableBrain()
    {
        boss.GetComponent<BaseBrain>().enabled = true;
    }

    private void ComponentToDisable(bool enable)
    {
        boss.gameObject.GetComponent<AISight>().enabled = enable;
    }
}
