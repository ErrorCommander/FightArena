using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Pooler : MonoBehaviour
{
    [SerializeField] private List<Pool> _pools;
    private Dictionary<GameObject, Pool> _poolsDictionary;

    public static Pooler Instance => _instance ??= new GameObject("Pooler GameObjects").AddComponent<Pooler>();
    private static Pooler _instance;
    private Pooler() { }

    /// <summary>
    /// Adding a pool or extending the old one if a pool of the specified objects already exists.
    /// </summary>
    /// <param name="pool">Pool with parameters to be created</param>
    /// <returns>true - Create; false - pool already exists</returns>
    public bool AddPool(Pool pool)
    {
        if (_poolsDictionary.ContainsKey(pool.Tag))
        {
            if (_poolsDictionary[pool.Tag].PoolSize < pool.PoolSize)
                _poolsDictionary[pool.Tag].ChangePoolSize(pool.PoolSize);

            return false;
        }

        _poolsDictionary.Add(pool.Tag, pool);
        _pools = _poolsDictionary.Values.ToList();
        pool.Initialize();
        return true;
    }

    /// <summary>
    /// Get array tags of created pools
    /// </summary>
    /// <returns>Array references gameObjects prefabs</returns>
    public GameObject[] GetTags()
    {
        return _poolsDictionary != null ? _poolsDictionary.Keys.ToArray() : null;
    }

    /// <summary>
    /// Take is active gameObject from the pool.
    /// </summary>
    /// <param name="tag">Reference gameObject prefab.</param>
    /// <param name="position">Set position for gameObject.</param>
    /// <param name="rotation">Set rotation for gameObject.</param>
    /// <returns>Reference copy gameObject</returns>
    public GameObject Spawn(GameObject tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolsDictionary.ContainsKey(tag))
            return null;

        var pool = _poolsDictionary[tag];
        var result = pool.TakeGameObject();
        result.transform.SetPositionAndRotation(position, rotation);

        return result;
    }

    /// <summary>
    /// Take is active gameObject from the pool.
    /// </summary>
    /// <param name="tag">Reference gameObject prefab.</param>
    /// <param name="position">Set position for gameObject.</param>
    /// <param name="rotation">Set rotation for gameObject.</param>
    /// <returns>Reference copy gameObject</returns>
    public GameObject Spawn(GameObject tag, Vector3 position, Vector3 rotation)
    {
        return Spawn(tag, position, Quaternion.Euler(rotation));
    }

    /// <summary>
    /// Take is active gameObject from the pool. Rotation - identity.
    /// </summary>
    /// <param name="tag">Reference gameObject prefab.</param>
    /// <param name="position">Set position for gameObject.</param>
    /// <returns>Reference copy gameObject</returns>
    public GameObject Spawn(GameObject tag, Vector3 position)
    {
        return Spawn(tag, position, Quaternion.identity);
    }

    /// <summary>
    /// Delete all gameObject in pool.
    /// </summary>
    /// <param name="tag">Reference gameObject prefab.</param>
    /// <returns>Pool is fuond</returns>
    public bool ClearPool(GameObject tag)
    {
        if (!_poolsDictionary.ContainsKey(tag))
        {
            Debug.LogWarning(tag.name + " Tag not found");
            return false;
        }

        //Debug.Log("Pooler -> clear pool " + tag.name);
        _poolsDictionary[tag].ClearPool();
        return true;
    }

    /// <summary>
    /// Delete all gameObject in pools.
    /// </summary>
    /// <param name="tags"></param>
    public void ClearPools(params GameObject[] tags)
    {
        foreach (var tag in tags)
            ClearPool(tag);
    }

    private bool DestroyPool(GameObject tag)
    {
        if (!_poolsDictionary.ContainsKey(tag))
        {
            Debug.LogWarning(tag.name + " Tag not found");
            return false;
        }

        //Debug.Log("Pooler -> Destroy pool " + tag.name);
        _poolsDictionary[tag].DestroyPool();
        _pools.Remove(_poolsDictionary[tag]);
        _poolsDictionary.Remove(tag);
        return true;
    }

    private void DestroyPool(params GameObject[] tags)
    {
        if (tags == null)
            return;

        foreach (var tag in tags)
            DestroyPool(tag);
    }

    private void Initialize()
    {
        if (_instance == null)
        {
            _instance = this;
            _poolsDictionary = new Dictionary<GameObject, Pool>();
            _pools ??= new List<Pool>();
            SceneManager.sceneUnloaded += UnloadScene;
            foreach (var pool in _pools)
                AddPool(pool);
        }
        else if (_pools != null)
        {
            foreach (var pool in _pools)
                _instance.AddPool(pool);

            Destroy(gameObject);
        }
    }

    private void DestroyAllPools()
    {
        if (_pools == null || _pools.Count == 0)
            return;

        DestroyPool(GetTags());
        _poolsDictionary = null;
        _pools = null;
    }

    private void Awake()
    {
        Initialize();
        Debug.Log(name + "  Created");
    }

    private void OnDestroy()
    {
        DestroyAllPools();
        Destroy(gameObject);
    }

    private void UnloadScene(Scene arg0)
    {
        Debug.Log("UnloadScene -> _instance clear");
        _instance = null;
        SceneManager.sceneUnloaded -= UnloadScene;
    }
}