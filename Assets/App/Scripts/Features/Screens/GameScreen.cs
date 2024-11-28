using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Features.Screens
{
    public abstract class GameScreen : MonoBehaviour
    {

        public virtual void Initialize()
        {
            
        }

        public virtual void Default()
        {
            
        }
        
        public virtual void Cleanup()
        {
            
        }

        public virtual UniTask Show()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}