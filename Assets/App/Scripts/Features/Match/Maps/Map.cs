using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Match.Maps
{
    public class Map : MonoBehaviourPun
    {
        [field: SerializeField] public List<Transform> SpawnPoints { get; private set; }
    }
}