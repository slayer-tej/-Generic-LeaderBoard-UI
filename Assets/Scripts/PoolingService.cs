using System.Collections.Generic;

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
                return pooledItem.item;
            }
        }
        return CreateNewPooledItem();
    }

    public virtual void ReturnItem(T item)
    {
        poolingItem<T> poolingItem = pooledItems.Find(i => i.item.Equals(item));
        poolingItem.isActive = false;
    }

    private T CreateNewPooledItem()
    {
        poolingItem<T> poolItem = new poolingItem<T>();
        poolItem.item = CreateItem();
        poolItem.isActive = true;
        pooledItems.Add(poolItem);
        //print(pooledItems.Count + "Instance Created" );
        return poolItem.item;
    }

    protected virtual T CreateItem()
    {
        return (T)null;
    }

    public class poolingItem<U>
    {
        public U item;
        public bool isActive;
    }
}