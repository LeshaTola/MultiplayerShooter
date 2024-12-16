using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

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