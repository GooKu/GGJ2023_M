using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public partial class GameMapView : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private GameObject tilePrefab;
        [SerializeField]
        private float hexSize = 1f;

        private GameMap gameMap;
        private List<ScriptableLayerData> layerDatas;

        private Dictionary<Hex, TileView> tileViews = new();

        public void SetUp(GameMap gameMap, List<ScriptableLayerData> layerDatas)
        {
            this.gameMap = gameMap;
            this.layerDatas = layerDatas;
            foreach (TileData tileData in gameMap.AllTileData)
            {
                var hex = tileData.Position;
                Vector2 point = hex.ToPoint(hexSize);
                GameObject clone = Instantiate(tilePrefab, point, Quaternion.identity, transform);
                clone.name = $"hex_{hex.column}_{hex.row}";
                TileView tileView = clone.GetComponent<TileView>();
                tileView.SetType(tileData.Type);
                tileViews.Add(hex, tileView);
            }
        }

        private void Update()
        {
            UpdateCamera();
        }

        public void UpdateTile(Hex pos, TileData.TileType tileType, Vector2Int tileResult, bool isMain = false)
        {
            if (!tileViews.TryGetValue(pos, out var tile)) { return; }

            switch (tileType)
            {
                default:
                    tile.UpdateSprite(-1, 4, true);
                    tile.SetType(tileType);
                    break;
                case TileData.TileType.Root:
                    tile.SetType(tileType);
                    if (pos == new Hex())
                    {
                        tile.UpdateSprite(0, 0, true);
                        break;
                    }

                    int index = isMain ? 1 : 2;
                    if(tileResult.x == 0)
                    {
                        tile.UpdateSprite(index, 4, isMain);
                    }
                    else
                    {
                        tile.UpdateSprite(tileResult.x, tileResult.y, isMain);
                    }
                    break;
            }
        }

        private float GetLayerWidth(int indexOfLayer)
        {
            if (layerDatas == null)
            {
                return 0f;
            }

            int hexWidth = layerDatas[indexOfLayer].size.x;
            float width = hexWidth * hexSize * 1.5f + hexSize * 0.5f;
            width += 0.5f;  // Bleed
            return width;
        }

        private float GetLayerHeight(int indexOfLayer)
        {
            if (layerDatas == null)
            {
                return 0f;
            }

            int hexHeight = layerDatas[indexOfLayer].size.y;
            float height = (hexHeight + 1) * hexSize * Mathf.Sqrt(3);
            height += 2.5f;  // Bleed and UI on top
            return height;
        }

        private Vector2 GetLayerCenter(int indexOfLayer)
        {
            if (layerDatas == null)
            {
                return new Vector2();
            }

            int offsetY = 0;
            for (var i = 0; i < layerDatas.Count; i++)
            {
                if (i != indexOfLayer)
                {
                    offsetY += layerDatas[i].size.y;
                    continue;
                }

                offsetY += layerDatas[i].size.y / 2;
                var hex = new Hex(0, offsetY);
                return hex.ToPoint(hexSize);
            }

            {
                var hex = new Hex(0, offsetY);
                return hex.ToPoint(hexSize);
            }
        }

        public float GetHexSize() { return hexSize; }
    }
}
