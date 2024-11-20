using System.Collections.Generic;

namespace App.Scripts.Modules.StateMachine.Services.CleanupService
{
    public interface ICleanupService
    {
        List<ICleanupable> Cleanupables { get; }

        void Cleanup();
    }
}