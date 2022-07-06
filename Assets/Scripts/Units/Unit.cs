using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : MonoBehaviour, IDamageable, IMovable
{
    [SerializeField] private bool _drawLineToTarget = true;
    [SerializeField] protected Transform _target;
    [field: SerializeField] public float Speed { get; private set; }

    [SerializeField] protected float _maxHealth = 100;
    [SerializeField] private float _deathDelay = 1f;

    public float Health { get; protected set; }
    public float PartHealth => Health / _maxHealth;
    [HideInInspector] public UnityEvent OnDie;
    [HideInInspector] public UnityEvent OnTakeDamage;

    protected NavMeshAgent _agent;

    public bool ApplyDamage(float damage)
    {
        if (Health <= 0)
            return false;

        damage = Math.Abs(damage);
        Health -= damage;
        OnTakeDamage?.Invoke();
        //Debug.Log(name + Health);

        if (Health <= 0)
        {
            Health = 0;
            OnDie?.Invoke();
            StartCoroutine(Death(_deathDelay));
            return true;
        }
        else return false;
    }

    public void MoveTo(Vector3 pointTarget)
    {
        _agent.SetDestination(pointTarget);
    }

    public void FollowTo(Transform target)
    {
        StopFollow();
        if (target != null)
        {
            _target = target;
            StartCoroutine(Follow(target));
        }
    }

    public void StopFollow()
    {
        StopCoroutine(Follow(_target));
        _target = null;
    }

    protected void Awake()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
    }

    private IEnumerator Follow(Transform target)
    {
        yield return null;

        while (target != null && target.gameObject.activeSelf)
        {
            _agent.destination = target.position;
            yield return null;
        }

        _agent.destination = transform.position;
    }

    private IEnumerator Death(float delay)
    {
        _target = null;
        _agent.speed = 0;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    protected void OnEnable()
    {
        Health = _maxHealth;
        _agent.speed = Speed;
    }

    protected void OnDrawGizmos()
    {
        if (_target != null && _drawLineToTarget)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, _target.transform.position);
        }
    }
}
