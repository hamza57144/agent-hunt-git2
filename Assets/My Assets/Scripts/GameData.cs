using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GameData
{
    public static int SelectedPlayerIndex { get { return LoadSelectedPlayer(); } }
    public static int UnlockedPlayerIndex { get { return LoadUnlockedPlayer(); } }
    public static int CompletedLevelIndex { get {return LoadCompletedLevel(); } }
    public static int SelectedLevelIndex { get{return LoadSelectedLevel(); } }
    public static int Selected_Pistol_Index { get { return LoadSelectedPistol(); } }
    public static int Selected_Gun_Index { get { return LoadSelectedGun(); } }
    public static int Unlocked_Pistol_Index { get { return LoadUnlockedPistol(); } }
    public static int Unlocked_Gun_Index { get { return LoadUnlockedGun(); } }
    public static int SoundSettingsIndex { get { return LoadSoundSettings(); } }
    public static int MusicSettingsIndex { get { return LoadMusicSettings(); } }
    public static int Coins { get { return LoadCoins(); } }
    public static bool showBossProgress;
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
        int coins = PlayerPrefs.GetInt(PlayerPrefsHandler.Coins, 100); // Default to 100       
        return coins;
        
    }
    public static void SaveSoundSetting(int index)
    {
       
        PlayerPrefs.SetInt(PlayerPrefsHandler.SoundSettings, index);
        PlayerPrefs.Save();        
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
    }
    public static int LoadSelectedPlayer()
    { 
        int selectedPlayerIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedPlayer, 0); 
/*        Debug.Log($"Loaded Selected Player: {selectedPlayerIndex}");*/
        return selectedPlayerIndex;
    }
    public static void SaveUnlockedPlayer(int index)
    {       
        PlayerPrefs.SetInt(PlayerPrefsHandler.UnlockedPlayerIndex, index);
        PlayerPrefs.Save();       
    }
    public static int LoadUnlockedPlayer()
    {
        int unlockedPlayerIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.UnlockedPlayerIndex, 0);      
        return unlockedPlayerIndex;
    }
    public static void SaveCompletedLevel(int index)
    {     
        PlayerPrefs.SetInt(PlayerPrefsHandler.CompletedLevel, index);
        PlayerPrefs.Save();        
    }
    public static int LoadCompletedLevel()
    {
       int CompletedLevelIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.CompletedLevel, 0); // Default to 0       
        return CompletedLevelIndex;
    }
    public static void SaveSelectedPistol(int index)
    {       
        PlayerPrefs.SetInt(PlayerPrefsHandler.SelectedPistol, index);
        PlayerPrefs.Save();        
    }
    public static int LoadSelectedPistol()
    {
       int SelectedWeapon_Pistol_Index = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedPistol, 1); // Default to 1        
        return SelectedWeapon_Pistol_Index;
    }
    public static void SaveUnlocked_Pistol(int index)
    {
        PlayerPrefs.SetInt(PlayerPrefsHandler.UnlockedPistol, index);
        PlayerPrefs.Save();        
    }
    public static int LoadUnlockedPistol()
    {
        int UnlockedPistol_Index = PlayerPrefs.GetInt(PlayerPrefsHandler.UnlockedPistol, 0); // Default to 1       
        return UnlockedPistol_Index;
    }
    public static void SaveSelected_Gun(int index)
    {
        
        PlayerPrefs.SetInt(PlayerPrefsHandler.SelectedGun, index);
        PlayerPrefs.Save();        
    }
    public static int LoadSelectedGun()
    {
       int  Selected_Gun_Index = PlayerPrefs.GetInt(PlayerPrefsHandler.SelectedGun, 0);       
        return Selected_Gun_Index;
    }
    public static void SaveUnlocked_Gun(int index)
    {

        PlayerPrefs.SetInt(PlayerPrefsHandler.UnlockedGun, index);
        PlayerPrefs.Save();       
    }
    public static int LoadUnlockedGun()
    {
        int UnlockedGun = PlayerPrefs.GetInt(PlayerPrefsHandler.UnlockedGun, 0);       
        return UnlockedGun;
    }
    public static void SaveSelectedLevel(int index)
    {                  
        PlayerPrefs.SetInt(PlayerPrefsHandler.UnlockedLevel, index);
        PlayerPrefs.Save();       
    }
    public static int LoadSelectedLevel()
    {
       int SelectedLevelIndex = PlayerPrefs.GetInt(PlayerPrefsHandler.UnlockedLevel, 0); // Default to 0        
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
    public const string SelectedPistol = "SelectedPistol";
    public const string SelectedGun = "SelectedGun";
    public const string UnlockedPistol = "UnlockedPistol";
    public const string UnlockedGun = "UnlockedGun";
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
