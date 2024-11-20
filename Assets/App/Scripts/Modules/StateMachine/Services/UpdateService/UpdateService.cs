using System.Collections.Generic;

namespace App.Scripts.Modules.StateMachine.Services.UpdateService
{
    public class UpdateService : IUpdateService
    {
        public List<IUpdatable> Updatables { get; }

        public UpdateService(List<IUpdatable> updatables)
        {
            Updatables = updatables;
        }

        public void Update()
        {
            foreach (var updatable in Updatables)
            {
                updatable.Update();
            }
        }
    }
}