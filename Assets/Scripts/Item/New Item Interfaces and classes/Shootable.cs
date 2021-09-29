using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shootable : ItemBase
{
    public float SpreadAngle;

    public float BulletSpeed;

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

    public abstract void Shot(); 

}
