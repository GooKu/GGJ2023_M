using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new layerData", menuName = "Scriptable Objects/ Layer Data")]
public class ScriptableLayerData : ScriptableObject
{
    public List<TileData> tileData;
}

[System.Serializable]
public class TileData
{
    [SerializeField] Vector2Int _tilePos;
    [SerializeField] int _tileID;
}
