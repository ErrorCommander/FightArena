using UnityEngine;

public class Barbarian : Fighter
{
    protected override void Attack(Unit unit)
    {
        if (_canAttack)
        {
            var units = _sensor.GetUnitsInArea();

            foreach (var item in units)
            {
                if (item.ApplyDamage(_damage))
                {
                    Score++;
                    FinishingStrike?.Invoke();
                }
            }
            
            DelayAfterAttack();
        }
    }
}
