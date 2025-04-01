using UnityEngine;

namespace App.Scripts.Modules.TasksSystem.Configs
{
    [CreateAssetMenu(fileName = "TaskProviderConfig", menuName = "Configs/Tasks/TaskProvider")]
    public class TaskProviderConfig : ScriptableObject
    {
        [field: SerializeField] public TasksPool TasksPool {get; private set;}
        [field: SerializeField] public int MaxTasksCount {get; private set;}
        [field: SerializeField] public bool IsRandom {get; private set;}
    }
}