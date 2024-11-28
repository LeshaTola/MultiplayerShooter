namespace App.Scripts.Scenes.Gameplay.Features.Commands.General
{
    public abstract class LabeledCommand : ILabeledCommand
    {
        public string Label { get; }

        public LabeledCommand(string label)
        {
            Label = label;
        }

        public abstract void Execute();
    }
}