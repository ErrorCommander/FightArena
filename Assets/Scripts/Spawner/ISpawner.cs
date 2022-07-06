using UnityEngine;

public interface ISpawner
{
    /// <summary>
    /// Spawn copy of the prefeb Fighter
    /// </summary>
    /// <param name="prefab">Refeb Fighter for spawn</param>
    /// <returns>Transform created Fighter</returns>
    public Unit SpawnUnit(Unit prefab);
}