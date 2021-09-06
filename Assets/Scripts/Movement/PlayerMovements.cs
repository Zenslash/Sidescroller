using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private int playerIndex = 0;

    private Rigidbody rigidBody;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private bool displaceButton = false;
    private bool isDisplaced = false;
    private bool isOnLadder = false;

    private Transform localTransform;
    
    private void Awake()
    {
        localTransform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();
    }
    
    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public void SetDisplaceInput(bool buttonInput)
    {
        displaceButton = buttonInput;
    }

    public void SetInputVector(Vector2 playerInputVector)
    {
        inputVector = playerInputVector;
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
                localTransform.Translate(new Vector3(newXCoord, 0, 2.0f));
            }
            else if (isDisplaced)
            {
                rigidBody.useGravity = true;
                isOnLadder = false;
                localTransform.Translate(new Vector3(0, 0, -2.0f));
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
               localTransform.Translate(new Vector3(0, 0, 2.0f));
            }
            else if (isDisplaced)
            {
                localTransform.Translate(new Vector3(0, 0, -2.0f));
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

        rigidBody.MovePosition(transform.position + moveDirection * Time.deltaTime);


    }

}
