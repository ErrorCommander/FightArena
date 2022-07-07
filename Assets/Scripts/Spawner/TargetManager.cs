using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private SpawnerFightersSystem _spawner;

    [HideInInspector] public UnityEvent<Fighter> AddNewTarget;

    private List<Fighter> _units = new List<Fighter>();

    /// <summary>
    /// Returns target from the list as target that is not equal to input the unit. Comparison is made by reference.
    /// </summary>
    /// <param name="unit">Target for compare</param>
    /// <returns>Matching Unit search result. Result equals null if not found.</returns>
    public Fighter GetSuitableTarget(Fighter unit)
    {
        if (_units.Count == 0)
            return null;

        int index = Random.Range(0, _units.Count);
        Fighter target = _units[index];
        if (target == unit && target.Health > 0)
        {
            if (index + 1 < _units.Count)
                target = _units[index + 1];
            else if (index > 1)
                target = _units[index - 1];
            else
                target = null;
        }

        return target;
    }

    private void Awake()
    {
        _spawner.OnSpawnFighter.AddListener(AddUnit);
        var units = FindObjectsOfType<Fighter>();
        foreach (var item in units)
        {
            AddUnit(item);
        }

        CheckList();
    }

    private void AddUnit(Fighter unit)
    {
        _units.Add(unit);
        AddNewTarget?.Invoke(unit);
        AddNewTarget.RemoveAllListeners();
        AssignTarget(unit);
    }

    private void AssignTarget(Fighter unit)
    {
        Fighter target = GetSuitableTarget(unit);
        if (target == null || !target.gameObject.activeSelf)
        {
            AddNewTarget.AddListener(unit.SetTarget);
        }
        else
        {
            target.OnDie.AddListener(unit.RemoveTarget);
            unit.SetTarget(target);
            unit.OnDie.AddListener(RemoveUnit);
        }
    }

    private void RemoveUnit()
    {
       // CheckList();
    }

    private async void CheckList()
    {
        List<Fighter> liveUnits = new List<Fighter>();
        
        await Task.Delay(1000);

        foreach (var unit in _units)
        {
            if (unit.Health > 0)
            {
                liveUnits.Add(unit);
                if (unit.Target == null)
                {
                    AssignTarget(unit);
                    //Debug.Log(unit + " -> " + unit.Target);
                }
            }
        }
        
        _units = liveUnits;

        CheckList();
    }
}
