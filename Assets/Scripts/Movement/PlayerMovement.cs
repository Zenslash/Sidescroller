using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private int playerIndex = 0;

    private Rigidbody rigidBody;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;
    private Vector3 velocity = Vector3.zero;

    private float gravity = Physics.gravity.y;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    
    public int GetPlayerIndex()
    {
        Debug.Log(playerIndex);
        return playerIndex;
    }

    public void SetInputVector(Vector2 playerInputVector)
    {
        inputVector = playerInputVector;
    }

    
    void Update()
    {
        
        moveDirection = new Vector3(inputVector.x, 0, 0);
        if (moveDirection != Vector3.zero)
        {
            rigidBody.velocity = moveDirection * playerSpeed;
        }
        /*velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }*/
    }
}
