using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    #region Fields
    
    [SerializeField]
    private float maxHealth = 100;
    
    private float currentHealth;
    
    #endregion

    #region Properties
    
    public float CurrentHealth
    {
        get => currentHealth;
    }

    public float CurrentHealthRatio
    {
        get => currentHealth / maxHealth;
    }
    
    #endregion

    #region Unity Callbacks
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    #endregion

    #region Public Methods

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    /**
     * Use positive value for dealing damage
     * And negative value for healing
     */
    public void ChangeHealth(float value)
    {
        currentHealth -= value;
    }

    #endregion
}
