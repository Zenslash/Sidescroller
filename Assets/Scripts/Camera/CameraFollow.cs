using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerStatsManager Target;
    public float ChaseSpeed;
    public float ZCordinates;
    /// <summary>
    /// How much sight tilt camera
    /// </summary>
    public float SightScaler;
    [SerializeField] private Vector3 cameraVelocity;

    private Transform targetTransform;

    private void Start()
    {
        targetTransform = Target.transform;
        Target.Attack.AttackFired += AttackShake;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPos = targetTransform.position + Target.Attack.Sight * SightScaler;
        desiredPos.z = ZCordinates;

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
