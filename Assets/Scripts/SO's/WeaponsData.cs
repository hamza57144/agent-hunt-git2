using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]


public class WeaponsData : ScriptableObject
{
    
    [System.Serializable]
    public class Weapon
    {
        public string weaponName;
        public bool isHeavy;
        public int price;
        public GameObject weaponPrefab;
        public int health;
        public float hiding;
        public float reloadTime;
        public bool isLocked;
        
    }
    public List<Weapon> pistolData;
    public List<Weapon> sniperData;
    public void UnlockWeaon(int index, Items weaponType)
    {
        if (weaponType == Items.pistols)
        {
            pistolData[index].isLocked = false;
            PlayerPrefs.SetInt("Pistol" + index + "isLocked", pistolData[index].isLocked ? 1 : 0);
        } else
        {
            sniperData[index].isLocked = false;
            PlayerPrefs.SetInt("Sniper" + index + "isLocked", sniperData[index].isLocked ? 1 : 0);
        }
    }
    public bool IsWeaponLocked(int index, Items weaponType)
    {
        if(weaponType== Items.pistols)
          return pistolData[index].isLocked;
        return sniperData[index].isLocked;
    }

    public Weapon GetWeapon(int index, Items weaponType)
    {
        if (weaponType == Items.pistols)
            return pistolData[index];
        return sniperData[index];

    }
  
}
