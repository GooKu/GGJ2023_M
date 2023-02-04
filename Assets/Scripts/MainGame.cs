using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class MainGame : MonoBehaviour
    {
        [SerializeField]
        private GameMapView gameMapView;
        [SerializeField]
        private GameUI gameUI;
        [SerializeField]
        private int startEnergy = 5;
        [SerializeField]
        private List<ScriptableLayerData> layerDatas = new();

        private Player player;
        private GameMap gameMap;

        private Root mainRoot;
        private Hex[] mainEmpty = new Hex[3];

        private int scroe = 0;

        private void Awake()
        {
            gameMap = BuildGameMap(layerDatas);
            gameMapView.SetUp(gameMap, layerDatas);
            startEnergy++;//++for frist root
            player = new(startEnergy);
            player.EnergyChnageEvent += gameUI.SetEnergyAmount;
            player.EnergyChnageEvent += GameEndCheck;
            gameUI.SetEnergyAmount(startEnergy);
            gameUI.SetScore(scroe);
            SetRoot(new Hex(), null, Root.Level.Main);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !player.IfPlayerDead())
            {
                InputHandle();
            }
        }

        private static GameMap BuildGameMap(List<ScriptableLayerData> layerDatas)
        {
            var gameMap = new GameMap(layerDatas);
            gameMap.Initialize();
            return gameMap;
        }

        private void InputHandle()
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Hex pos = Hex.PointToHex(worldPosition, gameMapView.GetHexSize());
            var tileData = gameMap.GetTile(pos);

            if (tileData == null)
            {
                return;
            }

            switch (tileData.Type)
            {
                case TileData.TileType.Obstacle:
                case TileData.TileType.Root:
                    return;
            }

            foreach (var p in mainEmpty)
            {
                if (pos.Equals(p))
                {
                    SetRoot(pos, mainRoot, Root.Level.Main);
                    return;
                }
            }

            var roots = new List<Root>();

            foreach (var r in player.ReturnRoots())
            {
                roots.Add(r);
            }

            foreach (var r in roots)
            {
                //Debug.Log($"{r.ReturnHex()}, {r.ReturnHex().IsNeighbor(pos)}");
                if (r.ReturnHex().IsNeighbor(pos))
                {
                    SetRoot(pos, r, Root.Level.Sub);
                    return;
                }
            }
        }

        private void SetRoot(Hex pos, Root parent, Root.Level level)
        {
            var tileData = gameMap.GetTile(pos);

            bool soundEffectTriggerd = false;

            if (tileData.Type == TileData.TileType.Water)
            {
                player.AddEnergy(8);
                AudioPlayer.Instance.PlayEffect(4);
                soundEffectTriggerd = true;
            }
            else if (tileData.Type == TileData.TileType.Light)
            {
                gameMap.UnlockNextLayer();
                gameMapView.MoveDown();
                AudioPlayer.Instance.PlayEffect(6);
                soundEffectTriggerd = true;

                if(gameMap.MaxUnlockedLayer >= 3)
                {
                    gameUI.ShowEnd(scroe, true);
                }
            }

            tileData.UpdateType(TileData.TileType.Root);

            if (parent != null)
            {
                //Debug.Log($"{pos}, root:{parent.ReturnHex()}, {parent.BranchLevel}");
                scroe++;
                gameUI.SetScore(scroe);
            }

            gameMapView.UpdateTile(pos, TileData.TileType.Root, level == Root.Level.Main);

            var root = new Root(parent, pos, level);

            if (level == Root.Level.Main)
            {
                mainRoot = root;

                var checkIndex = (pos.column & 1) == 0 ? 0 : 1;

                var checkVects = new Vector2Int[] { Hex.Directions[checkIndex, 0],
                    Hex.Directions[checkIndex, 4],
                    Hex.Directions[checkIndex, 5]
                };

                for (int i = 0; i < checkVects.Length; i++)
                {
                    var offset = checkVects[i];
                    var checkPos = new Hex(tileData.Position.column + offset.x, tileData.Position.row + offset.y);
                    mainEmpty[i] = checkPos;
                }

                player.RemoveEnergy(1);

                if(!soundEffectTriggerd)
                {
                    AudioPlayer.Instance.PlayEffect(5);
                }
            }
            else
            {
                player.RemoveEnergy(2);

                if (!soundEffectTriggerd)
                {
                    if(parent.WasMainRoot)
                    {
                        AudioPlayer.Instance.PlayEffect(3);
                    }
                    else
                    {
                        AudioPlayer.Instance.PlayEffect(5);
                    }
                }
            }

            if (parent != null)
            {
                parent.UpdateLevel(Root.Level.Sub);
            }

            player.AddRoot(root);
        }

        private void GameEndCheck(int energy)
        {
            if(energy > 0) { return; }

            gameUI.ShowEnd(scroe, gameMap.MaxUnlockedLayer >= 3);
        }
    }
}
