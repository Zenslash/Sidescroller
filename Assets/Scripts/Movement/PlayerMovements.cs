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

    [SerializeField] [Range(0, 10)] private float runSpeed = 5f;
    [SerializeField] [Range(0, 10)] private float walkSpeed = 2f;
    [SerializeField] [Range(0, 10)] private float crouchSpeed = 1f;
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
    private bool isOnLadder = false;
    private bool isDisplaced = false;
    //private bool isOnLadder = false;

    private Transform localTransform;

    public Transform PlayerLocalTransform => localTransform;
    public bool IsDisplacing => isDisplacing;

    public bool IsWalkingBackwards
    {
        get
        {
            //Debug.Log(Math.Sign(InputVector.x) != Math.Sign(localTransform.forward.x));
            //Debug.Log("invec" + InputVector.x + "loctr" + localTransform.forward.x);
            //Debug.Log("invec sign" + Math.Sign(InputVector.x) + "loctr sign" + Math.Sign(localTransform.forward.x));
            if (IsDisplacing)
            {
                isWalkingBackwards = false;
            }
            else if (GetPlayerVelocity == Vector3.zero)
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

    public bool IsOnLadder
    {
        get => isOnLadder;
        set => isOnLadder = value;
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
        set => inputVector = value;
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
        mainMovementAxis = transform.position;
        playerStatsManager = GetComponent<PlayerStatsManager>();
        localTransform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();
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

    #endregion

    public void Displace(Vector3 targetPosition, bool ladderTrigger)
    {
        IsOnLadder = ladderTrigger;
        var intInputVector = Mathf.RoundToInt(inputVector.y);
        if (intInputVector == 1 && !isDisplaced)
        {
            stairsTargetPosition = targetPosition;
            isDisplacing = true;

        }
        else if (intInputVector == -1 && isDisplaced)
        {
            stairsTargetPosition = localTransform.position;
            stairsTargetPosition.z = mainMovementAxis.z;
            isDisplacing = true;
        }
    }

    void Displacing()
    {
        var relativePositon =  stairsTargetPosition - transform.position;
        if (relativePositon != Vector3.zero)
        {
            moveRotation =
                Quaternion.LookRotation(new Vector3(relativePositon.x, 0, relativePositon.z), Vector3.up);
        }
        playerVelocity = relativePositon * GetMaxSpeed;
        if ((transform.position - stairsTargetPosition).magnitude < 0.1)
        {
            playerVelocity = Vector3.zero;
            currentVelocity = Vector3.zero;
            moveRotation = Quaternion.LookRotation(new Vector3(1, 0, 0), Vector3.up);
            isDisplacing = false;
            isDisplaced = !isDisplaced;
        }
    }

    void OnLadder()
    {
        Debug.Log(IsOnLadder);
    }


    void FixedUpdate()
    {
        //Debug.Log(currentVelocity);
        if (isDisplacing)
        {
            Displacing();
        }
        else if (isOnLadder && isDisplaced)
        {
            OnLadder();
        }
        else
        {
            playerVelocity = new Vector3(inputVector.x * GetMaxSpeed, 0, 0);
        }
        currentVelocity = Vector3.Lerp(currentVelocity, playerVelocity, timeVelocity);
        currentVelocity = Mathf.Abs(currentVelocity.magnitude) < 0.01 ? Vector3.zero : currentVelocity;
        rigidBody.rotation = Quaternion.RotateTowards(transform.rotation, moveRotation, rotationSpeed);
        rigidBody.MovePosition(transform.position + currentVelocity * Time.deltaTime);
    }

}

//2340