using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GameData
{
    public static int SelectedPlayerIndex { get; private set; }
    public static int UnlockedPlayerIndex { get; private set; }
    public static int CompletedLevelIndex { get; private set; }
    public static int SelectedLevelIndex { get; private set; }
    public static int SelectedWeapon_Pistol_Index { get; private set; }
    public static int SelectedWeapon_Gun_Index { get; private set; }
    public static int SoundSettingsIndex { get; private set; }
    public static int MusicSettingsIndex { get; private set; }

    public static void SaveSoundSetting(int index)
    {
       
        PlayerPrefs.SetInt(PlayerPrefsHandler.SoundSettings, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Music Settings: ");
    }
    public static void LoadSoundSettings()
    {
        SoundSettingsIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.SoundSettings, 1);
        
    }
    public static void SaveMusicSettings(int index)
    {
       
        PlayerPrefs.SetInt(PlayerPrefsHandler.MusicSettings, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Music Settingswerwerer: {index}");
    }
    public static void LoadMusicSettings()
    {
        MusicSettingsIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.MusicSettings, 1);

    }
    public static void SaveSelectedPlayer(int index)
    {
        SelectedPlayerIndex = index;
        PlayerPrefs.SetInt(PlayerPrefsHandler.SelectedPlayer, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Player: {index}");
    }
    public static void LoadSelectedPlayer()
    {
        SelectedPlayerIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedPlayer, 0); 
        Debug.Log($"Loaded Selected Player: {SelectedPlayerIndex}");
    }
    public static void SaveUnlockedPlayer(int index)
    {
        UnlockedPlayerIndex = index;
        PlayerPrefs.SetInt(PlayerPrefsHandler.UnlockedPlayerIndex, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Player: {index}");
    }
    public static void LoadUnlockedPlayer()
    {
        SelectedPlayerIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.UnlockedPlayerIndex, 0);
        Debug.Log($"Loaded Selected Player: {SelectedPlayerIndex}");
    }
    public static void SaveCompletedLevel(int index)
    {
        CompletedLevelIndex = index;
        PlayerPrefs.SetInt(PlayerPrefsHandler.CompletedLevel, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Level: {index}");
    }
    public static void LoadCompletedLevel()
    {
        CompletedLevelIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.CompletedLevel, 0); // Default to 0
        Debug.Log($"Loaded Selected Level: {CompletedLevelIndex}");
    }
    public static void SaveSelectedWeapon_Pistol(int index)
    {
        SelectedWeapon_Pistol_Index = index;
        PlayerPrefs.SetInt(PlayerPrefsHandler.SelectedWeaponPistol, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Weapon: {index}");
    }
    public static void LoadSelectedWeaponPistol()
    {
        SelectedWeapon_Pistol_Index = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedWeaponPistol, 1); // Default to 1
        Debug.Log($"Loaded Selected Weapon: {SelectedWeapon_Pistol_Index}");
    }
    public static void SaveSelectedWeapon_Gun(int index)
    {
        SelectedWeapon_Gun_Index = index;
        PlayerPrefs.SetInt(PlayerPrefsHandler.SelectedWeaponGun, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Weapon: {index}");
    }
    public static void LoadSelectedWeaponGun()
    {
        SelectedWeapon_Gun_Index = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedWeaponGun, 2); // Default to 1
        Debug.Log($"Loaded Selected Weapon: {SelectedWeapon_Gun_Index}");
    }
    public static void SaveSelectedLevel(int index)
    {
        SelectedLevelIndex = index;
        PlayerPrefs.SetInt(PlayerPrefsHandler.UnlockedLevel, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Level: {index}");
    }
    public static void LoadSelectedLevel()
    {
        SelectedLevelIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.UnlockedLevel, 0); // Default to 0
        Debug.Log($"Unlocked Level: {SelectedLevelIndex}");
    }
    public static void LoadGameData()
    {
        LoadSelectedPlayer();
        LoadCompletedLevel();
        LoadSelectedWeaponPistol();
        LoadSelectedLevel();
        LoadUnlockedPlayer();
        LoadSelectedWeaponGun();
        LoadMusicSettings();
        LoadSoundSettings();
    }
}
public static class TagsHandler
{
    public const  string Player = "Player";
    public const string Enemy = "Enemy";
    public const string BodyPart = "BodyPart";
    public const string Bullet = "Bullet";
}
public static class SceneHandler
{
    public const string MainMenu = "MainMenu";
    public const string GameplayScene = "Gameplay Scene";
    public const string LoadingScene = "LoadingScene";
}
public static class PlayerPrefsHandler
{   
    public const string CompletedLevel = "CompletedLevel";
    public const string UnlockedLevel = "UnlockedLevel";
    public const string SelectedWeaponPistol = "SelectedWeaponPistol";
    public const string SelectedWeaponGun = "SelectedWeaponGun";
    public const string SelectedPlayer = "SelectedPlayer";
    public const string UnlockedPlayerIndex = "UnlockedPlayerIndex";
    public const string SoundSettings = "SoundSettings";
    public const string MusicSettings = "MusicSettings";


}