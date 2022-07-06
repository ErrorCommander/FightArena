using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerArea : MonoBehaviour, ISpawner
{
    protected Pooler _pooler;
    /// <summary>
    /// Spawn copy of the prefeb Fighter in certain area
    /// </summary>
    /// <param name="prefab">Refeb Fighter for spawn</param>
    /// <returns>Transform created Fighter</returns>
    public abstract Unit SpawnUnit(Unit prefab);

    protected void Awake()
    {
        _pooler = Pooler.Instance;
    }
}
