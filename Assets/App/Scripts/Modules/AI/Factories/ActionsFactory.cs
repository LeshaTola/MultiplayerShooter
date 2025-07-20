using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.AI.Actions;
using App.Scripts.Modules.AI.Configs;
using App.Scripts.Modules.AI.Considerations;
using Zenject;

namespace App.Scripts.Modules.AI.Factories
{
    public class ActionsFactory : IActionsFactory
    {
        private readonly DiContainer _diContainer;
        private readonly ActionsDatabase _actionsDatabase;

        private readonly Dictionary<Type, IAction> _cashedValues = new();

        public ActionsFactory(DiContainer diContainer,
            ActionsDatabase actionsDatabase)
        {
            _diContainer = diContainer;
            _actionsDatabase = actionsDatabase;
        }

        public List<IAction> GetAllActions()
        {
            List<IAction> actions = new List<IAction>();
            foreach (var originalAction in _actionsDatabase.Actions)
            {
                actions.Add(GetAction(originalAction.GetType()));
            }

            return actions;
        }

        public IAction GetAction(Type type)
        {
            if (_cashedValues.TryGetValue(type, out var cashedAction))
            {
                return cashedAction;
            }
            else
            {
                var original = GetOriginalAction(type);

                var action = (IAction) _diContainer.Instantiate(type);
                foreach (var originalConsideration in original.Considerations)
                {
                    action.Considerations.Add(GetConsideration(originalConsideration));
                }

                _cashedValues.Add(type, action);
                return action;
            }
        }

        public IConsideration GetConsideration(IConsideration consideration)
        {
            return (IConsideration) _diContainer.Instantiate(consideration.GetType(),
                new object[] {consideration.Config});
        }

        private IAction GetOriginalAction(Type type)
        {
            return _actionsDatabase.Actions.FirstOrDefault(x => x.GetType().Equals(type));
        }
    }
}