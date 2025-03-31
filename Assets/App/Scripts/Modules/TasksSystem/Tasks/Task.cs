using System;

namespace App.Scripts.Modules.TasksSystem.Tasks
{
    public abstract class Task : ITask
    {
        public event Action<ITask> OnTaskCompleted;
        public event Action<float> OnProgressChanged;

        private float _progress;

        public float Progress
        {
            get => _progress;
            protected set
            {
                if (_progress.Equals(value))
                {
                    return;
                }
                
                _progress = value;
                OnProgressChanged?.Invoke(_progress);

                if (_progress >= 1f)
                {
                    Complete();
                }
            }
        }

        public virtual void Start()
        {
        }

        public virtual void Complete()
        {
            OnTaskCompleted?.Invoke(this);
        }

        public abstract ProgressPair GetProgress();
        
        public abstract void SetProgress(ProgressPair progress);

        public virtual void Import(Task original)
        {
        }
    }

}