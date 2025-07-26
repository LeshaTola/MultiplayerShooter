using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Photon.Pun;

namespace App.Scripts.Scenes.Gameplay.Effectors
{
    public interface IEntity
    {
        Health Health { get; }
        PhotonView PhotonView { get; }
        IEntityMovement Movement { get; }
        PlayerAudioProvider AudioProvider { get; }
        PlayerVisual Visual { get; }
    }
}