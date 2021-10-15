using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    #region Components
    [SerializeField] private PlayerMovements _movements;
    [SerializeField] private PlayerAttack _attack;
    [SerializeField] private PlayerAnimationStateController _animation;
    [SerializeField] private PlayerLook _look;
    [SerializeField] private PlayerHandIK _handIK;
    [SerializeField] private PlayerFootIK _footIK;
    
    public PlayerMovements Movements { get => _movements;  }
    public PlayerAttack Attack { get => _attack;  }
    
    public PlayerAnimationStateController Animation { get => _animation;  }
    
    public PlayerLook Look { get => _look;  }

    public PlayerHandIK HandIK { get => _handIK; }
    
    public PlayerFootIK FootIK { get => _footIK; }
    #endregion


    #region Stats
    private float _hp;

    public float Hp { get => _hp; set => _hp = value; }
    #endregion

    private void Awake()
    {
        _movements = transform.GetComponent<PlayerMovements>();
        _attack = transform.GetComponent<PlayerAttack>();
        _animation = transform.GetComponent<PlayerAnimationStateController>();
        _look = transform.GetComponent<PlayerLook>();
        _handIK = transform.GetComponent<PlayerHandIK>();
        _footIK = transform.GetComponent<PlayerFootIK>();
        FindObjectOfType<CameraFollow>().Target = this;
    }
}
