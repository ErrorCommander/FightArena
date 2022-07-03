using System.Collections;
using UnityEngine;

public abstract class Fighter : Unit
{
    [SerializeField] protected float _damage;
    [Tooltip("Count attack per second")]
    [SerializeField] private float _attackSpeed = 3;
    [SerializeField] private float _attackRange = 2;
    //[SerializeField] private float _changeTargetRange = 3;

    private Transform _mainTarget;
    private bool _canAttack = true;

    /// <summary>
    /// Tries to attack the specified target
    /// </summary>
    /// <param name="target">Target for attack</param>
    /// <returns>Was it possible to attack</returns>
    public bool TryAttack(IDamageable target)
    {
        if (_canAttack)
        {
            StartCoroutine(DelayAttack());
            Attack();
        }

        return _canAttack;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        _mainTarget = target;
        FollowTo(target);
    }

    protected abstract void Attack();

    private IEnumerator DelayAttack()
    {
        _canAttack = false;
        yield return new WaitForSeconds(1 / _attackSpeed);
        _canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, _changeTargetRange);
    }
}
