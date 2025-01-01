using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
   
    public static int SelectedPlayerIndex { get; private set; }
    public static int SelectedLevelIndex { get; private set; }
    public static int SelectedWeaponIndex { get; private set; }
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
    public static void SaveSelectedWeapon(int index)
    {
        SelectedWeaponIndex = index;
        PlayerPrefs.SetInt("SelectedWeapon", index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Weapon: {index}");
    }
    public static void LoadSelectedWeapon()
    {
        SelectedWeaponIndex = PlayerPrefs.GetInt("SelectedWeapon", 1); // Default to 0
        Debug.Log($"Loaded Selected Weapon: {SelectedWeaponIndex}");
    }

    public static void LoadGameData()
    {
        LoadSelectedPlayer();
        LoadSelectedLevel();
        LoadSelectedWeapon();
    }


}
public  class TagsHandler
{
    public static string Player { get { return "Player"; } }
    public static string Enemy { get { return "Enemy"; } }
    public static string BodyPart { get { return "BodyPart"; } }
    public static string Bullet { get { return "Bullet"; } }

}
public class SceneHandler
{
    public static string MainMenu { get { return "MainMenu"; } }
    public static string GamePlayScene { get { return "Gameplay Scene"; } }

}