using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static WeaponsData;
[CreateAssetMenu(fileName = "Stages Data", menuName = "Level Data")]

public class StatgesData : ScriptableObject
{
    [System.Serializable]
    public class Level
    {
        public string levelNo;
        public string totalEnemies;
        public string location;
        public int bossProgression;

    }
    public List<Level> levelData;
    public Level GetStage(int index)
    {      
            return levelData[index];      
    }
}
