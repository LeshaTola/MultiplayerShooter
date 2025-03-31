using System.Collections.Generic;
using App.Scripts.Modules.TasksSystem.Tasks;
using Zenject;

namespace App.Scripts.Modules.TasksSystem.Factories
{
    public class TaskFactory
    {
        private readonly DiContainer diContainer;

        public TaskFactory(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public Task GetTask(Task original)
        {
            var newTask = (Task) diContainer.Instantiate(original.GetType());
            newTask.Import(original);
            return newTask;
        }

        public List<Task> GetTasks(List<Task> originals)
        {
            List<Task> newTasks = new();
            foreach (var original in originals)
            {
                newTasks.Add(GetTask(original));
            }
            return newTasks;
        }
    }
}