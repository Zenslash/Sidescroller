using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    public void Equip(PlayerStatsManager statsManager);
    public void UnEquip(PlayerStatsManager statsManager);
}
