using System;
using System.Collections.Generic;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Modules.PopupAndViews.Configs;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.General.Providers
{
    public class PopupProvider : IPopupProvider
    {
        public Dictionary<Type, IPool<Popup.Popup>> PopupPoolsDictionary { get; private set; }

        private PopupDatabase popupDatabase;
        private Transform container;

        public PopupProvider(PopupDatabase popupDatabase, Transform container)
        {
            this.popupDatabase = popupDatabase;
            this.container = container;
            Setup(popupDatabase);
        }

        private void Setup(PopupDatabase popupDatabase)
        {
            PopupPoolsDictionary = new Dictionary<Type, IPool<Popup.Popup>>();
            foreach (var popup in popupDatabase.Popups)
            {
                var pool = new ObjectPool<Popup.Popup>(() => GameObject.Instantiate(popup, container), null, null, 0);
                PopupPoolsDictionary.Add(popup.GetType(), pool);
            }
        }
    }
}