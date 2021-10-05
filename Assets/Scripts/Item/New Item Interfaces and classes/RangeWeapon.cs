using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : ItemBase
{
    
    public WeaponType Type;

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

    public float MaxSpreadAngle;

    public GameObject Bullet;

    /// <summary>
    /// End of the gun(or bow)
    /// </summary>
    public Transform GunPointer;


    public void Shot() 
    {
        throw new System.NotImplementedException();
    }

}


public enum WeaponType
{
    Pistols,
    Bows,

}
