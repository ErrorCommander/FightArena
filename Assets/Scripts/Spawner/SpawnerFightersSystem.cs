using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnerFightersSystem : MonoBehaviour
{
    [SerializeField] private List<SpawnerBoxArea> _spawners;
    [SerializeField] private List<Fighter> _fightersPrefab;
    [SerializeField] [Range(0, 20)] private int _startSpawnCount;

    private void Start()
    {
        CycleSpawner(1);
    }

    private void Spawn()
    {
        foreach (var spawnPoint in _spawners)
        {
            spawnPoint.SpawnUnit(_fightersPrefab[0]);
        }
    }

    private async void CycleSpawner(float delay)
    {
        Spawn();
        await Task.Delay((int)(delay * 1000));
        if(gameObject != null)
            CycleSpawner(delay);
    }
}
