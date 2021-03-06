using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class PlayerLook : MonoBehaviour
{
    #region Components
    [SerializeField]private GameObject lookTarget;
    private Transform lookTargetTransform;
    private Transform playerTransform;
    
    
    private PlayerStatsManager playerStatsManager;
    private Vector2 inputVector;
    
    [Tooltip("Minimal distance from player to look target")]
    [SerializeField][Range(0,100)] private float minAimRadius;
    [Tooltip("Speed at which target moves to desired position")]
    [SerializeField] [Range(0, .2f)] private float aimSpeed;
    
    
    [SerializeField]private Vector3 defaultTargetPosition;
    private Vector3 wantedTargetPosition;
    
    /// <summary>
    /// Look vector input
    /// </summary>
    public Vector2 InputVector
    {
        get => inputVector;
        set => inputVector = value;
    }
    #endregion
    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        lookTargetTransform = lookTarget.transform;
        playerTransform = playerStatsManager.Movements.PlayerLocalTransform;
        defaultTargetPosition = playerTransform.forward * 100;
        defaultTargetPosition.y += 4f;
        lookTargetTransform.position = defaultTargetPosition;
        
    }
    void OnDrawGizmosSelected()
    {
        var correctedPlayerPosition = playerTransform.position;
        correctedPlayerPosition.y += 4;
        Gizmos.DrawWireSphere(correctedPlayerPosition, minAimRadius);
    }
    /// <summary>
    /// Called when look target gets to close to player, calculates position for target on the sphere so the target dont get too close.
    /// </summary>
    void DeadZone()
    {
        var sphereCoord = lookTargetTransform.position;
        var playerCoord = playerTransform.position;
        playerCoord.y += 4f;
        
        wantedTargetPosition.z = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(minAimRadius,2) - Mathf.Pow(sphereCoord.x, 2) +
            2 * sphereCoord.x * playerCoord.x - Mathf.Pow(playerCoord.x, 2) - Mathf.Pow(sphereCoord.y, 2) +
            2 * sphereCoord.y*playerCoord.y - Mathf.Pow(playerCoord.y,2))) + playerCoord.z;
    }
    /// <summary>
    /// Called when player model need to be rotated. Rotates player 180
    /// </summary>
    void FlipDefaultPosition()
    {
        if (Math.Sign(lookTargetTransform.position.x) != Math.Sign(playerTransform.forward.x) && Mathf.RoundToInt(inputVector.normalized.x) !=0)
        {
            playerStatsManager.Movements.Rotation = new Vector3(Mathf.RoundToInt(InputVector.normalized.x),0,0);
        }
    }
    
    void Update()
    {
        if(playerStatsManager.Movements.IsOnLadder) return;
        FlipDefaultPosition();
        if (inputVector != Vector2.zero)
        {
            wantedTargetPosition = new Vector3(InputVector.x, InputVector.y, playerTransform.position.z);
            if (Mathf.Abs(lookTargetTransform.position.x - playerTransform.position.x) < minAimRadius && Mathf.Abs(lookTargetTransform.position.y - playerTransform.position.y) < minAimRadius)//(Mathf.Abs(lookTargetTransform.position.magnitude - playerTransform.position.magnitude) < minAimRadius)
            {
                DeadZone();
            }
        }
        else
        {
            wantedTargetPosition = playerTransform.forward * 100;
            wantedTargetPosition.y += 4f;
        }

        var lerpedWantedPosition = Vector3.Lerp(lookTargetTransform.position, wantedTargetPosition, aimSpeed);
        lerpedWantedPosition.z = wantedTargetPosition.z;
        lookTargetTransform.position = lerpedWantedPosition;


    }
}
