namespace App.Scripts.Scenes.Gameplay.Features.Commands.General
{
    public interface ILabeledCommand : ICommand
    {
        string Label { get; }
    }
}