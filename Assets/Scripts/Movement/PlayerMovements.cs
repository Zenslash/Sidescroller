using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 3f; 
    
    private int localID = 0;
    private static int globalID = 0;

    private Rigidbody rigidBody;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private bool displaceButton = false;
    private bool isDisplaced = false;
    private bool isOnLadder = false;

    private Transform localTransform;

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
    
    void FixedUpdate()
    {
        if (isOnLadder)
        {
            moveDirection = new Vector3(0, inputVector.y * playerSpeed, 0);
        }
        else
        { 
            moveDirection = new Vector3(inputVector.x * playerSpeed, 0, 0);
        }
        Debug.Log(inputVector + " inputVector");
        Debug.Log(moveDirection + " moveDirection");
        
        rigidBody.MovePosition(transform.position + moveDirection * Time.deltaTime);


    }

}
