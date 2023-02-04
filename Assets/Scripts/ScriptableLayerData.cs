using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    [CreateAssetMenu(fileName = "new layerData", menuName = "Scriptable Objects/ Layer Data")]
    public class ScriptableLayerData : ScriptableObject
    {
        public List<TilesInLayer> tileData;
    }

    [System.Serializable]
    public class TilesInLayer
    {
        [SerializeField] Vector2Int _tilePos;
        [SerializeField] int _tileID;
    }
}

