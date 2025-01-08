using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayer", menuName = "Player")]
public class PlayerData : ScriptableObject
{
    [System.Serializable]
    public class Player
    {
        public string playerName;
        public int price;
        public GameObject playerPrefab;
        public int health;
        public float hiding;
        public float reloadTime;
       
    }
    public List<Player> playerList;
}
