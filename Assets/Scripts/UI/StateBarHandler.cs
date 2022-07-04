using UnityEngine;
using UnityEngine.Events;

public class StateBarHandler : MonoBehaviour
{
    [SerializeField] private StateBar _stateBarPrefab;
    [SerializeField] private Vector3 _offseetStatebar;
    [SerializeField] private Fighter _target;

    private StateBar _stateBar;

    private void Awake()
    {
        if (_stateBar == null)
            _stateBar = Instantiate(_stateBarPrefab, transform);
        
        _stateBar.transform.position = transform.position + _offseetStatebar;
    }

    private void Initialize()
    {
        _target.OnDie.AddListener(DisableStateBar);
        _target.OnTakeDamage.AddListener(RefreshHealthBar);
        _target.FinishingStrike.AddListener(RefreshScorehBar);
        _stateBar.Initialize();
    }

    private void RefreshHealthBar()
    {
        _stateBar.ChangeHealth(_target.PartHealth);
    }

    private void RefreshScorehBar()
    {
        _stateBar.ChangeScore(_target.Score);
    }

    private void DisableStateBar()
    {
        _target.OnTakeDamage.RemoveListener(RefreshHealthBar);
        _target.FinishingStrike.RemoveListener(RefreshScorehBar);
        _target.OnDie.RemoveListener(DisableStateBar);
    }

    private void OnDisable() => DisableStateBar();
    private void OnEnable() => Initialize();
}
