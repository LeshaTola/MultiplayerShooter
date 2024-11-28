using Cysharp.Threading.Tasks;

namespace App.Scripts.Features.Screens
{
    public class GameScreenPresenter
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
            return UniTask.CompletedTask;

        }

        public virtual UniTask Hide()
        {
            return UniTask.CompletedTask;
        }
    }
}