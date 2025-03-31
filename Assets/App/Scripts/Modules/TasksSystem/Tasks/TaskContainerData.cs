using System;
using System.Collections.Generic;
using App.Scripts.Modules.TasksSystem.Configs;
using UnityEngine.Serialization;

namespace App.Scripts.Modules.TasksSystem.Tasks
{
    [Serializable]
    public class TaskContainerData
    {
        [NonSerialized]
        public TaskConfig TaskConfig;
        
        public string TaskConfigId;
        public ProgressPair Progress;
        public List<ProgressPair> TasksData;
    }
}