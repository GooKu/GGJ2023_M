using System.Collections.Generic;
using UnityEngine;

public enum PoolExpandMethod
{
    OneAtATime, Double
}

public abstract class UnityObjectFactory<T> where T : Object
{
    public abstract T Create();
}

public class UnityObjectPool<T> where T : Object
{
    private class DefaultFactory : UnityObjectFactory<T>
    {
        private readonly T target;

        public DefaultFactory(T target)
        {
            this.target = target;
        }

        public override T Create()
        {
            return Object.Instantiate(target);
        }
    }

    private static readonly Vector3 POOL_POSITION = new(10000f, 10000f, 10000f);

    public string Name { get; }
    public int Length { get; private set; }
    public Transform PoolParent { get; private set; }

    private readonly UnityObjectFactory<T> factory;
    private readonly bool activeOnInstantiate;
    private readonly PoolExpandMethod expandMethod;

    private readonly List<T> poolObjects = new();
    private readonly List<T> usingObjects = new();

    private int highestInstanceId = 0;

    public UnityObjectPool(T target, int size)
        : this(target, target.name, size, true, PoolExpandMethod.OneAtATime)
    {

    }

    public UnityObjectPool(T target, int size, bool activeOnInstantiate)
        : this(target, target.name, size, activeOnInstantiate, PoolExpandMethod.OneAtATime)
    {

    }

    public UnityObjectPool(T target, int size, PoolExpandMethod expandMethod)
        : this(target, target.name, size, true, expandMethod)
    {

    }

    public UnityObjectPool(T target, int size, bool activeOnInstantiate, PoolExpandMethod expandMethod)
        : this(target, target.name, size, activeOnInstantiate, expandMethod)
    {

    }

    public UnityObjectPool(T target, string poolName, int size)
        : this(target, poolName, size, true, PoolExpandMethod.OneAtATime)
    {

    }

    public UnityObjectPool(T target, string poolName, int size, bool activeOnInstantiate)
        : this(target, poolName, size, activeOnInstantiate, PoolExpandMethod.OneAtATime)
    {

    }

    public UnityObjectPool(T target, string poolName, int size, PoolExpandMethod expandMethod)
        : this(target, poolName, size, true, expandMethod)
    {

    }

    public UnityObjectPool(T target, string poolName, int size, bool activeOnInstantiate, PoolExpandMethod expandMethod)
    {
        factory = new DefaultFactory(target);
        this.activeOnInstantiate = activeOnInstantiate;
        this.expandMethod = expandMethod;

        Name = string.IsNullOrEmpty(poolName) ? target.name : poolName;  // Prevent the poolName is null or empty by any reason
        PoolParent = new GameObject(string.Format("Pool {0}", poolName)).transform;
        for (int i = 0; i < size; i++)
        {
            CreatePoolObject();
        }
    }

    public UnityObjectPool(UnityObjectFactory<T> factory, int size)
        : this(factory, "", size, true, PoolExpandMethod.OneAtATime)
    {

    }

    public UnityObjectPool(UnityObjectFactory<T> factory, int size, bool activeOnInstantiate)
        : this(factory, "", size, activeOnInstantiate, PoolExpandMethod.OneAtATime)
    {

    }

    public UnityObjectPool(UnityObjectFactory<T> factory, int size, PoolExpandMethod expandMethod)
        : this(factory, "", size, true, expandMethod)
    {

    }

    public UnityObjectPool(UnityObjectFactory<T> factory, int size, bool activeOnInstantiate, PoolExpandMethod expandMethod)
        : this(factory, "", size, activeOnInstantiate, expandMethod)
    {

    }

    public UnityObjectPool(UnityObjectFactory<T> factory, string poolName, int size)
        : this(factory, poolName, size, true, PoolExpandMethod.OneAtATime)
    {

    }

    public UnityObjectPool(UnityObjectFactory<T> factory, string poolName, int size, bool activeOnInstantiate)
        : this(factory, poolName, size, activeOnInstantiate, PoolExpandMethod.OneAtATime)
    {

    }

    public UnityObjectPool(UnityObjectFactory<T> factory, string poolName, int size, PoolExpandMethod expandMethod)
        : this(factory, poolName, size, true, expandMethod)
    {

    }

