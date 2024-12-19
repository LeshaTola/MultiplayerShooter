using UnityEngine;

namespace App.Scripts.Modules.Factories.MonoFactories
{
    public class MonoFactory<T> : IFactory<T> where T : MonoBehaviour
    {
        private T template;
        private Transform parent = null;

        public MonoFactory(T template)
        {
            this.template = template;
        }

        public MonoFactory(T template, Transform parent)
        {
            this.template = template;
            this.parent = parent;
        }

        public virtual T GetItem()
        {
            return Object.Instantiate(template, parent);
        }
    }
}