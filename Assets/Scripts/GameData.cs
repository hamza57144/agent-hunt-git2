using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
   
    public static int SelectedPlayerIndex { get; private set; }
    public static int SelectedLevelIndex { get; private set; }
    public static void SaveSelectedPlayer(int index)
    {
        SelectedPlayerIndex = index;
        PlayerPrefs.SetInt("SelectedPlayer", index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Player: {index}");
    }
    public static void LoadSelectedPlayer()
    {
        SelectedPlayerIndex = PlayerPrefs.GetInt("SelectedPlayer", 0); 
        Debug.Log($"Loaded Selected Player: {SelectedPlayerIndex}");
    }
    public static void SaveSelectedLevel(int index)
    {
        SelectedLevelIndex = index;
        PlayerPrefs.SetInt("SelectedLevel", index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Level: {index}");
    }
    public static void LoadSelectedLevel()
    {
        SelectedLevelIndex = PlayerPrefs.GetInt("SelectedLevel", 0); // Default to 0
        Debug.Log($"Loaded Selected Level: {SelectedLevelIndex}");
    }


}

