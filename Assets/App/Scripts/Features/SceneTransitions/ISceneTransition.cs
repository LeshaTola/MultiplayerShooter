using Cysharp.Threading.Tasks;

namespace App.Scripts.Features.SceneTransitions
{
    public interface ISceneTransition
    {
        public UniTask PlayOnAsync();
        public UniTask PlayOffAsync();
    }
}