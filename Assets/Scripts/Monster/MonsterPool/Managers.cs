using UnityEngine;

public static class Managers
{
    private static PoolManager _pool;

    public static PoolManager Pool
    {
        get
        {
            if (_pool == null)
            {
                GameObject obj = new GameObject("PoolManager");
                _pool = obj.AddComponent<PoolManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _pool;
        }
    }
}
