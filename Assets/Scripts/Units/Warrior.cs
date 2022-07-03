using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Warrior : MonoBehaviour, IDamageable, IMovable
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    public bool IsAlive => Health > 0;

    [SerializeField] private float _deathDelay;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private float _changeTargetRange = 3;

    private NavMeshAgent _agent;

    public event UnityAction OnDeath;

    private void Awake()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.speed = Speed;
    }

    public void ApplyDamage(float damage)
    {
        if (!IsAlive)
            return;

        damage = Math.Abs(damage);
        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            OnDeath?.Invoke();
            StartCoroutine(Death(_deathDelay));
        }
    }

    public void MoveTo(Vector3 pointTarget)
    {
        _agent.destination = pointTarget;
    }

    public void MoveTo(Transform target)
    {

    }

    private IEnumerator Death(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _changeTargetRange);
    }
}
