using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeapon", menuName = "ScriptableObjects/RangeWeapon",order = 1)]
public class RangeWeapon : ItemBase
{
    
    public WeaponType Type;
    [Range(0f,90f)]
    public float SpreadAngle;

    public float BulletSpeed;

    /// <summary>
    /// Bullets fired when trigger hold
    /// </summary>
    public int BulletPerTrigger;

    /// <summary>
    /// Delay between shots then trigger is hold
    /// </summary>
    public float DelayPerTrigger;

    /// <summary>
    /// Bullets amount per on one shot, (many if shotgun, one if otherwise)
    /// </summary>
    public int BulletAmountPerShot;
    
    /// <summary>
    /// How long you need to aim for accurate shot 
    /// </summary>
    public float AimingTime;

    public float Damage;
    /// <summary>
    /// Timespan between shots
    /// </summary>
    public float RecoilTime;
    /// <summary>
    /// How much shot affects sight angle
    /// </summary>
    public float RecoilPunishTime;
    /// <summary>
    /// How strong will camera shake then fired
    /// </summary>
    public float RecoilPower;
    [Range(0f,90f)]
    public float MaxSpreadAngle;

    public GameObject Bullet;

    private Transform gunPointer;

    /// <summary>
    /// End of the gun(or bow)
    /// </summary>
    public Transform GetGunPointer
    {
        get => gunPointer;
    }


    public void Shot() 
    {
        throw new System.NotImplementedException();
    }

}


public enum WeaponType
{
    Pistol,
    Bow,
    Rifle,
    Automatic,
    Shotgun,
    HandMade

}
