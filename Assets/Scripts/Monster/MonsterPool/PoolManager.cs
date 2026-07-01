using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<Type, Queue<Component>> poolDictionary = new Dictionary<Type, Queue<Component>>();


    private Dictionary<Type, Transform> poolParents = new Dictionary<Type, Transform>();

    private Transform poolRoot;

    private void Awake()
    {
        CreatePoolRoot();
    }

    private void CreatePoolRoot()
    {
        GameObject rootObj = new GameObject("PoolRoot");

        rootObj.transform.SetParent(transform);

        poolRoot = rootObj.transform;
    }

    public void PreloadPool<T>(T prefab, int count) where T : Component
    {

        Type type = typeof(T);

        CreatePool(type);

        for (int i = 0; i < count; i++)
        {
            T obj = Instantiate(prefab);

            obj.gameObject.SetActive(false);

            obj.transform.SetParent(poolParents[type]);

            poolDictionary[type].Enqueue(obj);
        }

    }

    public T GetPool<T>(T prefab) where T : Component
    {
        Type type = typeof(T);

        CreatePool(type);

        T obj = null;

        if (poolDictionary[type].Count > 0)
        {
            obj = poolDictionary[type].Dequeue() as T;
        }
        else
        {
            obj = Instantiate(prefab);
            obj.transform.SetParent(poolParents[type]);
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnPool<T>(T obj) where T : Component
    {
        Type type = typeof(T);

        CreatePool(type);

        obj.gameObject.SetActive(false);

        if (obj.transform.parent != poolParents[type])
        {
            obj.transform.SetParent(poolParents[type]);
        }
        poolDictionary[type].Enqueue(obj);
    }

    private void CreatePool(Type type)
    {
        if (poolDictionary.ContainsKey(type))
        {
            return;
        }
        poolDictionary.Add(type, new Queue<Component>());

        CreatePoolParent(type);
    }

    private void CreatePoolParent(Type type)
    {
        GameObject parentObj = new GameObject(type.Name);

        parentObj.transform.SetParent(poolRoot);

        poolParents.Add(type, parentObj.transform);
    }
}