using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private int playerIndex = 0;

    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _inputVector = Vector2.zero;
    private Vector3 _velocity = Vector3.zero;

    private float _gravity = Physics.gravity.y;
    private void Awake()
    {

        _characterController = GetComponent<CharacterController>();
    }
    
    public int GetPlayerIndex()
    {
        Debug.Log(playerIndex);
        return playerIndex;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        _inputVector = inputVector;
    }

    
    void Update()
    {
        
        _moveDirection = new Vector3(_inputVector.x, 0, _inputVector.y);
        if (_moveDirection != Vector3.zero)
        {
            _characterController.Move(_moveDirection * playerSpeed * Time.deltaTime);
        }
        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
        if (_characterController.isGrounded && _velocity.y < 0){
            _velocity.y = 0f;
        }
    }
}
