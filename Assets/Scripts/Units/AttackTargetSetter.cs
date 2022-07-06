using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fighter))]
public class AttackTargetSetter : MonoBehaviour
{
    private TargetManager _targetManager;
    private Fighter _fighter;
    private Unit _target;

    private void SetTarget()
    {
        if (_target != null)
            _target.OnDie.RemoveListener(SetTarget);

        _target = _targetManager.GetTarget(_fighter);
        if (_target == null)
        {
            Debug.Log("Not found Units for Attack");
            _targetManager.AddNewTarget.AddListener(SetNewTarget);
            return;
        }

        _fighter.SetTarget(_target.transform);
        _target.OnDie.AddListener(SetTarget);
    }

    private void Awake()
    {
        _fighter = gameObject.GetComponent<Fighter>();
    }

    private void OnEnable()
    {
        _targetManager = TargetManager.Instance;
        SetTarget();
    }

    private void OnDisable()
    {
        if(_target != null)
            _target.OnDie.RemoveListener(SetTarget);

        _targetManager.AddNewTarget.RemoveListener(SetNewTarget);
        _target = null;
    }

    private void SetNewTarget(Unit target)
    {
        _targetManager.AddNewTarget.RemoveListener(SetNewTarget);

        _target = target;
        _fighter.SetTarget(_target.transform);
        _target.OnDie.AddListener(SetTarget);
    }
}
