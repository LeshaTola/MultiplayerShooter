using App.Scripts.Modules.Saves;
using App.Scripts.Scenes.MainMenu.Features.Promocodes.Saves;
using YG;

namespace App.Scripts.Features.Yandex.Saves
{
    public class YandexPromocodesDataProvider: IDataProvider<PromocodesSavesData>
    {
        public void SaveData(PromocodesSavesData data)
        {
            YG2.saves.PromocodesesData = data;
            YG2.SaveProgress();
        }

        public PromocodesSavesData GetData()
        {
            return YG2.saves.PromocodesesData;
        }

        public void DeleteData()
        {
            YG2.saves.PromocodesesData = null;
        }

        public bool HasData()
        {
            return YG2.saves.PromocodesesData != null;
        }
    }
}