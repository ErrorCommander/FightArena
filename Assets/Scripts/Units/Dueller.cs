using UnityEngine;

public class Dueller : Fighter
{
    protected override void Attack(Unit unit)
    {
        if (_canAttack)
        {
            //Debug.Log($"{name} attack({_damage}) -> {unit.name}");
            if (unit.ApplyDamage(_damage))
            {
                FinishingStrike?.Invoke();
            }
            DelayAfterAttack();
        }
    }
}
