using System.Collections.Generic;

namespace App.Scripts.Modules.StateMachine.Services.CleanupService
{
    public class CleanupService : ICleanupService
    {
        public List<ICleanupable> Cleanupables { get; private set; }

        public CleanupService(List<ICleanupable> cleanupables)
        {
            Cleanupables = cleanupables;
        }

        public void Cleanup()
        {
            foreach (var cleanupable in Cleanupables)
            {
                cleanupable.Cleanup();
            }
        }
    }
}