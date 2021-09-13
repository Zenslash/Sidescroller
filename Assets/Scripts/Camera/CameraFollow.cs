using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PlayerStatsManager _target;
    public Vector3 Offset;
    public PlayerStatsManager Target
    {
        set
        {
            _target = value;
            targetTransform = _target.transform;
            _target.Attack.AttackFired += AttackShake;
        }
        get
        {
            return _target;
        }
    }

    public float ChaseSpeed;
    public float ZCordinates;
    /// <summary>
    /// How much sight tilt camera
    /// </summary>
    public float SightScaler;
    [SerializeField] private Vector3 cameraVelocity;

    private Transform targetTransform;

    

    private void FixedUpdate()
    {
        Follow();
        
    }

    private void Follow()
    {
        if (Target == null )
        {
            return;
        }
        Vector3 desiredPos = targetTransform.position + Target.Attack.Sight * SightScaler + Offset;
        

        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref cameraVelocity, ChaseSpeed);
    }

    /// <summary>
    /// Shake camera in direction of vector
    /// </summary>
    /// <param name="ShakeVector">direction of shake</param>
    public void Shake(Vector3 ShakeVector)
    {
        cameraVelocity += ShakeVector;
    }

    private void AttackShake(AttackEventArgs args)
    {
        Shake(-args.AttackDirection * args.RecoilPower);
    }
}
