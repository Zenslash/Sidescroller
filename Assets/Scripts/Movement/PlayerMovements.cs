using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private int playerIndex = 0;
    [SerializeField] private float stairsZCoord = 6.0f;

    private Rigidbody rigidBody;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;
    private Vector3 velocity = Vector3.zero;

    private bool isDisplaced = false;
    
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
    

    public void MoveToStairs()
    {
        if (inputVector.y > 0.8f && !isDisplaced)
        {
            GetComponent<Transform>().Translate(new Vector3(0, 0, 2.0f));
            isDisplaced = !isDisplaced;
        }
        if (inputVector.y < -0.8f && isDisplaced)
        {
            GetComponent<Transform>().Translate(new Vector3(0, 0, -2.0f));
            isDisplaced = !isDisplaced;
        }    
    }
    
    void FixedUpdate()
    {
        
        moveDirection = new Vector3(inputVector.x * playerSpeed, 0, 0);
        rigidBody.MovePosition(transform.position + moveDirection * Time.deltaTime);

    }
}
