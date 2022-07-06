using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnOnClick : MonoBehaviour
{
    [SerializeField] private SpawnerFightersSystem _spawner;
    private InputPlayer _input;
    private Camera _camera;
    private float _marginError = 5f;
    private Pooler _pooler;

    private void Awake()
    {
        _input = new InputPlayer();
        _input.Player.Spawn.performed += Click;
        _camera = Camera.main;
        _pooler = Pooler.Instance;
    }

    private void Click(UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        if (_spawner == null)
        {
            Debug.LogWarning("Spawner not specified");
            return;
        }

        Vector2 screenPosition = input.ReadValue<Vector2>();
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        plane.Raycast(ray, out distance);
        Vector3 worldPosition = ray.GetPoint(distance);

        NavMeshHit hit;
        NavMesh.SamplePosition(worldPosition, out hit, _marginError, 1);
        worldPosition = hit.position;

        if (worldPosition.x == Mathf.Infinity)
        {
            Debug.Log("Not correct spawn point");
            return;
        }

        var unit = _pooler.Spawn(_spawner.SpawnFighter.gameObject, worldPosition, Quaternion.identity);
        _spawner.AddFighter(unit.GetComponent<Fighter>());
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
