using System;
using App.Scripts.Modules.Commands.General;

namespace App.Scripts.Features.Commands
{
    public class CustomCommand : LabeledCommand
    {
        private readonly Action _action;

        public CustomCommand(string label, Action action) : base(label)
        {
            _action = action;
        }

        public override void Execute()
        {
            _action?.Invoke();
        }
    }
}