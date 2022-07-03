using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : MonoBehaviour, IDamageable, IMovable
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Health { get; private set; }
    public bool IsAlive => Health > 0;

    [SerializeField] private float _deathDelay;

    private NavMeshAgent _agent;
    private Transform _targetFollowing;

    public event UnityAction OnDeath;

    private void Awake()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.speed = Speed;
    }

    public bool ApplyDamage(float damage)
    {
        if (!IsAlive)
            return false;

        damage = Math.Abs(damage);
        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            OnDeath?.Invoke();
            StartCoroutine(Death(_deathDelay));
            return true;
        }
        else return false;
    }

    public void MoveTo(Vector3 pointTarget)
    {
        _agent.destination = pointTarget;
    }

    public void FollowTo(Transform target)
    {
        StopFollow();
        _targetFollowing = target;
        StartCoroutine(Follow(target));
    }

    public void StopFollow()
    {
        StopCoroutine(Follow(_targetFollowing));
    }

    private IEnumerator Follow(Transform target)
    {
        while (target != null)
        {
            _agent.destination = target.position;
            yield return null;
        }

        _agent.destination = transform.position;
    }

    private IEnumerator Death(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
