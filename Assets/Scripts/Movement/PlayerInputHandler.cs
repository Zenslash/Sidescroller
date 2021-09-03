using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        var playersMovement = FindObjectsOfType<PlayerMovement>();
        var index = _playerInput.playerIndex;
        _playerMovement = playersMovement.FirstOrDefault(player => player.GetPlayerIndex() == index);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(_playerInput.playerIndex);
        _playerMovement.SetInputVector(context.ReadValue<Vector2>());
    }
}
