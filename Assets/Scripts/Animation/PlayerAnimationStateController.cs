using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    private PlayerMovements playerMovements;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovements = GetComponent<PlayerMovements>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("Speed", Mathf.RoundToInt(playerMovements.GetInputVector().x));
        
    }
}
