using App.Scripts.Features.Settings;
using App.Scripts.Modules.Saves;
using YG;

namespace App.Scripts.Features.Yandex.Saves
{
    public class YandexSettingsDataProvider: IDataProvider<SettingsData>
    {
        public void SaveData(SettingsData data)
        {
            YG2.saves.SettingsData = data;
            YG2.SaveProgress();
        }

        public SettingsData GetData()
        {
            return YG2.saves.SettingsData;
        }

        public void DeleteData()
        {
            YG2.saves.SettingsData = null;
        }

        public bool HasData()
        {
            return YG2.saves.SettingsData != null;
        }
    }
}

