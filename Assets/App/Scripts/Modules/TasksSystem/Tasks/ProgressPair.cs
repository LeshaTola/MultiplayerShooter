using System;

namespace App.Scripts.Modules.TasksSystem.Tasks
{
    [Serializable]
    public struct ProgressPair
    {
        public int Progress;
        public int Target;

        public ProgressPair(int progress, int target)
        {
            Progress = progress;
            Target = target;
        }
    }
}