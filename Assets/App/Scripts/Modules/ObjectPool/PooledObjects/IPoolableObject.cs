using App.Scripts.Modules.ObjectPool.Pools;

namespace App.Scripts.Modules.ObjectPool.PooledObjects
{
    public interface IPoolableObject<T>
    {
        public void OnGet(IPool<T> pool);
        public void Release();
        public void OnRelease();
    }
}