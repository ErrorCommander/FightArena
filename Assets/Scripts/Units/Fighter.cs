using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Fighter : Unit
{
    [Tooltip("Percentage discrepancy of the main parameters from the base value")]
    [SerializeField] [Range(0, 100)] private float _divergenceProcent = 10;
    [Header("Attack settings")]
    [SerializeField] private float _baseDamage = 5;
    [Tooltip("Count attack per second")]
    [SerializeField] private float _baseAttackSpeed = 3;
    [SerializeField] private float _attackRange = 2;
    [Tooltip("Draw attack range in UnityEditor")]
    [SerializeField] private bool _drawAttackRange;
    [Header("Bounty for killing")]
    [SerializeField] [Range(0, 100)] private float _healValue = 10;
    [SerializeField] [Range(0, 20)] private float _damageImprove = 2;
    [Header("Unit Sensor")]
    [SerializeField] protected UnitSensor _sensor;

    [HideInInspector] public UnityEvent FinishingStrike = new UnityEvent();
    public int Score { get; protected set; }
    protected bool _canAttack = true;
    protected UnityEvent _readyToAttack = new UnityEvent();

    protected float _damage;
    protected float _attackSpeed;

    /// <summary>
    /// Set the target that the unit will follow and attack.
    /// </summary>
    /// <param name="target">Target for follow and attack.</param>
    public void SetTarget(Fighter target)
    {
        if (target != null && target.gameObject != gameObject)
        {
            FollowTo(target.transform);
        }
    }

    /// <summary>
    /// Remove target for Fighter. Set target null.
    /// </summary>
    public void RemoveTarget()
    {
        Target = null;
    }

    protected abstract void Attack(Unit unit);

    protected new void Awake()
    {
        base.Awake();
        _agent.stoppingDistance = _attackRange;
        OnDie.AddListener(StopAttack);
        FinishingStrike.AddListener(Upgrade);
    }

    protected void DelayAfterAttack()
    {
        StartCoroutine(TimedDisableAttack());
    }

    private void AttackReadiness()
    {
        var units = _sensor.GetUnitsInArea();
        if (units == null || units.Count == 0)
            return;

        foreach (var item in units)
        {
            if (item.transform == Target)
            {
                Attack(item);
            }
        }

        Attack(units[0]);
    }

    private IEnumerator TimedDisableAttack()
    {
        _canAttack = false;
        yield return new WaitForSeconds(1 / _attackSpeed);
        _canAttack = true;
        _readyToAttack?.Invoke();
    }

    private void StopAttack()
    {
        StopCoroutine(TimedDisableAttack());
        _sensor.UnitEnter.RemoveListener(Attack);
        _readyToAttack.RemoveListener(AttackReadiness);
        _canAttack = false;
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        _canAttack = true;
        Score = 0;

        SetBaseParametersValue();
        Subscriptions();
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        StopAttack();
        Target = null;
    }

    private void Subscriptions()
    {
        _sensor.UnitEnter.AddListener(Attack);
        _readyToAttack.AddListener(AttackReadiness);
    }

    private void SetBaseParametersValue()
    {
        Vector2 factorRange = new Vector2(1 - _divergenceProcent / 100, 1 + _divergenceProcent / 100);
        _damage = _baseDamage * Random.Range(factorRange.x, factorRange.y);
        _attackSpeed = _baseAttackSpeed * Random.Range(factorRange.x, factorRange.y);
        Health = _maxHealth * Random.Range(factorRange.x, factorRange.y);
        //Debug.Log($"{name}  D:{_damage}  AtS:{_attackSpeed}   H:{Health}");
    }

    private void Upgrade()
    {
        _damage += _damageImprove;
        Heal(_healValue);
    }


#if UNITY_EDITOR
    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (_drawAttackRange)
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _attackRange);
        }
    }
#endif
}
