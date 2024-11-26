using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
    public class CameraRotationSync : MonoBehaviourPun, IPunObservable
    {
        private Quaternion syncedRotation;

        void Update()
        {
            if (photonView.IsMine)
            {
                return;
            }

            transform.localRotation = Quaternion.Lerp(transform.rotation, syncedRotation, Time.deltaTime * 10f);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.localRotation);
            }
            else
            {
                syncedRotation = (Quaternion) stream.ReceiveNext();
            }
        }
    }
}