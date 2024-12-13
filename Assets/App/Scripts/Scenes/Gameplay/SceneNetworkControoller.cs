using System;
using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.Gameplay.Esc;
using App.Scripts.Scenes.Gameplay.Esc.Menu;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
using App.Scripts.Scenes.Gameplay.KillChat;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Timer;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;
using Player = App.Scripts.Scenes.Gameplay.Player.Player;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.Gameplay
{
    public class RoomController : MonoBehaviourPunCallbacks
    {
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
