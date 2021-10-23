using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    #region Do not hurt me for this
    [SerializeField] private Vector3 gunPointer;
    //[SerializeField] private float spreadAngle;
    //[SerializeField] private float maxSpreadAngle;
    //[SerializeField] private float timeAiming;
    //[SerializeField] private float recoilTime;
    //[SerializeField] private float recoilPunishTime;
    //[SerializeField] public GameObject Bullet;
    //[SerializeField] private float bulletSpeed;
    //[SerializeField] private float recoilPower;
    #endregion

    public RangeWeapon CurrentWeapon;

    [SerializeField] private bool isAiming;
    [SerializeField] private float currentAngle;

    public delegate void AttackEventHandler(AttackEventArgs attack);
    public event AttackEventHandler AttackFired;
    
    public bool IsAiming
    {
        get => isAiming;
    }
    public bool CanFire
    {
        get 
        {
            return Time.time > lastFired;
        }  
    }
    [SerializeField] public Vector3 Sight;
    private Coroutine currentAim;
    private float lastFired;

    private void Start()
    {
        isAiming = false;
        
    }
    #region Refactor
    public void Aim(bool state)
    {
        if (state)
        {
            StartAiming();
        }
        else if (isAiming)
        {
            StopAiming();
        }
    }
    
    public void StartAiming()
    {
        isAiming = true;
        currentAim = StartCoroutine(Aiming());
    }

    public void StopAiming()
    {
        StopCoroutine(currentAim);
        isAiming = false;
    }
    #endregion
    public void Fire() //TODO Move it to RangeWeapon
    {
        if (isAiming && CanFire)
        {
            lastFired = Time.time + CurrentWeapon.RecoilTime;
            timeleft += CurrentWeapon.RecoilPunishTime;
            AttackEventArgs args = new AttackEventArgs(Sight, CurrentWeapon.RecoilPower);
            
            AttackFired.Invoke(args);
            CreateBullet();

        }
    }

    private void CreateBullet()
    {   
        float directionMistake = currentAngle == 0 ? 0 : Random.Range(-currentAngle, currentAngle);
        Vector3 directionVector = Quaternion.AngleAxis(directionMistake, Vector3.forward) * Sight.normalized;
        Quaternion direction = Quaternion.Euler(Vector2.SignedAngle(directionVector, Vector2.right), 90, 0);

        
        Rigidbody bullet = ObjectPoolManager.GetObject(CurrentWeapon.Bullet, gunPointer, direction).GetComponent<Rigidbody>();
        bullet.velocity = directionVector * CurrentWeapon.BulletSpeed;

    }

    /// <summary>
    /// How much time left until spread is gone
    /// </summary>
    private float timeleft;
   

    IEnumerator Aiming()
    {
        timeleft = CurrentWeapon.AimingTime;
        Vector3 Angle1;
        Vector3 Angle2;
        while (true)
        { // i feel great pain
            timeleft = timeleft > 0 ? timeleft - Time.deltaTime : 0; //TODO fix maxSpread bug
            currentAngle = (CurrentWeapon.SpreadAngle * (timeleft / CurrentWeapon.AimingTime))/ 2;
            if (currentAngle * 2 > CurrentWeapon.MaxSpreadAngle)
            {

                currentAngle = CurrentWeapon.MaxSpreadAngle / 2;
            }
            gunPointer = transform.position; //TODO Replace with Weapon Gunpointer
            Angle1 = Quaternion.AngleAxis(currentAngle, Vector3.forward) * Sight;
            Angle2 = Quaternion.AngleAxis(currentAngle, Vector3.back) * Sight;

            Debug.DrawLine(gunPointer, Angle1);
            Debug.DrawLine(gunPointer, Angle2);
            
            
            yield return null;
        }
      
    }
}