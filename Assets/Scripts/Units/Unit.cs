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

    [SerializeField] protected float _maxHealth = 100;
    [SerializeField] private float _deathDelay = 1f;

    public float Health { get; protected set; }
    public float PartHealth => Health / _maxHealth;
    [HideInInspector] public UnityEvent OnDie;
    [HideInInspector] public UnityEvent OnTakeDamage;

    protected NavMeshAgent _agent;

    private Transform _targetFollowing;

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
        _targetFollowing = target;
        StartCoroutine(Follow(target));
    }

    public void StopFollow()
    {
        StopCoroutine(Follow(_targetFollowing));
        _targetFollowing = null;
    }

    protected void Awake()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.speed = Speed;
    }

    private IEnumerator Follow(Transform target)
    {
        while (target != null && target.gameObject.activeSelf)
        {
            _agent.destination = target.position;
            yield return null;
        }

        _agent.destination = transform.position;
    }

    private IEnumerator Death(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Health = _maxHealth;
    }
}
