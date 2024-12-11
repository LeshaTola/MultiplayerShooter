namespace App.Scripts.Modules.Commands.General
{
    public interface ILabeledCommand : ICommand
    {
        string Label { get; }
    }
}