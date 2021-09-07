using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    #region Do not hurt me for this
    [SerializeField] private Vector3 gunPoiner;
    [SerializeField] private float spreadAngle;
    [SerializeField] private float maxSpreadAngle;
    [SerializeField] private float timeAiming;
    [SerializeField] private float recoilTime;
    public bool inputtt;
    #endregion
    [SerializeField] private bool isAiming;
    [SerializeField] public Vector3 Sight;
    private Coroutine currentAim;

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
    public void Update()
    {
        

    }

    IEnumerator Aiming()
    {
        float timeleft = timeAiming;
        Vector3 Angle1 = Quaternion.AngleAxis(spreadAngle, Vector3.forward) * Sight;
        Vector3 Angle2 = Quaternion.AngleAxis(spreadAngle, Vector3.back) * Sight;
        while (timeleft > 0)
        {
            //TODO Переписать
            gunPoiner = transform.position;

            
            Debug.DrawLine(gunPoiner, Angle1);
            Debug.DrawLine(gunPoiner, Angle2);
            timeleft -= Time.deltaTime;
            Angle1 = Quaternion.AngleAxis(spreadAngle * (timeleft / timeAiming), Vector3.forward) * Sight;
            Angle2 = Quaternion.AngleAxis(spreadAngle * (timeleft / timeAiming), Vector3.back) * Sight;
            yield return null;
        }
        while (true)
        {
            //TODO Переписать
            gunPoiner = transform.position;
            Debug.DrawLine(gunPoiner, Sight);
            yield return null;
        }
    }
}