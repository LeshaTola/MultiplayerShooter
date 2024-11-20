using System.Collections.Generic;

namespace App.Scripts.Modules.StateMachine.Services.UpdateService
{
    public interface IUpdateService
    {
        List<IUpdatable> Updatables { get; }

        void Update();
    }
}