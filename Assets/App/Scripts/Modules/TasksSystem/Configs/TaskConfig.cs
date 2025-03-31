using System.Collections.Generic;
using App.Scripts.Modules.TasksSystem.CompleteActions;
using App.Scripts.Modules.TasksSystem.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.TasksSystem.Configs
{
    [CreateAssetMenu(fileName = "TaskConfig", menuName = "Configs/Tasks/Task")]
    public class TaskConfig : SerializedScriptableObject
    {
        [field: SerializeField, ReadOnly] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public List<Task> Tasks { get; set; }
        [field: SerializeField] public List<CompleteAction> CompleteActions { get; set; }
        
        private void OnValidate()
        {
            Id = name;
        }
    }
}