using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static GGJ23M.Root;

namespace GGJ23M
{
    public class Player
    {
        public System.Action<int> EnergyChnageEvent;

        int _currentEnergy;

        public bool IfPlayerDead => _currentEnergy <= 0;

        private readonly List<Root> _roots = new();

        public Player(int energy)
        {
            _currentEnergy = energy;
        }

        public void AddEnergy(int addValue)
        {
            _currentEnergy += addValue;
            EnergyChnageEvent?.Invoke(_currentEnergy);
        }

        public void RemoveEnergy(int removeValue)
        {
            _currentEnergy -= removeValue;

            EnergyChnageEvent?.Invoke(_currentEnergy);
        }

        public void AddRoot(Root root)
        {
            _roots.Add(root);
        }

        public List<Root> ReturnRoots()
        {
            return _roots;
        }

        public TileResult UpdateNeighbor(Hex targetHex)
        {
            TileResult tileResult = new();
            foreach (var root in _roots)
            {
                if (root.ReturnHex().IsNeighbor(targetHex))
                {
                    tileResult.Results.Add(CheckNeighbor(root.ReturnHex()));
                    tileResult.Hexes.Add(root.ReturnHex());
                    tileResult.root = root;
                }
            }
            return tileResult;
        }
        public Vector2Int CheckNeighbor(Hex targetHex)
        {
            Vector2Int tileResult;

            bool _0 = false;
            bool _1 = false;
            bool _2 = false;
            bool _3 = false;
            bool _4 = false;
            bool _5 = false;

            foreach (var root in _roots)
            {
                Hex hex = root.ReturnHex();

                int checkIndex = (targetHex.column & 1) == 0 ? 0 : 1;


                for (int i = 0; i < 6; i++)
                {
                    var offset = Hex.Directions[checkIndex, i];

                    if (targetHex.row + offset.y == hex.row
                        && targetHex.column + offset.x == hex.column)
                    {
                        switch (i)
                        {
                            case 0:
                                _4 = true;
                                break;
                            case 1:
                                _5 = true;
                                break;
                            case 2:
                                _0 = true;
                                break;
                            case 3:
                                _1 = true;
                                break;
                            case 4:
                                _2 = true;
                                break;
                            case 5:
                                _3 = true;
                                break;
                        }
                    }
                }
            }

            Debug.Log(targetHex.ToString() + ":" + _0 + "," + _1 + "," + _2 + "," + _3 + "," + _4 + "," + _5);
            tileResult = CalculateTile(_0, _1, _2, _3, _4, _5);

            return tileResult;
        }

        Vector2Int CalculateTile(bool _0, bool _1, bool _2, bool _3, bool _4, bool _5)
        {
            Vector2Int tileResult = new();

            bool[] bools = new bool[6];
            bools[0] = _0;
            bools[1] = _1;
            bools[2] = _2;
            bools[3] = _3;
            bools[4] = _4;
            bools[5] = _5;

            int connectedRoots = 0;
            foreach (var item in bools)
            {
                if (item) connectedRoots++;
            }

            switch (connectedRoots)
            {
                case 1:
                    tileResult = OneConnected(_0, _1, _2, _3, _4, _5);
                    break;
                case 2:
                    tileResult = TwoConnected(_0, _1, _2, _3, _4, _5);
                    break;
                case 3:
                    tileResult = ThreeConnected(_0, _1, _2, _3, _4, _5);
                    break;
                case 4:
                    tileResult = FourConnected(_0, _1, _2, _3, _4, _5);
                    break;
                case 5:
                    tileResult = FiveConnected(_0, _1, _2, _3, _4, _5);
                    break;
                case 6:
                    tileResult = SixConnected(_0, _1, _2, _3, _4, _5);
                    break;
            }

            return tileResult;
        }
        Vector2Int OneConnected(bool _0, bool _1, bool _2, bool _3, bool _4, bool _5)
        {
            int tileID = 14;
            int tileAngle = 0;


            bool[] bools = new bool[6];
            bools[0] = _0;
            bools[1] = _1;
            bools[2] = _2;
            bools[3] = _3;
            bools[4] = _4;
            bools[5] = _5;
            for (int i = 0; i < bools.Length; i++)
            {
                if (bools[i])
                {
                    tileAngle = i;
                }
            }
            return new Vector2Int(tileID, tileAngle);            
        }

        Vector2Int TwoConnected(bool _0, bool _1, bool _2, bool _3, bool _4, bool _5)
        {
            int tileID = 0;
            int tileAngle = 0;

            if (_0 && _1)
            {
                tileID = 17;
                tileAngle = 1;
            }
            else if (_0 && _2)
            {
                tileID = 4;
                tileAngle = 2;
            }
            else if (_0 && _3)
            {
                tileID = 9;
                tileAngle = 2;
            }
            else if (_0 && _4)
            {
                tileID = 4;
                tileAngle = 0;
            }
            else if (_0 && _5)
            {
                tileID = 17;
                tileAngle = 0;
            }

            else if(_1 && _2)
            {
                tileID = 17;
                tileAngle = 2;
            }
            else if(_1 && _3)
            {
                tileID = 4;
                tileAngle = 3;
            }
            else if (_1 && _4)
            {
                tileID = 9;
                tileAngle = 0;
            }
            else if(_1 && _5)
            {
                tileID = 4;
                tileAngle = 1;
            }

            else if(_2 && _3)
            {
                tileID = 17;
                tileAngle = 3;
            }
            else if(_2 && _4)
            {
                tileID = 4;
                tileAngle = 4;
            }
            else if(_2 && _5)
            {
                tileID = 9;
                tileAngle = 1;
            }

            else if(_3 && _4)
            {
                tileID = 17;
                tileAngle = 4;
            }
            else if(_3 && _5)
            {
                tileID = 4;
                tileAngle = 5;
            }

            else if(_4 && _5)
            {
                tileID = 17;
                tileAngle = 5;
            }
            Debug.Log(tileID);
            return new Vector2Int(tileID, tileAngle);
        }

