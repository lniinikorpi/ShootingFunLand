using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public bool isAutomatic = false;
    public float fireRate = 2;
    public float bulletSpeed = 10;
}
