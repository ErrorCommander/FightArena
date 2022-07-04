using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dueller : Fighter
{
    protected override void Attack(Unit unit)
    {
        if (_canAttack)
        {
            Debug.Log($"{name} attack({_damage}) -> {unit.name}");
            unit.ApplyDamage(_damage);
            DelayAfterAttack();
        }
    }
}
