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
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float rotationSpeed = 20f;

    private int localID = 0;
    private static int globalID = 0;

    private Rigidbody rigidBody;

    private Vector3 playerVelocity = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;
    private Quaternion moveRotation;

    private bool isRunning = false;
    private bool displaceButton = false;
    private bool isDisplaced = false;
    private bool isOnLadder = false;

    private Transform localTransform;
    
    public bool IsRunning
    {
        get => isRunning;
        set => isRunning = value;
    }
    public Vector3 GetPlayerVelocity => currentVelocity;

    public float GetMaxSpeed
    {
        get
        {
            if (IsRunning)
            {
                return Mathf.Abs(runSpeed);
            }
            else
            {
                return Mathf.Abs(walkSpeed);
            }
        }
    }

    private void Awake()
    
    {
        localID = globalID;
        globalID++;
        localTransform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();
    }
    
    public int GetID()
    {
        return localID;
    }

    public void SetDisplaceInput(bool buttonInput)
    {
        displaceButton = buttonInput;
    }

    public void SetInputVector(Vector2 playerInputVector)
    {
        inputVector = playerInputVector;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    public void MoveToLadder(float ladderPosition)
    {
        if (displaceButton)
        {
            if (!isDisplaced)
            {
                rigidBody.useGravity = false;
                isOnLadder = true;
                var newXCoord = ladderPosition - transform.position.x;
                localTransform.Translate(new Vector3(newXCoord, 0, 2.0f), Space.World);
            }
            else if (isDisplaced)
            {
                rigidBody.useGravity = true;
                isOnLadder = false;
                localTransform.Translate(new Vector3(0, 0, -2.0f), Space.World);
            }
            isDisplaced = !isDisplaced;
            displaceButton = false;
        }
    }
    public void MoveToStairs()
    {
        if (displaceButton)
        {
            if (!isDisplaced)
            {
               localTransform.Translate(new Vector3(0, 0, 2.0f), Space.World);
            }
            else if (isDisplaced)
            {
                localTransform.Translate(new Vector3(0, 0, -2.0f), Space.World);
            }
            isDisplaced = !isDisplaced;
            displaceButton = false;
        }
    }

    public float TimeVelocity;
    [SerializeField]private Vector3 currentVelocity;
    void FixedUpdate()
    {
        if (isRunning)
        {
            playerVelocity = new Vector3(inputVector.x * runSpeed, 0, 0);
        }
        else
        {
            playerVelocity = new Vector3(inputVector.x * walkSpeed, 0, 0);
        }
        currentVelocity = Vector3.Lerp(currentVelocity, playerVelocity, TimeVelocity);
        currentVelocity.x = Mathf.Abs(currentVelocity.x) < 0.0001 ? 0 :currentVelocity.x;

        if (currentVelocity != Vector3.zero)
        {
            moveRotation = Quaternion.LookRotation(new Vector3(0, 0, currentVelocity.x), Vector3.up);
        }

        rigidBody.rotation = Quaternion.RotateTowards(transform.rotation, moveRotation, rotationSpeed);
        //rigidBody.velocity = playerVelocity;
        rigidBody.MovePosition(transform.position + currentVelocity * Time.deltaTime);


        //Debug.Log("test");

    }

}
