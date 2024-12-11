using App.Scripts.Modules.Commands.General;

namespace App.Scripts.Modules.Commands.Provider
{
    public interface ICommandsProvider
    {
        T GetCommand<T>() where T : ICommand;
    }
}