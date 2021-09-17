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
    
    [SerializeField][Range(0,10)] private float runSpeed = 5f;
    [SerializeField][Range(0,10)] private float walkSpeed = 2f;
    [SerializeField][Range(0,10)] private float crouchSpeed = 1f;
    [SerializeField] [Range(0, 100)] private float moveToStairsSpeed = 10f;
    [SerializeField] [Range(0, 10)] private float timeToDisplace = 0.01f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float timeVelocity = 0.5f;
    [SerializeField] private Vector3 currentVelocity;
    private Rigidbody rigidBody;

    private Vector3 playerVelocity = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private Vector3 stairsTargetPosition;

    private Vector3 mainMovementAxis;
    private Quaternion moveRotation;

    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isWalkingBackwards = false;
    private bool isDisplacing = false;
    private bool isDisplaced = false;
    //private bool isOnLadder = false;

    private Transform localTransform;
    
    public bool IsWalkingBackwards
    {
        get => isWalkingBackwards;
        set => isWalkingBackwards = value;
    }
    public bool IsCrouching
    {
        get => isCrouching;
        set => isCrouching = value;
    }
    
    public bool IsRunning
    {
        get => isRunning;
        set => isRunning = value;
    }

    public Vector3 GetPlayerVelocity => currentVelocity;

    public Vector2 InputVector
    {
        get => inputVector;
        set
        {
            IsWalkingBackwards = CheckIfBackwards();
            inputVector = value;
        }
    }
    public float GetMaxSpeed
    {
        get
        {
            if (isCrouching)
            {
                return Mathf.Abs(crouchSpeed);
            }
            if (isRunning && !isCrouching && !isWalkingBackwards)
            {
                return Mathf.Abs(runSpeed);
            }
            else
            {
                return Mathf.Abs(walkSpeed);
            }
        }
    }
    #endregion
    private void Awake()
    
    {
        //localID = globalID;
        //globalID++;
        mainMovementAxis = transform.position;
        playerStatsManager = GetComponent<PlayerStatsManager>();
        localTransform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();
    }

    #region Refactor
    public void SetDisplaceInput(Vector2 displaceInputVector)
    {
        isDisplacing = true;
    }
    

    #endregion
   
    
    private bool CheckIfBackwards()
    {
        var rot = 0;
        if (Mathf.RoundToInt(transform.localRotation.y) == 0)
        {
            rot = 1;
        }
        return rot != Mathf.CeilToInt(inputVector.x);
    }

    #region Remake
    public void MoveToLadder(float ladderPosition)
    {
        /*if (isDisplacing)
        {
            if (!isDisplaced && inputVector.y > 0.9)
            {
                rigidBody.useGravity = false;
                isOnLadder = true;
                var newXCoord = ladderPosition - transform.position.x;
                localTransform.Translate(new Vector3(newXCoord, 0, 2.0f), Space.World);
            }
            else if (isDisplaced && inputVector.y < -0.9)
            {
                rigidBody.useGravity = true;
                isOnLadder = false;
                localTransform.Translate(new Vector3(0, 0, -2.0f), Space.World);
            }
            isDisplaced = !isDisplaced;
            isDisplacing = false;
        }*/
    }

    public void MoveToStairs(Vector3 targetPosition)
    {
        if (inputVector.y > 0.9)
        {
            stairsTargetPosition = targetPosition;   
            isDisplacing = true;
        }
        else if (inputVector.y < -0.9)
        {
            stairsTargetPosition.z = mainMovementAxis.z;
            isDisplacing = true;
        }
        /*if (displace)
        {
            if (!isDisplaced && inputVector.y > 0.9)
            {
               localTransform.Translate(new Vector3(0, 0, 2.0f), Space.World);
               isDisplaced = !isDisplaced;
            }
            else if (isDisplaced && inputVector.y < -0.9)
            {
                localTransform.Translate(new Vector3(0, 0, -2.0f), Space.World);
                isDisplaced = !isDisplaced;
            }
            
            displace = false;
        }*/
    }
    #endregion

    void FixedUpdate()
    {
        if (isDisplacing)
        {
            var relativePositon =  stairsTargetPosition - transform.position; 
            moveRotation = Quaternion.LookRotation(new Vector3(-relativePositon.z, 0, relativePositon.x), Vector3.up);
            rigidBody.rotation = Quaternion.RotateTowards(transform.rotation, moveRotation, rotationSpeed*10);
            playerVelocity = Vector3.SmoothDamp(transform.position, stairsTargetPosition, ref currentVelocity, timeToDisplace, moveToStairsSpeed);
            if ((transform.position - stairsTargetPosition).magnitude < 0.05)
            {
                moveRotation = Quaternion.LookRotation(new Vector3(0, 0, currentVelocity.x), Vector3.up);
                isDisplacing = false;
            }
        }
        if (isCrouching)
        {
            playerVelocity = new Vector3(inputVector.x * crouchSpeed, 0, 0);
        }
        else if(isRunning && !isWalkingBackwards && !isCrouching)
        {
            playerVelocity = new Vector3(inputVector.x * runSpeed, 0, 0);
        }
        else
        {
            playerVelocity = new Vector3(inputVector.x * walkSpeed, 0, 0);
        }
        currentVelocity = Vector3.Lerp(currentVelocity, playerVelocity, timeVelocity);
        currentVelocity.x = Mathf.Abs(currentVelocity.x) < 0.0001 ? 0 :currentVelocity.x;

        /*if (currentVelocity != Vector3.zero)
        {
            moveRotation = Quaternion.LookRotation(new Vector3(0, 0, currentVelocity.x), Vector3.up);
        }
        */
        rigidBody.rotation = Quaternion.RotateTowards(transform.rotation, moveRotation, rotationSpeed);
        rigidBody.MovePosition(transform.position + currentVelocity * Time.deltaTime);
    }

}

//2340