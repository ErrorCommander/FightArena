using UnityEngine;

public class Barbarian : Fighter
{
    protected override void Attack(Unit unit)
    {
        if (_canAttack)
        {
            Debug.Log("Barbarian Attack");
            var units = _sensor.GetUnitsInArea();
            Debug.Log("Target count: " + units.Count);

            foreach (var item in units)
            {
                Debug.Log($"{name} attack({_damage}) -> {item.name}");
                if (item.ApplyDamage(_damage))
                {
                    FinishingStrike?.Invoke();
                }
            }
            
            DelayAfterAttack();
        }
    }
}
