using System.Collections.Generic;
using System;
using UnityEngine;

public class PoolingService<T> : MonoSingletonGeneric<PoolingService<T>> where T : class
{
    private List<poolingItem<T>> pooledItems = new List<poolingItem<T>>();

    public virtual T GetItem()
    {
        if (pooledItems.Count > 0)
        {
            poolingItem<T> pooledItem = pooledItems.Find(i => !i.isActive);
            if (pooledItem != null)
            {
                pooledItem.isActive = true;
                Debug.Log("Got item from Pool");
                return pooledItem.item;
            }
        }
        return CreateNewPooledItem();
    }

    public virtual void ReturnItem(T item)
    {
        poolingItem<T> poolingItem = pooledItems.Find(i => i.item.Equals(item));
        poolingItem.isActive = false;
        Debug.Log("Returned Item to pool");
    }

    private T CreateNewPooledItem()
    {
        poolingItem<T> poolItem = new poolingItem<T>();
        poolItem.item = CreateItem();
        poolItem.isActive = true;
        pooledItems.Add(poolItem);
        return poolItem.item;
    }

    protected virtual T CreateItem()
    {
        return (T)null;
    }

    public class poolingItem<T>
    {
        public T item;
        public bool isActive;
    }
}