namespace App.Scripts.Modules.Saves
{
    public interface IStorage
    {
        void SetString(string key, string value);
        string GetString(string key);
        void DeleteString(string key);
    }
}