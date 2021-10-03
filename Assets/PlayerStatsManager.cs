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
    
    public PlayerMovements Movements { get => _movements;  }
    public PlayerAttack Attack { get => _attack;  }
    
    public PlayerAnimationStateController Animation { get => _animation;  }
    
    public PlayerLook Look { get => _look;  }
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
        FindObjectOfType<CameraFollow>().Target = this;
    }
}
