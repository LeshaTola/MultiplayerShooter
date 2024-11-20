using System.Collections.Generic;

namespace App.Scripts.Modules.StateMachine.Services.InitializeService
{
    public interface IInitializeService
    {
        List<IInitializable> Initializables { get; }

        void Initialize();
    }
}