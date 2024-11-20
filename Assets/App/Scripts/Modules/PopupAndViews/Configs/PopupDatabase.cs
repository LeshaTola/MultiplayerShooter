using System.Collections.Generic;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Configs
{
    [CreateAssetMenu(fileName = "PopupDatabase", menuName = "Dictionaries/Popup")]
    public class PopupDatabase : SerializedScriptableObject
    {
        [SerializeField] private List<Popup> popups;

        public List<Popup> Popups => popups;
    }
}