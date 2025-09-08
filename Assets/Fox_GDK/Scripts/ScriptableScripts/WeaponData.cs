using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "FoxTools/Inventory/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "Weapon Name is not defined";
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float captureTime = 3f;

    //[HideInInspector]
    public float fireRateMinimizer = 1;
    public int upgradeLimit = 3;
    public int currentUpgradeStep = 0;

    public void Reset()
    {
        fireRateMinimizer = 1;
        currentUpgradeStep = 0;
    }

    public WeaponData ExtractData()
    {
        WeaponData weaponData = new WeaponData();
        weaponData.weaponName = this.weaponName;
        weaponData.bulletPrefab = this.bulletPrefab;
        weaponData.fireRate = this.fireRate;
        weaponData.captureTime = this.captureTime;
        weaponData.fireRateMinimizer = this.fireRateMinimizer;
        weaponData.upgradeLimit = this.upgradeLimit;
        weaponData.currentUpgradeStep = this.currentUpgradeStep;
        return weaponData;
    }
}