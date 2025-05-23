﻿using UnityEngine;

namespace App.Scripts.Modules.Saves
{
    public class PlayerPrefsStorage : IStorage
    {
        public string GetString(string key)
        {
            return PlayerPrefs.GetString(key, null);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void DeleteString(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}