        Vector2Int ThreeConnected(bool _0, bool _1, bool _2, bool _3, bool _4, bool _5)
        {
            int tileID = 0;
            int tileAngle = 0;

            if (_0 && _2 && _4)
            {
                tileID = 5;
                tileAngle = 0;
            }
            else if(_1 && _3 && _5)
            {
                tileID = 5;
                tileAngle = 1;
            }

            else if (_0 && _1 && _2)
            {
                tileID = 10;
                tileAngle = 2;
            }
            else if(_1 && _2 && _3)
            {
                tileID = 10;
                tileAngle = 3;
            }
            else if(_2 && _3 && _4)
            {
                tileID = 10;
                tileAngle = 4;
            }
            else if(_3 && _4 && _5)
            {
                tileID = 10;
                tileAngle = 5;
            }
            else if(_4 && _5 && _0)
            {
                tileID = 10;
                tileAngle = 0;
            }
            else if(_5 && _0 && _1)
            {
                tileID = 10;
                tileAngle = 1;
            }

            else if(_0 && _3 && _5)
            {
                tileID = 13;
                tileAngle = 0;
            }
            else if(_1 && _4 && _0)
            {
                tileID = 13;
                tileAngle = 1;
            }
            else if(_2 && _5 && _1)
            {
                tileID = 13;
                tileAngle = 2;
            }
            else if(_3 && _0 && _2)
            {
                tileID = 13;
                tileAngle = 3;
            }
            else if(_4 && _1 && _3)
            {
                tileID = 13;
                tileAngle = 1;
            }
            else if(_5 && _2 && _4)
            {
                tileID = 13;
                tileAngle = 5;
            }

            else if(_0 && _1 && _3)
            {
                tileID = 15;
                tileAngle = 0;
            }
            else if(_1 && _2 && _4)
            {
                tileID = 15;
                tileAngle = 1;
            }
            else if(_2 && _3 && _5)
            {
                tileID = 15;
                tileAngle = 2;
            }
            else if(_3 && _4 && _0)
            {
                tileID = 15;
                tileAngle = 3;
            }
            else if(_4 && _5 && _1)
            {
                tileID = 15;
                tileAngle = 4;
            }
            else if(_5 && _0 && _2)
            {
                tileID = 15;
                tileAngle = 5;
            }

            return new Vector2Int(tileID, tileAngle);
        }

        Vector2Int FourConnected(bool _0, bool _1, bool _2, bool _3, bool _4, bool _5)
        {
            int tileID = 0;
            int tileAngle = 0;

            if(_0 && _1 && _2 && _3)
            {
                tileID = 11;
                tileAngle = 3;
            }
            else if(_1 && _2 && _3 && _4)
            {
                tileID = 11;
                tileAngle = 4;
            }
            else if(_2 && _3 && _4 && _5)
            {
                tileID = 11;
                tileAngle = 5;
            }
            else if(_3 && _4 && _5 && _0)
            {
                tileID = 11;
                tileAngle = 0;
            }
            else if(_4 && _5 && _0 && _1)
            {
                tileID = 11;
                tileAngle = 1;
            }
            else if(_5 && _0 && _1 && _2)
            {
                tileID = 11;
                tileAngle = 2;
            }

            else if(_0 && _1 && _3 && _4)
            {
                tileID = 12;
                tileAngle = 2;
            }
            else if(_1 && _2 && _4 && _5)
            {
                tileID = 12;
                tileAngle = 3;
            }
            else if(_2 && _3 && _5 && _0)
            {
                tileID = 12;
                tileAngle = 4;
            }

            else if(_1 && _0 && _5 && _3)
            {
                tileID = 16;
                tileAngle = 1;
            }
            else if(_2 && _1 && _0 && _4)
            {
                tileID = 16;
                tileAngle = 2;
            }
            else if(_3 && _2 && _1 && _5)
            {
                tileID = 16;
                tileAngle = 3;
            }
            else if(_4 && _3 && _2 && _0)
            {
                tileID = 16;
                tileAngle = 4;
            }
            else if(_5 && _4 && _3 && _1)
            {
                tileID = 16;
                tileAngle = 5;
            }
            else if(_0 && _5 && _4 && _2)
            {
                tileID = 16;
                tileAngle = 0;
            }

            return new Vector2Int(tileID, tileAngle);
        }

        Vector2Int FiveConnected(bool _0, bool _1, bool _2, bool _3, bool _4, bool _5)
        {
            int tileID = 18;
            int tileAngle = 0;

            return new Vector2Int(tileID, tileAngle);
        }

        Vector2Int SixConnected(bool _0, bool _1, bool _2, bool _3, bool _4, bool _5)
        {
            int tileID = 18;
            int tileAngle = 0;

            return new Vector2Int(tileID, tileAngle);
        }
    }

    public class TileResult 
    {
        public List<Hex> Hexes = new();
        public List<Vector2Int> Results = new();

        public Root root;
    }
}
