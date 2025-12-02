using System;

namespace App.Scripts.Modules.Commands.General.WithData
{
    public class CommandData
    {
    }

    [Serializable]
    public abstract class CommandWithData : ICommand
    {
        public virtual CommandData Data { get; }

        public CommandWithData(CommandData data)
        {
            Data = data;
        }

        public abstract void Execute();
    }

}