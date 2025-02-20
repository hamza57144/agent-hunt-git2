using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        public bool isLocked;
    }

    public List<Player> playerList;

    private const string fileName = "PlayerData.json";

    // Save data to a file
    public void SaveData()
    {
        // Convert the player list to JSON
        string json = JsonUtility.ToJson(this, true);
        // Write the JSON to a file
        string path = Application.persistentDataPath + "/" + fileName;
        File.WriteAllText(path, json);
        Debug.Log($"Data saved to: {path}");
    }

    // Load data from a file
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            // Read the JSON from the file
            string json = File.ReadAllText(path);
            // Overwrite the current data with the loaded data
            JsonUtility.FromJsonOverwrite(json, this);
            Debug.Log($"Data loaded from: {path}");
        }
        else
        {
            Debug.LogWarning($"Save file not found at: {path}");
        }
    }

    public void UnlockPlayer(int index)
    {
        playerList[index].isLocked = false;
        PlayerPrefs.SetInt("Player" + index + "isLocked", playerList[index].isLocked ? 1 : 0);
    }

    public bool IsPlayerLocked(int index)
    {
        return playerList[index].isLocked;
    }

    public Player GetPlayer(int index)
    {
        return playerList[index];
    }
}
