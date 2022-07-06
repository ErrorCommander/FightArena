using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnControlUI : MonoBehaviour
{
    [SerializeField] private SpawnerFightersSystem _spawner;
    [SerializeField] private Toggle _enableSpawn;
    [SerializeField] private Slider _speedSpawn;
    [SerializeField] [Range(1, 5)] private float _delayMultipler = 4;
    [SerializeField] private AnimationCurve _spawnDelay;

    private void ChangeSpawnSpeed(float speednormalizedValue)
    {
        float delay = _spawnDelay.Evaluate(speednormalizedValue);
        _spawner.DelaySpawn = delay * _delayMultipler;
    }
    
    private void ChangeSpawnEnable(bool enableSpawn)
    {
        _spawner.IsSpawnsOverTime = enableSpawn;
    }

    private void OnEnable()
    {
        _enableSpawn.isOn = _spawner.IsSpawnsOverTime;

        _speedSpawn.onValueChanged.AddListener(ChangeSpawnSpeed);
        _enableSpawn.onValueChanged.AddListener(ChangeSpawnEnable);
    }

    private void OnDisable()
    {
        _speedSpawn.onValueChanged.RemoveListener(ChangeSpawnSpeed);
        _enableSpawn.onValueChanged.RemoveListener(ChangeSpawnEnable);
    }
}
