namespace App.Scripts.Modules.Saves
{
    public interface ISavable
    {
        public void SaveState();
        public void LoadState();
    }
}