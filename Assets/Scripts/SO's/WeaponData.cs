using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    [System.Serializable]
    public class Weapon
    {
        public string weaponName;
        public int price;
        public GameObject weaponPrefab;
        public int health;
        public float hiding;
        public float reloadTime;
        
    }
    public List<Weapon> weaponList;
}