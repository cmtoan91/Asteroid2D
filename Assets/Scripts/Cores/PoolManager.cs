using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool
{
    public Transform Parent { get; set; }

    public List<Component> PooledGO = new List<Component>();
}
public class PoolManager 
{
    static Dictionary<string, ObjectPool> _pools = new Dictionary<string, ObjectPool>();

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        SceneManager.sceneUnloaded += (scene) => ClearPool();
    }
    public static string GetName<T>(T prefab) where T: Component
    {
        if (prefab == null) return "";
        return prefab.gameObject.name;
    }
    public static void PoolGameObject<T>(T prefab, string nameMod = "", int count = 1) where T: Component
    {
        string name = GetName(prefab) + nameMod + "(Clone)";
        if (!_pools.ContainsKey(name))
        {
            GameObject go = new GameObject(name + " || GO Pool");
            ObjectPool goPool = new ObjectPool();
            goPool.Parent = go.transform;       
            _pools[name] = goPool;
        }

        for (int i = 0; i < count; i++)
        {
            T toPool = Object.Instantiate(prefab, _pools[name].Parent);
            _pools[name].PooledGO.Add(toPool);
            toPool.gameObject.SetActive(false);
        }
    }

    public static T GetInstance<T>(T prefab, string nameMod = "") where T : Component
    {
        string name = GetName(prefab) + nameMod + "(Clone)";

        if (!_pools.ContainsKey(name))
        {
            PoolGameObject(prefab, nameMod, 1);
        }

        if(_pools[name].PooledGO.Count <= 0)
        {
            T toPool = Object.Instantiate(prefab);
            toPool.name = name;
            return toPool;
        }
        else
        {
            T instance = _pools[name].PooledGO[0] as T;
            instance.transform.parent = null;
            instance.gameObject.SetActive(true);
            _pools[name].PooledGO.RemoveAt(0);
            return instance;
        }
    }

    public static GameObject GetInstanceGameObject<T>(T prefab, string nameMod = "") where T : Component
    {
        string name = GetName(prefab) + nameMod;

        if (!_pools.ContainsKey(name))
        {
            PoolGameObject(prefab, nameMod, 1);
        }

        if (_pools[name].PooledGO.Count <= 0)
        {
            T toPool = Object.Instantiate(prefab);
            return toPool.gameObject;
        }
        else
        {
            T instance = _pools[name].PooledGO[0] as T;
            instance.transform.parent = null;
            instance.gameObject.SetActive(true);
            _pools[name].PooledGO.RemoveAt(0);
            return instance.gameObject;
        }
    }


    public static void ReleaseInstance<T>(T instance) where T : Component
    {
        string name = GetName(instance);

        if (_pools.TryGetValue(name, out var ops))
        {
            instance.transform.SetParent(ops.Parent);
            instance.gameObject.SetActive(false);
            ops.PooledGO.Add(instance);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = Vector3.one;
        }
        else
        {
            if(instance != null)
                Object.Destroy(instance.gameObject);
        }
    }

    public static void ClearPool() 
    {
        foreach(string name in _pools.Keys) 
        {
            if (_pools[name].PooledGO.Count > 0)
            {
                foreach(Component go in _pools[name].PooledGO)
                {
                    if(go != null) Object.Destroy(go);
                }
                _pools[name].PooledGO.Clear();
            }
        }
        _pools.Clear();
    }
}
