using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFighters : MonoBehaviour
{
    [SerializeField] private List<SpawnerArea> _spawners;
    [SerializeField] private List<Fighter> _fighters;
    [SerializeField] [Range(0, 20)] private int _startSpawnCount;


}
