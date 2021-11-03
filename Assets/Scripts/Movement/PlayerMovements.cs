using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Mirror;

public class PlayerMovements : NetworkBehaviour
{
    #region Comopnents

    private PlayerStatsManager playerStatsManager;
    [Tooltip("Max speed of running")]
    [SerializeField] [Range(0, 10)] private float runSpeed = 5f;
    [Tooltip("Max speed of walking")]
    [SerializeField] [Range(0, 10)] private float walkSpeed = 2f;
    [Tooltip("Max speed of crouch walking")]
    [SerializeField] [Range(0, 10)] private float crouchSpeed = 1f;
    [Tooltip("Max speed of climbing")]
    [SerializeField] [Range(0, 10)] private float climbSpeed = 3f;
    [Tooltip("Rotation speed")]
    [SerializeField] private float rotationSpeed = 20f;
    [Tooltip("Speed at which player gains max movement speed")]
    [SerializeField] private float acceleration = 0.5f;

    private Rigidbody rigidBody;
    
    private Vector3 currentVelocity;
    private Vector3 velocity = Vector3.zero;
    private Vector3 displacementTargetPosition;
    private Vector3 displacementTargetRotation;
    private Quaternion rotation = Quaternion.identity;

    private float mainMovementAxis;
    private Vector2 inputVector = Vector2.zero;
    private bool inputInteract = false;
    
    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isWalkingBackwards = false;
    private bool isDisplacing = false;
    private bool isOnLadder = false;
    private bool isDisplaced = false;

    private Transform localTransform;

    /// <summary>
    /// Returns player transform component.
    /// </summary>
    public Transform PlayerLocalTransform => localTransform;

    /// <summary>
    /// Returns true if player is currently displacing
    /// </summary>
    public bool IsDisplacing
    {
        get => isDisplacing;
        set => isDisplacing = value;
    }
   
    /// <summary>
    /// set true if interact button is pressed
    /// </summary>
    public bool InputInteract
    {
        set => inputInteract = value;
    }
   
    /// <summary>
    /// Returns true if player moving opposite way of his rotation
    /// </summary>
    public bool IsWalkingBackwards
    {
        get
        {
            if (IsDisplacing)
            {
                isWalkingBackwards = false;
            }
            else if (PlayerVelocity == Vector3.zero)
            {
                isWalkingBackwards = false;
            }
            else
            {
                isWalkingBackwards = Math.Sign(InputVector.x) != Math.Sign(localTransform.forward.x);
            }

            return isWalkingBackwards;
        }
    }
    
    /// <summary>
    /// Returns true if player is on ladder or is displacing to ladder
    /// </summary>
    public bool IsOnLadder
    {
        get => isOnLadder;
        set => isOnLadder = value;
    }
   
    /// <summary>
    /// Returns true if player is crouching
    /// </summary>
    public bool IsCrouching
    {
        get => isCrouching;
        set => isCrouching = value;
    }
    
    /// <summary>
    /// True if player is running.
    /// </summary>
    public bool IsRunning
    {
        get => isRunning;
        set => isRunning = value;
    }
    
    /// <summary>
    /// Returns and sets current player velocity
    /// </summary>
    public Vector3 PlayerVelocity
    {
        get => currentVelocity;
        set => currentVelocity = value;
    }

    /// <summary>
    /// Returns and sets movement input
    /// </summary>
    public Vector2 InputVector
    {
        get => inputVector;
        set => inputVector = value;
    }
   
    /// <summary>
    /// Returns max speed player can move
    /// </summary>
    public float GetMaxSpeed
    {
        get
        {
            if (IsCrouching)
            {
                return Mathf.Abs(crouchSpeed);
            }

            if (IsRunning && !IsCrouching && !IsWalkingBackwards)
            {
                return Mathf.Abs(runSpeed);
            }
            else
            {
                return Mathf.Abs(walkSpeed);
            }
        }
    }
   
