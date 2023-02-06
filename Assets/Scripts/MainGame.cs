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
        [SerializeField]
        private Ant antSample;

        private Player player;
        private GameMap gameMap;
        private List<Ant> ants = new List<Ant>();

        private Root mainRoot;
        private Hex[] mainEmpty = new Hex[3];

        private int scroe = 0;

        private void Awake()
        {
            gameMap = BuildGameMap(layerDatas);
            gameMapView.SetUp(gameMap, layerDatas);
            foreach(var l in layerDatas)
            {
                foreach(var a in l.ants)
                {
                    var hex = new Hex(a.x, a.y);
                    Vector2 point = hex.ToPoint(gameMapView.GetHexSize());
                    var ant = Instantiate(antSample, point, Quaternion.identity, transform);
                    ant.Init(hex, gameMapView.GetHexSize());
                    ants.Add(ant);
                }
            }
            startEnergy++;//++for frist root
            player = new(startEnergy);
            player.EnergyChnageEvent += gameUI.SetEnergyAmount;
            player.EnergyChnageEvent += GameEndCheck;
            gameUI.SetEnergyAmount(startEnergy);
            gameUI.SetScore(scroe);
            SetRoot(new Hex(), null, Root.Level.Main);
        }

        private void Start()
        {
            AudioPlayer.Instance.PlayMusic(0);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !player.IfPlayerDead)
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
            foreach (var ant in ants)
            {
                ant.Move(gameMap);
            }

            var tileData = gameMap.GetTile(pos);

            bool soundEffectTriggerd = false;

            if (tileData.Type == TileData.TileType.Water)
            {
                player.AddEnergy(8);
                AudioPlayer.Instance.PlayEffect(4);
                EffectManager.Instance.Play(0, tileData.Position.ToPoint(gameMapView.GetHexSize()));
                soundEffectTriggerd = true;
            }
            else if (tileData.Type == TileData.TileType.Light)
            {
                gameMap.UnlockNextLayer();
                gameMapView.MoveDown();
                AudioPlayer.Instance.PlayEffect(6);
                EffectManager.Instance.Play(1, tileData.Position.ToPoint(gameMapView.GetHexSize()));
                soundEffectTriggerd = true;

                if (gameMap.MaxUnlockedLayer >= layerDatas.Count)
                {
                    GameEnd(true);
                }
            }

            tileData.UpdateType(TileData.TileType.Root);

            if (parent != null)
            {
                //Debug.Log($"{pos}, root:{parent.ReturnHex()}, {parent.BranchLevel}");
                scroe++;
                gameUI.SetScore(scroe);
            }
            
            Vector2Int tileResult = player.CheckNeighbor(pos);
            gameMapView.UpdateTile(pos, TileData.TileType.Root, tileResult, level == Root.Level.Main);

            var root = new Root(pos, level);

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

                if (!soundEffectTriggerd)
                {
                    AudioPlayer.Instance.PlayEffect(5);
                }
            }
            else
            {
                player.RemoveEnergy(2);

                if (!soundEffectTriggerd)
                {
                    if (parent.WasMainRoot)
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
                var ants = new List<Ant>();

                foreach(var ant in this.ants)
                {
                    ants.Add(ant);
                }

                foreach (var ant in ants)
                {
                    if(pos != ant.Pos) { continue; }
                    player.RemoveEnergy(5);
                    this.ants.Remove(ant);
                    Destroy(ant.gameObject);
                    gameUI.ShowHit();
                }
            }

            player.AddRoot(root);

            TileResult neighborsTileResults = player.UpdateNeighbor(pos);
            for (int i = 0; i < neighborsTileResults.Results.Count; i++)
            {
                gameMapView.UpdateTile(neighborsTileResults.Hexes[i], TileData.TileType.Root, neighborsTileResults.Results[i], neighborsTileResults.root.WasMainRoot);
            }
        }

        private void GameEndCheck(int energy)
        {
            if (energy > 0) { return; }

            GameEnd(gameMap.MaxUnlockedLayer >= layerDatas.Count);
        }

        private void GameEnd(bool isPass)
        {
            gameMapView.enabled = false;
            gameUI.ShowEnd(scroe, isPass);

            if (isPass)
            {
                AudioPlayer.Instance.PlayMusic(2);
            }
            else
            {
                AudioPlayer.Instance.PlayMusic(1);
            }
        }

    }
}
