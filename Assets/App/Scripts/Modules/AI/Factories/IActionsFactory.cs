using System;
using System.Collections.Generic;
using App.Scripts.Modules.AI.Actions;
using App.Scripts.Modules.AI.Considerations;

namespace App.Scripts.Modules.AI.Factories
{
	public interface IActionsFactory
	{
		IAction GetAction(Type type);
		List<IAction> GetAllActions();
		IConsideration GetConsideration(IConsideration consideration);
	}
}