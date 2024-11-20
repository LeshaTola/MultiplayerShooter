namespace App.Scripts.Modules.Factories
{
    public interface IFactory<T>
    {
        T GetItem();
    }
}