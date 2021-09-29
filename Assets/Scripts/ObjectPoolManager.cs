using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPoolManager : MonoBehaviour
{
    #region Pool
    class Pool
    {
        
        private string _objectName;
        public string ObjectName
        {
            get { return _objectName; }
            private set { _objectName = value; }
        }

        public GameObject Prefab;
        public int Count;
        public int Available
        {
            get { return objects.Count; }
        }
        private Stack<GameObject> objects;

        public Pool(GameObject gameObject, int count)
        {
            Prefab = gameObject;
            objects = new Stack<GameObject>();
            for (int i = 0; i < count; i++)
            {
                CreateCopy();
            }
            ObjectName = gameObject.name;
        }

        private void CreateCopy()
        {
            GameObject copy = Transform.Instantiate(Prefab, Instance.transform);
            copy.SetActive(false);
            objects.Push(copy);
        }

        public Pool(PoolInfo info) : this(info.Prefab, info.Count) { }

        public Pool(GameObject gameObject) : this(gameObject, 0) { }
        
        public GameObject GetCopy()
        {
            if (Available <= 0)
            {
                CreateCopy();
            }
            return objects.Pop();
            
        }

        public void ReturnCopy(GameObject gameObject)
        {
            objects.Push(gameObject);
            gameObject.transform.parent = Instance.transform;
            gameObject.SetActive(false);
        }
    }
    #endregion
    [Serializable]
    public struct PoolInfo 
    {
        public GameObject Prefab;
        public int Count;
    }

    public List<PoolInfo> Infos;
    private static ObjectPoolManager _instance;
    private List<Pool> Pools;
    
    public static ObjectPoolManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

   
    void Start()
    {
        Pools = new List<Pool>();
        foreach (PoolInfo item in Infos)
        {
            Pools.Add(new Pool(item));
        }
    }

    /// <summary>
    /// Not Safe, use only if you 100% shure object already instantiated in PoolManager
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="pos"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    //public static GameObject GetObject(string Name,Vector3 pos, Quaternion rotation) If human can make mistake He WILL make mistake
    //{ //TODO make with recources
    //    GameObject obj = Instance.PoolFind(Name).GetCopy();
    //    obj.transform.position = pos;
    //    obj.transform.rotation = rotation;
    //    obj.SetActive(true);
    //    return obj;
    //}

    //private Pool PoolFind(string Name)
    //{
    //    Pool pool = Instance.Pools.Find(p => p.ObjectName == Name);
    //    if (pool == null)
    //    {
    //        throw new Exception("Object has not been Instantiated in ObjectPoolManager you moron");
    //    }
    //    return pool;
    //}

    private Pool PoolFind(GameObject gameObject)
    {
        Pool pool = Instance.Pools.Find(p => p.ObjectName == gameObject.name);
        if (pool == null)
        {
            pool = new Pool(gameObject);
            Instance.Pools.Add(pool);
        }
        return pool;
    }

    public static GameObject GetObject(GameObject gameObject)
    {
        return Instance.Pools.Find(p => p.ObjectName == gameObject.name).GetCopy();
    }

    public static GameObject GetObject(GameObject gameObject,Vector3 pos,Quaternion rotation)
    {
        GameObject obj = GetObject(gameObject);
        obj.transform.position = pos;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;

    }

    //TODO Find more clear solution
    public static void ReturnCopy(GameObject gameObject)
    {
        Pool pool = Instance.PoolFind(gameObject);
        pool.ReturnCopy(gameObject);
    }
   
}
