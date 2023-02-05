using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M {
    public class Ant : MonoBehaviour
    {
        public enum Direction
        {
            RD,
            RU,
            U,
            LU,
            LD,
            D
        }

        public Direction Direct { get; private set; }

        public Hex Pos { get; private set; }

        private float hexSize;

        public void Init(Hex pos, float hexSize)
        {
            Pos = pos;
            this.hexSize = hexSize;
            Direct = (Direction)Random.Range(0, 6);
        }

        public Hex GetNextPost(Direction direction)
        {
            int checkIndex = (Pos.column & 1) == 0 ? 0 : 1;
            var offset = Hex.Directions[checkIndex, (int)direction];
            return new Hex(Pos.column + offset.x, Pos.row + offset.y);
        }

        public void Move(GameMap map)
        {
            var nexPos = GetNextPost(Direct);
            var tileData = map.GetTile(nexPos);

            if (!CanMove(tileData))
            {
                var indexs = new List<int>();

                for (int i = 0; i < 6; i++)
                {
                    if (i == (int)Direct) { continue; }
                    indexs.Add(i);
                }

                for (int i = 0; i < indexs.Count; i++)
                {
                    var value = indexs[i];
                    var swapIndex = Random.Range(0, indexs.Count);
                    indexs[i] = indexs[swapIndex];
                    indexs[swapIndex] = value;
                }

                bool cantMove = true;

                for (int i = 0; i < indexs.Count; i++)
                {
                    var dir = (Direction)indexs[i];
                    var tryPos = GetNextPost(dir);
                    tileData = map.GetTile(tryPos);
                    if (!CanMove(tileData)) { continue; }
                    Direct = dir;
                    nexPos = tryPos;
                    cantMove = false;
                    break;
                }

                if (cantMove)
                {
                    return;
                }
            }

            Pos = nexPos;
            transform.position = Pos.ToPoint(hexSize);
        }

        public bool CanMove(TileData tileData)
        {
            if(tileData == null) { return false; }

            switch (tileData.Type)
            {
                case TileData.TileType.Root:
                case TileData.TileType.Obstacle:
                    return false;
                default:
                    return true;
            }
        }
    }
}