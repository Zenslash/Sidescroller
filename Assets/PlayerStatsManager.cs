using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    #region Components
    [SerializeField] private PlayerMovements _movements;
    [SerializeField] private PlayerAttack _attack;
    [SerializeField] private PlayerAnimationStateController _animation;
    [SerializeField] private BagButton _bagButton;
    public PlayerMovements Movements { get => _movements;  }
    public PlayerAttack Attack { get => _attack;  }
    
    public PlayerAnimationStateController Animation { get => _animation;  }

    public BagButton BagButton { get => _bagButton; }

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
        _bagButton = transform.GetComponent<BagButton>();
    }
}
