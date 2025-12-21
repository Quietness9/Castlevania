using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] bool _isDontDestroyOnLoad = false;
    [SerializeField] List<GameObject> _objectParent = new();


    GameObject _emptyHolder;

    static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools = new();
    static Dictionary<GameObject, GameObject> _cloneToPrefabMap = new();
    static Dictionary<PoolType,GameObject> _objectParentDir=new();


    public static PoolType PoolingType;

    private void Awake()
    {
        InitPoolParent();
    }

    /// <summary>
    /// 初始化父对象
    /// </summary>
    private void InitPoolParent()
    {
        _objectParentDir.Add(PoolType.CraftListObject, _objectParent[0]);
        _objectParentDir.Add(PoolType.GameObject, _objectParent[1]);

        _emptyHolder = new GameObject("Object Pools");

        //_gameObjectsEmpty = new GameObject("GameObject Pools");
        _objectParent[1].transform.SetParent(_emptyHolder.transform, false);


        if (_isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(_objectParent[1].transform.root);
        }
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="poolType"></param>
    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObject)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>
            (
                createFunc: () => CreateObject(prefab, pos, rot, poolType),
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: OnDestroyObject

            );

        _objectPools.Add(prefab, pool);
    }

    /// <summary>
    /// 创建对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="poolType"></param>
    /// <returns></returns>
    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, pos, rot);

        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);

        if(parentObject != null)
        {
            obj.transform.SetParent(parentObject.transform, false);
        }
        else
        {
            Debug.LogWarning("没有此类型的父对象"+poolType.ToString());
        }
        

        return obj;
    }

    /// <summary>
    /// 获得其父对象
    /// </summary>
    /// <param name="poolType"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static GameObject SetParentObject(PoolType poolType)
    {
        if (_objectParentDir.ContainsKey(poolType))
        {
            return _objectParentDir[poolType];
        }

        return null;
    }

    /// <summary>
    /// 获得对象
    /// </summary>
    /// <param name="object"></param>
    private static void OnGetObject(GameObject obj)
    {
        //当需要在获得对象时增加其他逻辑加在下面
    }

    /// <summary>
    /// 返回对象
    /// </summary>
    /// <param name="object"></param>
    /// <exception cref="NotImplementedException"></exception>
    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    /// <summary>
    /// 销毁对象
    /// </summary>
    /// <param name="obj"></param>
    private static void OnDestroyObject(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
        {
            _cloneToPrefabMap.Remove(obj);
        }
    }

    /// <summary>
    /// 生产需要的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectToSpawn"></param>
    /// <param name="spawnPos"></param>
    /// <param name="spawnRotation"></param>
    /// <param name="poolType"></param>
    /// <returns></returns>
    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos,
        Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);
        }

        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            //obj.transform.position = spawnPos;
            //obj.transform.rotation = spawnRotation;
            obj.transform.SetPositionAndRotation(spawnPos, spawnRotation);
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.Log($"Object {objectToSpawn.name} 没有组件类型{typeof(T)}");
                return null;
            }

            return component;
        }

        return null;
    }

    /// <summary>
    /// 返回组件版本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typePrefab"></param>
    /// <param name="spawnPos"></param>
    /// <param name="spawnRotation"></param>
    /// <param name="poolType"></param>
    /// <returns></returns>
    public static T SpawnObject<T>(T typePrefab, Vector3 spawnPos,
        Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T : Component
    {
        return SpawnObject<T>(typePrefab, spawnPos, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject gameObject, Vector3 spawnPos,
        Quaternion spawnRotation, PoolType poolType = PoolType.GameObject)
    {
        return SpawnObject<GameObject>(gameObject, spawnPos, spawnRotation, poolType);
    }


    /// <summary>
    /// 返回对象池
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="poolType"></param>
    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObject)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);

            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("尝试返回一个没有池的对象" + obj.name);
        }
    }
}
