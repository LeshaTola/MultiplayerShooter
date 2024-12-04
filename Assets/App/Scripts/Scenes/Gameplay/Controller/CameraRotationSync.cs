using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
    public class CameraRotationSync : MonoBehaviourPun, IPunObservable
    {
        //private Quaternion syncedRotation;
        private float syncedRotation;

        void Update()
        {
            if (photonView.IsMine)
            {
                return;
            }

            var rotation = Quaternion.Euler(syncedRotation, transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z);
            transform.localRotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.localRotation.x);
            }
            else
            {
                syncedRotation =  (float) stream.ReceiveNext();
            }
        }
    }
}