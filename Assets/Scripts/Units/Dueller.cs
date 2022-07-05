using UnityEngine;

public class Dueller : Fighter
{
    protected override void Attack(Unit unit)
    {
        if (_canAttack)
        {
            if (unit.ApplyDamage(_damage))
            {
                Score++;
                FinishingStrike?.Invoke();
            }
            DelayAfterAttack();
        }
    }
}
