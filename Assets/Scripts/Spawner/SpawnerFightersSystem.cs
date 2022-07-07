using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerFightersSystem : MonoBehaviour
{
    [SerializeField] private List<SpawnerArea> _spawners;
    [SerializeField] private List<Fighter> _fightersPrefab;
    [SerializeField] private SpawnQueue _spawnQueue = SpawnQueue.Successively;
    [SerializeField] [Range(0, 50)] private int _startSpawnCount;
    public bool IsSpawnsOverTime = true;
    [Range(0.2f, 10)] public float DelaySpawn = 3f;

    public Fighter SpawnFighter => _fightersPrefab[Random.Range(0, _fightersPrefab.Count)];
    [HideInInspector] public UnityEvent<Fighter> OnSpawnFighter;

    private int _indexLastSpawner;
    private uint _indexUnit;
    private float _timer = 0;
    private int _initPoolSize = 4;

    /// <summary>
    /// Add a fighter to be registered in the system
    /// </summary>
    /// <param name="fighter"></param>
    public void AddFighter(Fighter fighter)
    {
        if (fighter != null)
        {
            fighter.name = $"Fighter #{_indexUnit++}";
            OnSpawnFighter?.Invoke(fighter);
        }
    }   

    private void Awake()
    {
        foreach(var unit in _fightersPrefab)
            Pooler.Instance.AddPool(new Pool(unit.gameObject, _initPoolSize));
    }

    private void Start()
    {
        Spawn(_startSpawnCount);
    }

    private void Update()
    {
        if (IsSpawnsOverTime)
        {
            _timer += Time.deltaTime;

            if (DelaySpawn <= _timer)
            {
                _timer = 0;
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        Fighter result;

        switch (_spawnQueue)
        {
            case SpawnQueue.Random:
                _indexLastSpawner = Random.Range(0, _spawners.Count);
                result = _spawners[_indexLastSpawner].SpawnUnit(SpawnFighter);
                break;

            case SpawnQueue.Successively:
                _indexLastSpawner++;
                if (_indexLastSpawner >= _spawners.Count)
                    _indexLastSpawner = 0;
                result = _spawners[_indexLastSpawner].SpawnUnit(SpawnFighter);
                break;

            default:
                goto case SpawnQueue.Successively;
        }

        result.name = $"Fighter #{_indexUnit++}";
        OnSpawnFighter?.Invoke(result);
    }

    private void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
            Spawn();
    }

    private enum SpawnQueue
    {
        Random,
        Successively
    }
}
