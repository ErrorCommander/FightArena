using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private SpawnerFightersSystem _spawner;

    [HideInInspector] public UnityEvent<Unit> AddNewTarget = new UnityEvent<Unit>();
    public static TargetManager Instance => _instance ??= new GameObject("TargetManager").AddComponent<TargetManager>();

    private static TargetManager _instance;
    private List<Unit> _units = new List<Unit>();

    /// <summary>
    /// Returns target from the list as target that is not equal to input the unit. Comparison is made by reference.
    /// </summary>
    /// <param name="unit">Target for compare</param>
    /// <returns>Matching Unit search result. Result equals null if not found.</returns>
    public Unit GetTarget(Unit unit)
    {
        if (_units.Count == 0)
            return null;

        int index = Random.Range(0, _units.Count);
        Unit result = _units[index];
        if (result == unit)
        {
            if (index + 1 < _units.Count)
                result = _units[index + 1];
            else if (index > 1)
                result = _units[index - 1];
            else
                result = null;
        }

        return result;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _spawner = FindObjectOfType<SpawnerFightersSystem>();
        }
        else
        {
            _instance._spawner = this._spawner;
            Destroy(this);
        }

        _spawner.OnSpawnFighter.AddListener(AddUnit);
        var units = FindObjectsOfType<Unit>();
        foreach (var item in units)
        {
            AddUnit(item);
        }
    }

    private void AddUnit(Unit unit)
    {
        _units.Add(unit);
        unit.OnDie.AddListener(RemoveUnit);
        AddNewTarget?.Invoke(unit);
    }

    private void RemoveUnit()
    {
        List<Unit> liveUnits = new List<Unit>();

        foreach (var unit in _units)
        {
            if (unit.Health > 0)
            {
                liveUnits.Add(unit);
            }
        }
        
        _units = liveUnits;
    }
}
