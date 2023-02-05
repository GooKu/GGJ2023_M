using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    [CreateAssetMenu(fileName = "LayerData", menuName = "Scriptable Objects/Layer Data")]
    public class ScriptableLayerData : ScriptableObject
    {
        public Vector2Int size;
        public List<TileInLayer> tiles = new();
        public List<Vector2Int> ants = new();
    }

    [Serializable]
    public class TileInLayer
    {
        public Vector2Int position;
        public int tileId;
    }
}

