using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private PlayerStatsManager _target;
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
    /// <summary>
    /// speed of Seek vector
    /// </summary>
    public float ChaseSpeed;
    public Vector3 Offset;
    /// <summary>
    /// How much sight tilt camera
    /// </summary>
    public float SightScaler;
    [SerializeField] private Vector3 seekPos;
    [SerializeField] private Vector3 cameraVelocity;
    [SerializeField] private Vector3 desiredPos;
    /// <summary>
    /// Speed of camera
    /// </summary>
    [SerializeField]
    [Range(0,1f)]
    private float speed;

    private Transform targetTransform;

    private void Start()
    {
        targetTransform = Target.transform;
        Target.Attack.AttackFired += AttackShake;
    }

    private void FixedUpdate()
    {
        if (Target == null) return;
        desiredPos = targetTransform.position + Target.Attack.Sight * SightScaler + Offset;
        seekPos = Vector3.Lerp(seekPos, desiredPos, ChaseSpeed);
        Vector3 finalPos = Vector3.Lerp(transform.position,seekPos,speed);
        transform.position = finalPos;
        Debug.DrawLine(transform.position, seekPos,Color.red);
        Debug.DrawLine(seekPos,desiredPos);
        //Vector3.MoveTowards(transform.position, desiredPos, Time.deltaTime * ChaseSpeed); //Vector3.SmoothDamp(transform.position, desiredPos, ref cameraVelocity, ChaseSpeed);
    }

    /// <summary>
    /// Shake camera in direction of vector
    /// </summary>
    /// <param name="ShakeVector">direction of shake</param>
    public void Shake(Vector3 ShakeVector)
    {
        seekPos += ShakeVector;

    }

    private void AttackShake(AttackEventArgs args)
    {

        Shake(-args.AttackDirection * args.RecoilPower);

    }
}
