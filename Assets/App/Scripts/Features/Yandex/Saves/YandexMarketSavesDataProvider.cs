using App.Scripts.Modules.Saves;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using YG;

namespace App.Scripts.Features.Yandex.Saves
{
    public class YandexMarketSavesDataProvider: IDataProvider<MarketSavesData>
    {
        public void SaveData(MarketSavesData data)
        {
            YG2.saves.MarketData = data;
            YG2.SaveProgress();
        }

        public MarketSavesData GetData()
        {
            return YG2.saves.MarketData;
        }

        public void DeleteData()
        {
            YG2.saves.MarketData = null;
        }

        public bool HasData()
        {
            return YG2.saves.MarketData != null;
        }
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public MarketSavesData MarketData;
    }
}