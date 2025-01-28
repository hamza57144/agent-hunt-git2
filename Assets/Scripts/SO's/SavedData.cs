using UnityEngine;

public class SavedData : MonoBehaviour
{
    public WeaponsData weaponData;
    public PlayerData playerData;
    private MainMenuManager mainMenuManager;
    private void Awake()
    {
        mainMenuManager =MainMenuManager.Instance;
        for (int i = 1; i < weaponData.pistolData.Count; i++)
        {
            if (PlayerPrefs.HasKey("Pistol" + i + "isLocked"))
            {
                weaponData.pistolData[i].isLocked = PlayerPrefs.GetInt("Pistol" + i + "isLocked") == 1;

            }
            else
            {
                weaponData.pistolData[i].isLocked = true;
                mainMenuManager.DisplayWeapons(GameData.Unlocked_Pistol_Index,Items.pistols);
            }
        }
        for (int i = 1; i < weaponData.sniperData.Count; i++)
        {
            if (PlayerPrefs.HasKey("Sniper" + i + "isLocked"))
            {
                weaponData.pistolData[i].isLocked = PlayerPrefs.GetInt("Sniper" + i + "isLocked") == 1;

            }
            else
            {
                weaponData.sniperData[i].isLocked = true;
                mainMenuManager.DisplayWeapons(GameData.Unlocked_Gun_Index, Items.snipers);
            }
        }
        for (int i = 1; i < playerData.playerList.Count; i++)
        {
            if (PlayerPrefs.HasKey("Player" + i + "isLocked"))
            {
                playerData.playerList[i].isLocked = PlayerPrefs.GetInt("Player" + i + "isLocked") == 1;

            }
            else
            {
                playerData.playerList[i].isLocked=true;
                mainMenuManager.DisplayPlayer(GameData.SelectedPlayerIndex);


            }
        }


    }
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs data cleared.");
    }
    public void ClearWeaponsData()
    {
        for (int i = 1; i < weaponData.sniperData.Count; i++)
        {
            PlayerPrefs.DeleteKey("Sniper" + i + "isLocked");
        }

        for (int i = 1; i < weaponData.pistolData.Count; i++)
        {
            PlayerPrefs.DeleteKey("Pistol" + i + "isLocked");
        }
        PlayerPrefs.Save();
    }
    public void ClearPlayersData()
    {
        for (int i = 1; i < playerData.playerList.Count; i++)
            PlayerPrefs.DeleteKey("Player" + i + "isLocked");
        PlayerPrefs.Save();
    }
}
