using CoverShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelManager Instance { get; set; }
    public int levelIndex = 0;

    [System.Serializable]
    public class Level
    {
        public string levelName;
        public GameObject levelPrefab;
        public Transform PlayerPosition;
        #region Note
        /// <summary > 
        /// Note
        /// Sequense is important.  Assign Cover Point in sequence,
        /// means first cover point must be assigned in 0 index and second
        /// in index 1 and vice versa
        /// </summary>        
        [Tooltip("Note:Sequense is important. " +
            "Assign Cover Point in sequence, means first cover point must be assigned in 0 index and second in index 1 and vice versa")]
        #endregion
        public CoverPoint[] coverPoints;
        #region Note
        /// <summary>
        /// Special note from Hamza,I added cover and cover points separtely for 
        /// some reasons,If you want to change this logic, modify the Nav_Movement script accordingly.
        /// Note:Sequense is important. 
        /// </summary>
        [Tooltip("Special note from Hamza,I added cover and cover points separtely for some reasons,If you want to change this logic, modify the Nav_Movement script accordingly." +
            "Note:Sequense is important. "
         )]
        #endregion
        public Cover[] covers;

    }

    public List<Level> levels = new List<Level>();
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
        Instance = this;
    }
}