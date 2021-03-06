using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerBoxArea : SpawnerArea
{
    [SerializeField] private Vector2 _point1;
    [SerializeField] private Vector2 _point2;

    public override Fighter SpawnUnit(Fighter prefab)
    {
        Vector3 pos = GetSpawnPoint();
        Fighter unit = _pooler.Spawn(prefab.gameObject, pos, Quaternion.identity).GetComponent<Fighter>();
        return unit;
    }

    private Vector3 GetSpawnPoint()
    {
        float rndX = Random.Range(transform.position.x + _point1.x, transform.position.x + _point2.x);
        float rndZ = Random.Range(transform.position.z + _point1.y, transform.position.z + _point2.y);
        NavMeshHit hit;

        Vector3 pos = new Vector3(rndX, transform.position.y, rndZ);
        NavMesh.SamplePosition(pos, out hit, 5f, 1);
        pos = hit.position;

        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 point1 = new Vector3(transform.position.x + _point1.x, transform.position.y, transform.position.z + _point1.y);
        Vector3 point2 = new Vector3(transform.position.x + _point1.x, transform.position.y, transform.position.z + _point2.y);
        Vector3 point3 = new Vector3(transform.position.x + _point2.x, transform.position.y, transform.position.z + _point1.y);
        Vector3 point4 = new Vector3(transform.position.x + _point2.x, transform.position.y, transform.position.z + _point2.y);

        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point2, point3);
        Gizmos.DrawLine(point3, point4);
        Gizmos.DrawLine(point4, point1);
        Gizmos.DrawLine(point1, point3);
        Gizmos.DrawLine(point2, point4);
    }
}
