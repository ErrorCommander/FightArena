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
    [field: SerializeField] public Transform Target { get; protected set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }

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

        if (Health <= 0)
        {
            Health = 0;
            OnDie?.Invoke();
            OnDie.RemoveAllListeners();
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
        if (target != null && gameObject.activeSelf)
        {
            Target = target;
            StartCoroutine(Follow(target));
        }

        Target = null;
    }

    public void StopFollow()
    {
        StopCoroutine(Follow(Target));
        Target = null;
    }

    protected void Heal(float value)
    {
        Health += value;
        if (Health > _maxHealth)
            Health = _maxHealth;
    }

    protected void Awake()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    protected void OnEnable()
    {
        Health = _maxHealth;
        _agent.speed = MoveSpeed;
    }

    protected void OnDrawGizmos()
    {
        if (Target != null && _drawLineToTarget)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, Target.transform.position);
        }
    }

    private IEnumerator Follow(Transform target)
    {
        while (target != null && target.gameObject.activeSelf)
        {
            _agent.destination = target.position;
            yield return new WaitForSeconds(Time.deltaTime * 5);
        }

        Target = null;
        _agent.destination = transform.position;
    }

    private IEnumerator Death(float delay)
    {
        Target = null;
        _agent.speed = 0;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

}
