using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GameData
{
    public static int SelectedPlayerIndex { get { return LoadSelectedPlayer(); } }
    public static int UnlockedPlayerIndex { get { return LoadUnlockedPlayer(); } }
    public static int CompletedLevelIndex { get {return LoadCompletedLevel(); } }
    public static int SelectedLevelIndex { get{return LoadSelectedLevel(); } }
    public static int SelectedWeapon_Pistol_Index { get { return LoadSelectedWeaponPistol(); } }
    public static int SelectedWeapon_Gun_Index { get { return LoadSelectedWeaponGun(); } }
    public static int SoundSettingsIndex { get { return LoadSoundSettings(); } }
    public static int MusicSettingsIndex { get { return LoadMusicSettings(); } }
    public static int Coins { get { return LoadCoins(); } } 
    public static void AddCoins(int amount)
    {
        int coins = Coins+amount;        
        SaveCoins(coins);
        Debug.Log($"Added {amount} coins. Total coins: {Coins}");
    }

    public static void SpendCoins(int amount)
    {
          int coins = Coins - amount;
          
            SaveCoins(coins);
            Debug.Log($"Spent {amount} coins. Remaining coins: {Coins}");
           
       
    }
    private static void SaveCoins(int amount)
    {
        PlayerPrefs.SetInt(PlayerPrefsHandler.Coins, amount);
        PlayerPrefs.Save();
    }

    public static int LoadCoins()
    {
        int coins = PlayerPrefs.GetInt(PlayerPrefsHandler.Coins, 500); // Default to 500
        Debug.Log($"Loaded Coins: {coins}");
        return coins;
        
    }
    public static void SaveSoundSetting(int index)
    {
       
        PlayerPrefs.SetInt(PlayerPrefsHandler.SoundSettings, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Music Settings: ");
    }
    public static int LoadSoundSettings()
    {
       int  SoundSettingsIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.SoundSettings, 1);
        return SoundSettingsIndex;
    }
    public static void SaveMusicSettings(int index)
    {
       
        PlayerPrefs.SetInt(PlayerPrefsHandler.MusicSettings, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Music Settings: {index}");
    }
    public static int LoadMusicSettings()
    {
       int MusicSettingsIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.MusicSettings, 1);
        return MusicSettingsIndex;

    }
    public static void SaveSelectedPlayer(int index)
    {
        
        PlayerPrefs.SetInt(PlayerPrefsHandler.SelectedPlayer, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Player: {index}");
    }
    public static int LoadSelectedPlayer()
    { 
        int selectedPlayerIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedPlayer, 0); 
        Debug.Log($"Loaded Selected Player: {selectedPlayerIndex}");
        return selectedPlayerIndex;
    }
    public static void SaveUnlockedPlayer(int index)
    {       
        PlayerPrefs.SetInt(PlayerPrefsHandler.UnlockedPlayerIndex, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Player: {index}");
    }
    public static int LoadUnlockedPlayer()
    {
        int unlockedPlayerIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.UnlockedPlayerIndex, 0);
        Debug.Log($"Loaded Selected Player: {unlockedPlayerIndex}");
        return unlockedPlayerIndex;
    }
    public static void SaveCompletedLevel(int index)
    {     
        PlayerPrefs.SetInt(PlayerPrefsHandler.CompletedLevel, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Level: {index}");
    }
    public static int LoadCompletedLevel()
    {
       int CompletedLevelIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.CompletedLevel, 0); // Default to 0
        Debug.Log($"Loaded Selected Level: {CompletedLevelIndex}");
        return CompletedLevelIndex;
    }
    public static void SaveSelectedWeapon_Pistol(int index)
    {       
        PlayerPrefs.SetInt(PlayerPrefsHandler.SelectedWeaponPistol, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Weapon: {index}");
    }
    public static int LoadSelectedWeaponPistol()
    {
       int SelectedWeapon_Pistol_Index = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedWeaponPistol, 1); // Default to 1
        Debug.Log($"Loaded Selected Weapon: {SelectedWeapon_Pistol_Index}");
        return SelectedWeapon_Pistol_Index;
    }
    public static void SaveSelectedWeapon_Gun(int index)
    {
        
        PlayerPrefs.SetInt(PlayerPrefsHandler.SelectedWeaponGun, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Weapon: {index}");
    }
    public static int LoadSelectedWeaponGun()
    {
       int  SelectedWeapon_Gun_Index = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedWeaponGun, 2); 
        Debug.Log($"Loaded Selected Weapon: {SelectedWeapon_Gun_Index}");
        return SelectedWeapon_Gun_Index;
    }
    public static void SaveSelectedLevel(int index)
    {                  
        PlayerPrefs.SetInt(PlayerPrefsHandler.UnlockedLevel, index);
        PlayerPrefs.Save();
        Debug.Log($"Saved Selected Level: {index}");
    }
    public static int LoadSelectedLevel()
    {
       int SelectedLevelIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.UnlockedLevel, 0); // Default to 0
        Debug.Log($"Unlocked Level: {SelectedLevelIndex}");
        return SelectedLevelIndex;
    }
    /*public static void LoadGameData()
    {
        LoadSelectedPlayer();
        LoadCompletedLevel();
        LoadSelectedWeaponPistol();
        LoadSelectedLevel();
        LoadUnlockedPlayer();
        LoadSelectedWeaponGun();
        LoadMusicSettings();
        LoadSoundSettings();
       
    }*/
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
    public const string Coins = "Coins";
}
public static class AnimationHandler
{
    public const string MainMenuPlayerAnimation = "SetAnimation";
}