    public UnityObjectPool(UnityObjectFactory<T> factory, string poolName, int size, bool activeOnInstantiate, PoolExpandMethod expandMethod)
    {
        this.factory = factory;
        this.activeOnInstantiate = activeOnInstantiate;
        this.expandMethod = expandMethod;

        Name = string.IsNullOrEmpty(poolName) ? factory.GetType().Name : poolName;  // Prevent the poolName is null or empty by any reason
        PoolParent = new GameObject(string.Format("Pool By {0}", poolName)).transform;
        for (int i = 0; i < size; i++)
        {
            CreatePoolObject();
        }
    }

    private void CreatePoolObject()
    {
        highestInstanceId += 1;
        T obj = factory.Create();
        obj.name = string.Format("{0}_instance{1}", Name, highestInstanceId);

        ResetObject(obj);
        poolObjects.Add(obj);
        Length++;
    }

    private void ResetObject(T obj)
    {
        if (obj is Component)
        {
            Component component = obj as Component;
            component.transform.SetParent(PoolParent);
            component.transform.localPosition = POOL_POSITION;
            component.transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
            component.gameObject.SetActive(activeOnInstantiate);
        }
        else if (obj is GameObject)
        {
            GameObject gameObject = obj as GameObject;
            gameObject.transform.SetParent(PoolParent);
            gameObject.transform.localPosition = POOL_POSITION;
            gameObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
            gameObject.SetActive(activeOnInstantiate);
        }
    }

    public bool IsEmpty()
    {
        if (PoolParent == null)
        {
            return true;
        }

        return poolObjects.Count == 0;
    }

    /// <summary>
    /// 檢查物件是否受此物件池管理
    /// </summary>
    public bool IsObjectBelongToPool(T obj)
    {
        if (obj == null)
        {
            return false;
        }
        else
        {
            return usingObjects.Contains(obj) || poolObjects.Contains(obj);
        }
    }

    /// <summary>
    /// 從物件池取出一個物件
    /// </summary>
    public T Get()
    {
        if (PoolParent == null)
        {
            throw new System.ObjectDisposedException("The pool has been destroyed");
        }

        T obj = null;
        while (obj == null && poolObjects.Count > 0)
        {
            if (poolObjects[0] == null)
            {  // Prevent the object has been destoryed by any reason
                poolObjects.RemoveAt(0);
                Length--;
                continue;
            }
            else
            {
                obj = poolObjects[0];
            }
        }

        // If we cannot get any available object
        if (obj == null)
        {
            // Create new object by the expand method
            if (expandMethod == PoolExpandMethod.OneAtATime)
            {
                CreatePoolObject();
            }
            else if (expandMethod == PoolExpandMethod.Double)
            {
                int expandAmount = highestInstanceId;  // highestInstanceId will increase in CreatePoolObject(), so we need to cache it
                for (int i = 0; i < expandAmount; i++)
                {
                    CreatePoolObject();
                }
            }

            // Decide the returned object
            obj = poolObjects[0];
        }

        poolObjects.RemoveAt(0);
        usingObjects.Add(obj);
        return obj;
    }

    /// <summary>
    /// 回收指定物件
    /// </summary>
    public void Recover(T obj)
    {
        if (PoolParent == null)
        {
            throw new System.ObjectDisposedException("The pool has been destroyed");
        }

        if (obj == null)
        {
            Debug.LogErrorFormat("Recover null into the object pool. Pool Name: {0}", Name);
            return;
        }

        if (!usingObjects.Contains(obj))
        {
            Debug.LogErrorFormat("Recover gameObject which is not belong to this object pool. Pool Name: {0}, Object Name: {1}",
                Name, obj.name);
            return;
        }

        ResetObject(obj);
        poolObjects.Add(obj);
        usingObjects.Remove(obj);
    }

    /// <summary>
    /// 重設此物件池管理的所有物件，並將其全部返回物件池內
    /// </summary>
    public void Reset()
    {
        if (PoolParent == null)
        {
            throw new System.ObjectDisposedException("The pool has been destroyed");
        }

        while (usingObjects.Count > 0)
        {
            T obj = usingObjects[0];
            if (obj == null)
            {  // Prevent the object has been destoryed by any reason
                usingObjects.RemoveAt(0);
                Length--;
            }
            else
            {
                Recover(obj);
            }
        }
    }

    /// <summary>
    /// 銷毀此物件池以及其管理的所有物件
    /// </summary>
    public void Destroy()
    {
        if (PoolParent == null)
        {
            throw new System.ObjectDisposedException("The pool has been destroyed");
        }

        Reset();

        poolObjects.ForEach(p =>
        {
            if (p != null)
            {
                Object.Destroy(p);
            }
        });
        poolObjects.Clear();
        Object.Destroy(PoolParent.gameObject);
        PoolParent = null;
    }
}
