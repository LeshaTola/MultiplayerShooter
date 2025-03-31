using App.Scripts.Modules.TasksSystem.Configs;
using App.Scripts.Modules.TasksSystem.Tasks;
using UnityEngine;

namespace App.Scripts.Modules.TasksSystem.Factories
{
    public class TasksContainerFactory
    {
        private readonly CompleteActionFactory completeActionFactory;
        private readonly TaskFactory taskFactory;

        public TasksContainerFactory(CompleteActionFactory completeActionFactory, TaskFactory taskFactory)
        {
            this.completeActionFactory = completeActionFactory;
            this.taskFactory = taskFactory;
        }

        public TasksContainer GetTaskContainer(TaskConfig config)
        {
            var newConfig = Object.Instantiate(config);
            newConfig.Tasks = taskFactory.GetTasks(config.Tasks);
            newConfig.CompleteActions = completeActionFactory.CreateCompleteActions(config.CompleteActions);
            
            return new TasksContainer(newConfig);
        }
    }
}