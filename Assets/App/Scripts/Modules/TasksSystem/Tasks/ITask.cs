using System;

namespace App.Scripts.Modules.TasksSystem.Tasks
{
    public interface ITask
    {
        public event Action<ITask> OnTaskCompleted;
        public event Action<float> OnProgressChanged;
        
        public float Progress { get;}
        public void Complete();
    }
}