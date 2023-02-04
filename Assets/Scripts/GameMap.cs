using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class GameMap
    {
        private readonly List<ScriptableLayerData> layerDatas;
        private readonly Dictionary<Hex, TileData> mapData = new();

        public IEnumerable<TileData> AllTileData => mapData.Values;

        public GameMap(List<ScriptableLayerData> layerDatas)
        {
            this.layerDatas = layerDatas;
        }

        public void Initialize()
        {
            int offsetY = 0;
            for (var i = 0; i < layerDatas.Count; i++)
            {
                BuildLayer(this, layerDatas[i], offsetY);
                offsetY += layerDatas[i].size.y;
            }
        }


        private static void BuildLayer(GameMap gameMap, ScriptableLayerData layerData, int offsetY)
        {
            int width = layerData.size.x;
            int height = layerData.size.y;

            // Build tiles
            int extend = (width - 1) / 2;
            for (var i = -extend; i <= extend; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var hex = new Hex(i, j + offsetY);
                    gameMap.AddTile(new TileData(hex));
                }
            }

            // Setup tile types
            for (var i = 0; i < layerData.tiles.Count; i++)
            {
                var hex = new Hex(layerData.tiles[i].position.x, layerData.tiles[i].position.y);
                TileData tileData = gameMap.GetTile(hex);
                if (tileData == null)
                {
                    Debug.LogWarning($"Tile {hex} not exist");
                    continue;
                }
                tileData.UpdateType((TileData.TileType)layerData.tiles[i].tileId);
            }
        }


        public void AddTile(TileData tileData)
        {
            mapData.Add(tileData.Position, tileData);
        }

        public TileData GetTile(Hex hex)
        {
            bool success = mapData.TryGetValue(hex, out TileData tileData);
            if (success)
            {
                return tileData;
            }

            return null;
        }
    }
}
