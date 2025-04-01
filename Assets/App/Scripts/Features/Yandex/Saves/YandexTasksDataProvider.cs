using App.Scripts.Modules.Saves;
using App.Scripts.Modules.TasksSystem.Providers;
using YG;

namespace App.Scripts.Features.Yandex.Saves
{
    public class YandexTasksDataProvider: IDataProvider<TasksData>
    {
        public void SaveData(TasksData data)
        {
            YG2.saves.TasksData = data;
            YG2.SaveProgress();
        }

        public TasksData GetData()
        {
            return YG2.saves.TasksData;
        }

        public void DeleteData()
        {
            YG2.saves.TasksData = null;
        }

        public bool HasData()
        {
            return YG2.saves.TasksData != null;
        }
    }
}