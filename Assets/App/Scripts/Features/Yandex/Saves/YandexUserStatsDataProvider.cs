using App.Scripts.Modules.Saves;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using YG;

namespace App.Scripts.Features.Yandex.Saves
{
    public class YandexUserStatsDataProvider: IDataProvider<UserStatsData>
    {
        public void SaveData(UserStatsData data)
        {
            YG2.saves.UserStatsData = data;
            YG2.SaveProgress();
        }

        public UserStatsData GetData()
        {
            return YG2.saves.UserStatsData;
        }

        public void DeleteData()
        {
            YG2.saves.UserStatsData = null;
        }

        public bool HasData()
        {
            return YG2.saves.UserStatsData != null;
        }
    }
}