using CoverShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelManager Instance { get; set; }
    public int levelIndex = 0;
    [SerializeField] GameManager gameManager;

    [System.Serializable]
    public class Level
    {
        public string levelName;
        public GameObject level;
        public Transform PlayerPosition;
        #region Note
        /// <summary > 
        /// Note
        /// Sequense is important.  Assign Cover Points in sequence,
        /// means first cover point must be assigned in 0 index and second
        /// in index 1 and so on
        /// </summary>        
        [Tooltip("Note:Sequense is important. " +
            "Assign Cover Points in sequence, means first cover point must be assigned in 0 index and second in index 1 and so on")]
        #endregion
        public CoverPoint[] coverPoints;
        #region Note
        /// <summary>        
        /// Note:Sequense is important. 
        /// </summary>
        [Tooltip(
            "Note:Sequense is important. "
         )]
        #endregion
        public Cover[] covers;
        public int bullets=15;
        
    }

    public List<Level> levels = new List<Level>();
    public int totalLevels {  get { return levels.Count; } }
    public Level GetLevel { get { return levels[levelIndex]; } }
    public CoverPoint[] GetCoverPoints()
    {
        return levels[levelIndex].coverPoints;
    }
    
    public Cover[] GetCovers()
    {
        return levels[levelIndex].covers;
    }
    private void Awake()
    {        
       levelIndex=GameData.CompletedLevelIndex;
         Instance = this;
        SetPlayerPosition();
        EnableSelectedLevel();
    }
    private void Start()
    {
       
    }
    private void SetPlayerPosition()
    {
        gameManager.PlayerMotor.gameObject.transform.localRotation = levels[levelIndex].PlayerPosition.rotation;
        gameManager.PlayerMotor.gameObject.transform.localPosition = levels[levelIndex].PlayerPosition.position;
    }    
    private void EnableSelectedLevel()
    {
        
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == GameData.CompletedLevelIndex)
            {               
                levels[i].level.gameObject.SetActive(true);
            }
            else
            {
                levels[i].level.gameObject.SetActive(false);
            }
        }
    }
}
