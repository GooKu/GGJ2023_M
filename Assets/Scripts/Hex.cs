using System;
using UnityEngine;

namespace GGJ23M
{
    // Use odd-q coordinate.
    // See: https://www.redblobgames.com/grids/hexagons/
    public struct Hex : IEquatable<Hex>
    {
        public static Vector2Int[,] Directions = new Vector2Int[2, 6] {
            {
                new Vector2Int(1, 0),
                new Vector2Int(1, -1),
                new Vector2Int(0, -1),
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
            },
            {
                new Vector2Int(1, 1),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 1),
            }
        };

        public int column;
        public int row;

        public Hex(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public bool Equals(Hex other)
        {
            return column == other.column &&
                   row == other.row;
        }

        public override bool Equals(object obj)
        {
            return obj is Hex hex &&
                   column == hex.column &&
                   row == hex.row;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(column, row);
        }

        public override string ToString()
        {
            return $"({column}, {row})";
        }

        public static bool operator ==(Hex a, Hex b)
        {
            return a.column == b.column && a.row == b.row;
        }

        public static bool operator !=(Hex a, Hex b)
        {
            return a.column != b.column || a.row != b.row;
        }

        public Hex Neighbor(int direction)
        {
            int parity = column & 1;
            Vector2Int diff = Directions[parity, direction];
            return new Hex(column + diff.x, row + diff.y);
        }

        public Vector2 ToPoint(float hexSize)
        {
            float x = hexSize * column * 3f / 2f;
            float y = -hexSize * Mathf.Sqrt(3f) * (row + 0.5f * (column & 1));
            return new Vector2(x, y);
        }

        public bool IsNeighbor(Hex hex)
        {
            int checkIndex = (hex.column & 1) == 0 ? 0 : 1;

            for (int i = 0; i < 6; i++)
            {
                var offset = Directions[checkIndex, i];

                if (hex.row + offset.y == row
                    && hex.column + offset.x == column)
                {
                    return true;
                }
            }

            return false;
        }

        public static Hex PointToHex(Vector2 point, float hexSize)
        {
            float q = (2f / 3f * point.x) / hexSize;
            float r = (-1f / 3f * point.x - Mathf.Sqrt(3f) / 3f * point.y) / hexSize;
            Vector2Int axialRounded = AxialRound(q, r);
            Hex hex = AxialToOddQ(axialRounded);
            return hex;
        }

        private static Vector2Int AxialRound(float q, float r)
        {
            var axial = new Vector2(q, r);
            Vector3 cube = AxialToCube(axial);
            Vector3Int cubeRounded = CubeRound(cube);
            Vector2Int axialRounded = CubeToAxial(cubeRounded);
            return axialRounded;
        }

        private static Vector3Int CubeRound(Vector3 cube)
        {
            int q = Mathf.RoundToInt(cube.x);
            int r = Mathf.RoundToInt(cube.y);
            int s = Mathf.RoundToInt(cube.z);

            float q_diff = Mathf.Abs(q - cube.x);
            float r_diff = Mathf.Abs(r - cube.y);
            float s_diff = Mathf.Abs(s - cube.z);

            if (q_diff > r_diff && q_diff > s_diff)
                q = -r - s;
            else if (r_diff > s_diff)
                r = -q - s;
            else
                s = -q - r;

            return new Vector3Int(q, r, s);
        }

        private static Hex AxialToOddQ(Vector2Int axial)
        {
            int column = axial.x;
            int row = axial.y + (axial.x - (axial.x & 1)) / 2;
            return new Hex(column, row);
        }

        private static Vector2Int OddQToAxial(Hex offset)
        {
            int q = offset.column;
            int r = offset.row - (offset.column - (offset.column & 1)) / 2; ;
            return new Vector2Int(q, r);
        }

        private static Vector3Int AxialToCube(Vector2Int axial)
        {
            int q = axial.x;
            int r = axial.y;
            int s = -q - r;
            return new Vector3Int(q, r, s);
        }

        private static Vector3 AxialToCube(Vector2 axial)
        {
            float q = axial.x;
            float r = axial.y;
            float s = -q - r;
            return new Vector3(q, r, s);
        }

        private static Vector2Int CubeToAxial(Vector3Int cube)
        {
            int q = cube.x;
            int r = cube.y;
            return new Vector2Int(q, r);
        }
    }
}
