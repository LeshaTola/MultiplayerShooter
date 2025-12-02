using App.Scripts.Modules.PopupAndViews.Popups.YesNo;
using UnityEngine;
using Zenject;

namespace App.Scripts.Modules.OtherPlatforms.Android
{
    public class ExitButton:MonoBehaviour
    {
        private YesNoPopupRouter _yesNoPopupRouter;

        [Inject]
        public void Construct(YesNoPopupRouter yesNoPopupRouter)
        {
            _yesNoPopupRouter = yesNoPopupRouter;
        }
#if ANDROID

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _yesNoPopupRouter.ShowPopup(new()
                {
                    Header = ConstStrings.ATTENTION,
                    Message = ConstStrings.SURE_EXIT,
                    YesCommand = new CustomCommand(ConstStrings.YES, () => Application.Quit()),
                    NoCommand = new CustomCommand(ConstStrings.NO, () => _yesNoPopupRouter.HidePopup().Forget()),
                }).Forget();
            }
        }
#endif
    }
}