using System.Collections.Generic;
using App.Scripts.Modules.AI.Considerations;

namespace App.Scripts.Modules.AI.Actions
{
	public interface IAction
	{
		List<IConsideration> Considerations { get; }

		void Execute();
		float GetScore();
		void Init(List<IConsideration> considerations);
	}


}
