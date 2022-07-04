using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class UnitSensor : MonoBehaviour
{
    [SerializeField] private float _radius;
    private SphereCollider _collider;
    private Dictionary<Transform, Unit> _units;

    public UnityEvent<Unit> UnitEnter = new UnityEvent<Unit>();
    public UnityEvent<Unit> UnitExit = new UnityEvent<Unit>();

    private void Awake()
    {
        _collider = gameObject.GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = _radius;
    }

    public List<Unit> GetUnitsInArea()
    {
        List<Unit> updatedListUnits = new List<Unit>();
        List<Transform> removeList = new List<Transform>();

        foreach (var unit in _units)
        {
            if (unit.Key != null && unit.Value.gameObject.activeSelf && unit.Value.Health > 0)
                updatedListUnits.Add(unit.Value);
            else
                removeList.Add(unit.Key);
        }

        foreach (var item in removeList)
            _units.Remove(item);

        return updatedListUnits;
    }

    /// <summary>
    /// Print in debag log list Unit in Area
    /// </summary>
    [ContextMenu("Debag print list Units")]
    public void PrintListUnits()
    {
        List<Unit> units = GetUnitsInArea();

        if (units == null || units.Count == 0)
            Debug.Log("Units in sensor not found");

        foreach (var unit in units)
        {
            Debug.Log(unit.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit;

        if (other.TryGetComponent<Unit>(out unit))
        {
            if (!_units.ContainsKey(other.transform))
            {
                _units.Add(other.transform, unit);
                UnitEnter?.Invoke(unit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_units.ContainsKey(other.transform))
        {
            UnitExit?.Invoke(_units[other.transform]);
        }
        
        _units.Remove(other.transform);
    }

    private void OnEnable()
    {
        _units = new Dictionary<Transform, Unit>();
    }

    private void OnDisable()
    {
        _units.Clear();
    }
}
