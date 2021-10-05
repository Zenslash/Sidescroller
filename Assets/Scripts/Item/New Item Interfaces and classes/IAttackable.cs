using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public float GetDamage { get; set; }

    public float GetSwingTime { get; set; }

    public void Swing();
}
