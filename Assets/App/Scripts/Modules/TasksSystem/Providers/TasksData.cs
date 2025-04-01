using System;
using System.Collections.Generic;
using App.Scripts.Modules.TasksSystem.Tasks;

namespace App.Scripts.Modules.TasksSystem.Providers
{
    [Serializable]
    public class TasksData
    {
        public long LastUpdateDate;
        public List<TaskContainerData> Tasks;
    }
}