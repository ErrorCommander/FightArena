using System;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float Health { get; private set; }

    public bool ApplyDamage(float damage)
    {
        if (Health <= 0)
            return false;

        damage = Math.Abs(damage/2);
        Health -= damage;
        Debug.Log(name + Health);

        if (Health <= 0)
        {
            Health = 0;
            Destroy(gameObject, 5f);
            gameObject.GetComponent<Material>().color = Color.grey;
            return true;
        }
        else return false;
    }
}
