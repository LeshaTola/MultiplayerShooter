using System.Collections.Generic;
using App.Scripts.Modules.AI.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.AI.Configs
{
	[CreateAssetMenu(fileName = "ActionsDatabase", menuName = "Configs/AI/Actions")]
	public class ActionsDatabase : SerializedScriptableObject
	{
		[SerializeField] private List<IAction> _actions = new();

		public List<IAction> Actions => _actions;
	}
}
