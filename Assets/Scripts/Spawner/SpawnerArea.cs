using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerArea : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _spawnArea;
    [SerializeField] private float _spawnDelay;

    private void OnDrawGizmos()
    {
        
    }
}
