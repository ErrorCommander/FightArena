using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _target;

    private void Awake()
    {
        _target = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _target.forward);
    }
}
