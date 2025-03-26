using System;
using System.Linq;
using System.Text;
using App.Scripts.Modules.Saves;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Saves;
using YG;

namespace App.Scripts.Features.Yandex.Saves
{
    public class YandexPromocodesDataProvider : IDataProvider<PromocodesSavesData>
    {
        private const string Key = "MySecretKey";

        public void SaveData(PromocodesSavesData data)
        {
            var encryptedData = EncryptData(data);

            YG2.saves.PromocodesesData = encryptedData;
            YG2.SaveProgress();
        }

        public PromocodesSavesData GetData()
        {
            var decryptedData = DecryptData();

            return decryptedData;
        }

        public void DeleteData()
        {
            YG2.saves.PromocodesesData = null;
        }

        public bool HasData()
        {
            return YG2.saves.PromocodesesData != null;
        }

        private static PromocodesSavesData EncryptData(PromocodesSavesData data)
        {
            var encryptedData = new PromocodesSavesData();

            foreach (var promocode in data.UsedPromocodes)   
            {
                encryptedData.UsedPromocodes.Add(new PromocodeData()
                {
                    PromoCode = Encrypt(promocode.PromoCode),
                    Uses = promocode.Uses,
                });
            }

            return encryptedData;
        }

        private static PromocodesSavesData DecryptData()
        {
            var decryptedData = new PromocodesSavesData();

            foreach (var promocode in YG2.saves.PromocodesesData.UsedPromocodes)
            {
                var promoCode = Decrypt(promocode.PromoCode);
                if (string.IsNullOrEmpty(promoCode))
                {
                    continue;
                }
                
                decryptedData.UsedPromocodes.Add(new PromocodeData()
                {
                    PromoCode = promoCode,
                    Uses = promocode.Uses,
                });
            }

            return decryptedData;
        }

        public static string Encrypt(string promoCode)
        {
            return Convert.ToBase64String(XorEncoding(Encoding.UTF8.GetBytes(promoCode), Key));
        }

        public static string Decrypt(string encryptedPromoCode)
        {
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedPromoCode);
                byte[] decryptedBytes = XorEncoding(encryptedBytes, Key);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Ошибка расшифровки промокода: {encryptedPromoCode}. Пропускаем.");
                return string.Empty; // Или можешь вернуть `null`, если так удобнее
            }
        }
        
        private static byte[] XorEncoding(byte[] data, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] result = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return result;
        }
    }
}