using System.Collections.Generic;
using App.Scripts.Modules.AI.Actions;

namespace App.Scripts.Modules.AI.Resolver
{
	public class ActionResolver : IActionResolver
	{
		private List<IAction> _actions = new();

		public void Init(List<IAction> actions)
		{
			_actions = actions;
		}

		public IAction GetBestAction()
		{
			if (_actions == null || _actions.Count <= 0)
			{
				return null;
			}

			IAction bestAction = null;
			float bestScore = float.MinValue;

			foreach (IAction action in _actions)
			{
				float score = action.GetScore();
				if (score > bestScore)
				{
					bestScore = score;
					bestAction = action;
				}
			}

			return bestAction;
		}
	}
}
