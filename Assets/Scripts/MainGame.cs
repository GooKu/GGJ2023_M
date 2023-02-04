using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class MainGame : MonoBehaviour
    {
        [SerializeField]
        private GameMapView gameMapView;
        [SerializeField]
        private List<ScriptableLayerData> layerDatas = new();

        private Player player = new();
        private GameMap gameMap;

        private void Awake()
        {
            gameMap = BuildGameMap(layerDatas);
            gameMapView.SetUp(gameMap, layerDatas);
            SetRoot(new Hex(), null);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InputHandle();
            }
        }

        private static GameMap BuildGameMap(List<ScriptableLayerData> layerDatas)
        {
            var gameMap = new GameMap();

            int maxWidth = 0;
            int totalHeight = 0;
            for (var i = 0; i < layerDatas.Count; i++)
            {
                if (maxWidth < layerDatas[i].size.x)
                {
                    maxWidth = layerDatas[i].size.x;
                }

                totalHeight += layerDatas[i].size.y;
            }

            int offsetY = 0;
            for (var i = 0; i < layerDatas.Count; i++)
            {
                BuildLayer(gameMap, layerDatas[i].size.x, layerDatas[i].size.y, offsetY);
                offsetY += layerDatas[i].size.y;
            }

            return gameMap;
        }

        private static void BuildLayer(GameMap gameMap, int width, int height, int offsetY)
        {
            int extend = (width - 1) / 2;
            for (var i = -extend; i <= extend; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var hex = new Hex(i, j + offsetY);
                    gameMap.AddTile(new TileData(hex));
                }
            }
        }

        private void InputHandle()
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Hex pos = Hex.PointToHex(worldPosition, gameMapView.GetHexSize());
            var tileData = gameMap.GetTile(pos);

            if (tileData == null
                || tileData.Rootable == TileData.RootableType.Invalid)
            {
                return;
            }

            SetRoot(pos, tileData.Parent);

        }

        private void SetRoot(Hex pos, Root parent)
        {
            var tileData = gameMap.GetTile(pos);
            tileData.UpdateType(TileData.TileType.Root);

            gameMapView.UpdateTile(pos, TileData.TileType.Root);

            Root.Level level = Root.Level.Main;

            if(parent != null)
            {
                level = parent.BranchLevel;
                tileData.SetParent(parent);
            }

            var root = new Root(parent, pos, level);


            #region update arounding tiles
            #endregion
            int checkIndex = 0;

            if (parent != null)
            {
                parent.UpdateLevel(Root.Level.Sub);

                checkIndex = (pos.row & 1) == 0 ? 0 : 1;

                for (int i = 0; i < 6; i++)
                {
                    var offset = Hex.Directions[checkIndex, i];

                    var checkPos = new Hex(tileData.Position.column + offset.x, tileData.Position.row + offset.y);
                    var neighborTile = gameMap.GetTile(checkPos);

                    if(neighborTile == null
                        || neighborTile.Rootable == TileData.RootableType.Main)
                    {
                        continue;
                    }

                    neighborTile.UpdateRootableType(TileData.RootableType.Sub);
                }
            }

            checkIndex = (pos.row & 1) == 0 ? 0 : 1;
            var checkVects = new Vector2Int[] { Hex.Directions[checkIndex, 0],
                Hex.Directions[checkIndex, 4],
                Hex.Directions[checkIndex, 5]
            };

            for(int i = 0; i < checkVects.Length; i++)
            {
                var offset = checkVects[i];
                var checkPos = new Hex(tileData.Position.column + offset.x, tileData.Position.row + offset.y);
                var checkTile = gameMap.GetTile(checkPos);

                if (checkTile == null || checkTile.Type == TileData.TileType.Obstacle)
                {
                    continue;
                }

                checkTile.UpdateRootableType(TileData.RootableType.Main);
                checkTile.SetParent(root);
            }

            checkVects = new Vector2Int[] { Hex.Directions[checkIndex, 1],
                Hex.Directions[checkIndex, 2],
                Hex.Directions[checkIndex, 3]
            };

            for (int i = 0; i < checkVects.Length; i++)
            {
                var offset = checkVects[i];
                var checkPos = new Hex(tileData.Position.column + offset.x, tileData.Position.row + offset.y);
                var checkTile = gameMap.GetTile(checkPos);

                if (checkTile == null || checkTile.Type == TileData.TileType.Obstacle)
                {
                    continue;
                }

                checkTile.UpdateRootableType(TileData.RootableType.Sub);
                if(checkTile.Parent == null)
                {
                    checkTile.SetParent(root);
                }
            }

            player.AddRoot(root);
        }
    }
}
