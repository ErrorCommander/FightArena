using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerFightersSystem : MonoBehaviour
{
    [SerializeField] private List<SpawnerBoxArea> _spawners;
    [SerializeField] private List<Fighter> _fightersPrefab;
    [SerializeField] private SpawnQueue _spawnQueue = SpawnQueue.Successively;
    [SerializeField] [Range(0, 50)] private int _startSpawnCount;
    [SerializeField] private bool _isSpawnsOverTime;
    [SerializeField] [Range(0.2f, 10)] private float _delaySpawn = 3f;

    [HideInInspector] public UnityEvent<Unit> OnSpawnFighter;

    private int _indexLastSpawner;
    private uint _indexUnit;
    private float _timer = 0;

    private void Start()
    {
        Spawn(_startSpawnCount);
    }

    private void Update()
    {
        if (_isSpawnsOverTime)
        {
            _timer += Time.deltaTime;

            if (_delaySpawn <= _timer)
            {
                _timer = 0;
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        Unit result;

        switch (_spawnQueue)
        {
            case SpawnQueue.Random:
                _indexLastSpawner = Random.Range(0, _spawners.Count);
                result = _spawners[_indexLastSpawner].SpawnUnit(_fightersPrefab[Random.Range(0, _fightersPrefab.Count)]);
                break;

            case SpawnQueue.Successively:
                _indexLastSpawner++;
                if (_indexLastSpawner >= _spawners.Count)
                    _indexLastSpawner = 0;
                result = _spawners[_indexLastSpawner].SpawnUnit(_fightersPrefab[Random.Range(0, _fightersPrefab.Count)]);
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
