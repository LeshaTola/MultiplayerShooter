﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.PlayerStats.Rank.Configs
{
    [Serializable]
    public class RankData
    {
        [FoldoutGroup("@Name")]
        public string Name;
        [FoldoutGroup("@Name")]
        [PreviewField(60)]
        public Sprite Sprite;
        [FoldoutGroup("@Name")]
        public int ExpForRank;
    }
}