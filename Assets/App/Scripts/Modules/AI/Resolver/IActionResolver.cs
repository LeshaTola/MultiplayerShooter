using System.Collections.Generic;
using App.Scripts.Modules.AI.Actions;

namespace App.Scripts.Modules.AI.Resolver
{
	public interface IActionResolver
	{
		IAction GetBestAction();
		void Init(List<IAction> actions);
	}
}
