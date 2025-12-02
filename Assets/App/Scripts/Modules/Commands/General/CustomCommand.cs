using System;

namespace App.Scripts.Modules.Commands.General
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