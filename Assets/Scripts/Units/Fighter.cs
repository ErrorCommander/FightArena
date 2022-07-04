using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Fighter : Unit
{
    [HideInInspector] public UnityEvent FinishingStrike = new UnityEvent();
    public int Score { get; protected set; }

    [Header("Attack settings")]
    [SerializeField] protected float _damage = 5;
    [Tooltip("Count attack per second")]
    [SerializeField] private float _attackSpeed = 3;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] protected UnitSensor _sensor;
    [SerializeField] private Transform _target;

    protected bool _canAttack = true;
    protected UnityEvent _readyToAttack = new UnityEvent();


    /// <summary>
    /// Set the target that the unit will follow and attack
    /// </summary>
    /// <param name="target">Target for follow and attack</param>
    public void SetTarget(Transform target)
    {
        FollowTo(target);
    }

    protected abstract void Attack(Unit unit);

    protected void Awake()
    {
        base.Awake();
        _agent.stoppingDistance = _attackRange;
        OnDie.AddListener(StopAttack);
    }

    protected void DelayAfterAttack()
    {
        StartCoroutine(TempDisableAttack());
    }

    private void AttackReadiness()
    {
        var units = _sensor.GetUnitsInArea();
        if (units == null || units.Count == 0)
            return;

        foreach (var item in units)
        {
            if (item.transform == _target)
            {
                Attack(item);
            }
        }

        Attack(units[0]);
    }

    private IEnumerator TempDisableAttack()
    {
        _canAttack = false;
        yield return new WaitForSeconds(1 / _attackSpeed);
        _canAttack = true;
        _readyToAttack?.Invoke();
    }

    private void StopAttack()
    {
        StopCoroutine(TempDisableAttack());
        _sensor.UnitEnter.RemoveListener(Attack);
        _readyToAttack.RemoveListener(AttackReadiness);
        _canAttack = false;
    }

    private void OnEnable()
    {
        Health = _maxHealth;
        _canAttack = true;
        _sensor.UnitEnter.AddListener(Attack);
        _readyToAttack.AddListener(AttackReadiness);
        Score = 0;

        if (_target != null)
            FollowTo(_target);
    }

    private void OnDisable()
    {
        StopAttack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
