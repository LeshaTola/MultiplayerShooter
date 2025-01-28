using System.Collections.Generic;
using App.Scripts.Features.Match.Maps;
using UnityEngine;

namespace App.Scripts.Features.Match.Configs
{

    [CreateAssetMenu(fileName = "MapsConfig", menuName = "Configs/Maps/Maps")]
    public class MapsConfig : ScriptableObject
    {
        [field:SerializeField]public List<MapConfig> Maps { get; private set; }
    }
}