using App.Scripts.Modules.Commands.General;
using Zenject;

namespace App.Scripts.Modules.Commands.Provider
{
    public class CommandsProvider : ICommandsProvider
    {
        private DiContainer diContainer;

        public CommandsProvider(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public T GetCommand<T>() where T : ICommand
        {
            return (T) diContainer.Resolve(typeof(T));
        }
    }
}