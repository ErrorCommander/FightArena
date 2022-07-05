using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnerFightersSystem : MonoBehaviour
{
    [SerializeField] private List<SpawnerBoxArea> _spawners;
    [SerializeField] private List<Fighter> _fightersPrefab;
    [SerializeField] private SpawnQueue _spawnQueue = SpawnQueue.Successively;
    [SerializeField] [Range(0, 50)] private int _startSpawnCount;

    private int _indexLastSpawner;

    private void Start()
    {
        Spawn(_startSpawnCount);
    }

    private void Spawn()
    {
        switch (_spawnQueue)
        {
            case SpawnQueue.Random:
                _indexLastSpawner = Random.Range(0, _spawners.Count);
                _spawners[_indexLastSpawner].SpawnUnit(_fightersPrefab[Random.Range(0, _fightersPrefab.Count)]);
                break;

            case SpawnQueue.Successively:
                _indexLastSpawner++;
                if (_indexLastSpawner >= _spawners.Count)
                    _indexLastSpawner = 0;
                _spawners[_indexLastSpawner].SpawnUnit(_fightersPrefab[Random.Range(0, _fightersPrefab.Count)]);
                break;

            default:
                goto case SpawnQueue.Successively;
        }
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
