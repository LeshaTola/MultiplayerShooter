using App.Scripts.Modules.Commands.General;

namespace App.Scripts.Modules.PopupAndViews.Popups.YesNo
{
    public class YesNoPopupData
    {
        public string Header;
        public string Message;
        public ILabeledCommand YesCommand;
        public ILabeledCommand NoCommand;
    }
}