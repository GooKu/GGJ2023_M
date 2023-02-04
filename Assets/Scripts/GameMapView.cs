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
            if (Input.GetKeyDown(KeyCode.Z))
            {
                MoveUp();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                MoveDown();
            }
        }

        public void UpdateTile(Hex pos, TileData.TileType tileType, bool isMain = false)
        {
            if (!tileViews.TryGetValue(pos, out var tile)) { return; }

            switch (tileType)
            {
                default:
                    tile.UpdateSprite(-1);
                    tile.SetType(tileType);
                    break;
                case TileData.TileType.Root:
                    tile.SetType(tileType);
                    int index = isMain ? 0 : 1;
                    tile.UpdateSprite(index);
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
            return hexWidth * hexSize * 1.5f + hexSize * 0.5f;
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