    /// <summary>
    /// Sets rotation for player
    /// </summary>
    public Vector3 Rotation
    {
        set => rotation = Quaternion.LookRotation(value, Vector3.up);
    }

    /// <summary>
    /// True if player is not on main movement axis
    /// </summary>
    public bool IsDisplaced
    {
        get => isDisplaced;
        set => isDisplaced = value;
    }

    #endregion

    private void Awake()
    {
        mainMovementAxis = transform.position.z;
        playerStatsManager = GetComponent<PlayerStatsManager>();
        localTransform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(0,7);
    }
    
    
    /// <summary>
    /// Called when player in stairs or ladder trigger. Sets target for player to displace
    /// </summary>
    /// <param name="targetTransform">Displacement target</param>
    /// <param name="isLadderTrigger">Decides if player displaces to ladder or to stairs</param>
    public void SetDisplacementTarget(Transform targetTransform, bool isLadderTrigger)
    {
        if(IsDisplacing) return;
        var intInputVector = Mathf.RoundToInt(inputVector.y);
        displacementTargetRotation = targetTransform.forward;
        if (isLadderTrigger)
        {
            if (intInputVector == 1 && !IsDisplaced)
            {
                displacementTargetPosition = targetTransform.position;
                IsDisplacing = true;
                IsOnLadder = true;
                rigidBody.useGravity = false;
            }
            else if (inputInteract && IsOnLadder)
            {
                displacementTargetPosition = localTransform.position;
                displacementTargetPosition.z = mainMovementAxis;
                IsDisplacing = true;
                IsOnLadder = false;
                inputInteract = false;
                rigidBody.useGravity = true;
            }
        }
        else
        {
            if (intInputVector == 1 && !IsDisplaced)
            {
                displacementTargetPosition = targetTransform.position;
                IsDisplacing = true;

            }
            else if (intInputVector == -1 && IsDisplaced)
            {
                displacementTargetPosition = localTransform.position;
                displacementTargetPosition.z = mainMovementAxis;
                IsDisplacing = true;
            }
        }
    }
    
    
    /// <summary>
    /// Called when player in stairs or ladder trigger and presses button to displace
    /// </summary>
    void Displacing()
    {
        displacementTargetPosition.y = PlayerLocalTransform.position.y;
        var relativePositon =  displacementTargetPosition - transform.position;
        if (relativePositon != Vector3.zero)
        {
            Rotation = new Vector3(relativePositon.x, 0, relativePositon.z);
        }
        velocity = relativePositon * GetMaxSpeed;
        if ((transform.position - displacementTargetPosition).magnitude < 0.1)
        {
            
            velocity = Vector3.zero;
            PlayerVelocity = Vector3.zero;
            IsDisplaced = !IsDisplaced;
            IsDisplacing = false;
            if (IsDisplaced)
            {
                Rotation = displacementTargetRotation;
            }
            else if (!IsDisplaced)
            {
                Rotation = new Vector3(1, 0, 0);
            }
        }
    }
    
    
    /// <summary>
    /// Called when player is on ladder. Changes movement to climbing movement 
    /// </summary>
    void LadderMovement()
    {
        velocity = new Vector3(0, inputVector.y * climbSpeed, 0);
    }
    
    
    void FixedUpdate()
    {
        rigidBody.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
        if (IsDisplacing)
        {
            Displacing();
        }
        else if (IsOnLadder && IsDisplaced)
        {
            LadderMovement();
        }
        else
        {
            velocity = new Vector3(inputVector.x * GetMaxSpeed, 0, 0);
        }
        PlayerVelocity = Vector3.Lerp(PlayerVelocity, velocity, acceleration);
        PlayerVelocity = PlayerVelocity.magnitude < 0.01 ? Vector3.zero : PlayerVelocity;
        
        rigidBody.MovePosition(transform.position + PlayerVelocity * Time.deltaTime);
    }

}