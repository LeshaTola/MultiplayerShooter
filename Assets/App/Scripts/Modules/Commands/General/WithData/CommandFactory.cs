using System;
using System.Collections.Generic;
using Zenject;

namespace App.Scripts.Modules.Commands.General.WithData
{
    public class CommandFactory
    {
        private readonly DiContainer _container;

        public CommandFactory(DiContainer container)
        {
            _container = container;
        }

        public CommandWithData GetCommand(CommandWithData commandWithData)
        {
            return (CommandWithData) _container.Instantiate(commandWithData.GetType(),new List<object>()
            {
                commandWithData.Data
            });
        }

        public List<CommandWithData> GetCommands(List<CommandWithData> originalCommands)
        {
            List<CommandWithData> commands = new List<CommandWithData>();
            foreach (var command in originalCommands)
            {
                commands.Add(GetCommand(command));
            }
            return commands;
        }
    }
}