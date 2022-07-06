using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InputSpawn : MonoBehaviour
{
    private InputPlayer _input;
    private Camera _camera;
    private float _marginError = 5f;

    private void Awake()
    {
        _input = new InputPlayer();
        _input.Player.Spawn.performed += Click;
        _camera = Camera.main;
    }

    private void Click(UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        Vector2 screenPosition = input.ReadValue<Vector2>();
        Debug.Log("Click pos " + screenPosition);
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

        Debug.Log("Spawn in " + worldPosition);
        Debug.DrawLine(worldPosition, worldPosition + Vector3.up * 5, Color.red, 10f);
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
