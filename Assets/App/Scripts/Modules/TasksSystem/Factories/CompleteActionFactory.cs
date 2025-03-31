using System.Collections.Generic;
using App.Scripts.Modules.TasksSystem.CompleteActions;
using Zenject;

namespace App.Scripts.Modules.TasksSystem.Factories
{
    public class CompleteActionFactory
    {
        private readonly DiContainer diContainer;

        public CompleteActionFactory(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public CompleteAction CreateCompleteAction(CompleteAction original)
        {
            var newAction = (CompleteAction) diContainer.Instantiate(original.GetType());
            newAction.Import(original);
            return newAction;
        }
        
        public List<CompleteAction> CreateCompleteActions(List<CompleteAction> originals)
        {
            List<CompleteAction> newActions = new();
            foreach (var original in originals)
            {
                newActions.Add(CreateCompleteAction(original));
            }
            return newActions;
        }
    }
}