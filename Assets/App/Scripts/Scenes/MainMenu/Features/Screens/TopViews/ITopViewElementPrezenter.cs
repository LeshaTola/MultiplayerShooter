using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.TopViews
{
    public interface ITopViewElementPrezenter
    {
        public UniTask Show();

        public UniTask Hide();
    }
}