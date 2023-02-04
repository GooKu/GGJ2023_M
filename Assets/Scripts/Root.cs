using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class Root
    {
        public enum Level
        {
            Main = 0,
            Sub
        }

        int _energyCost;
        //0:main, 1:sub
        public Level BranchLevel { get; private set; }

        Root _lastRoot;
        List<Root> _childRoots = new();
        Hex _rootHex;

        public Root(Root lastRoot, Hex rootHex, Level branchLevel)
        {
            _lastRoot = lastRoot;
            _rootHex = rootHex;
            BranchLevel = branchLevel;
        }

        public void UpdateLevel(Level level)
        {
            BranchLevel = level;
        }

        public int CalculateCost()
        {
            //or just return branchLevel
            switch (BranchLevel)
            {
                case 0:
                    _energyCost = 1;
                    break;
                default:
                    break;
            }
            return _energyCost;
        }

        public Hex ReturnHex()
        {
            return _rootHex;
        }

        public void CreateChildRoot(Hex childRootHex)
        {
            //Instantiate(prefab, childRootHex, quaternion.identity, ???).addComponent(Root).Init(this, childRoothex, _branchLevel);
            //Player.AddRoot();
            //_childRoots.Add();
        }

        public void SeperateRoot(Hex rootHex)
        {
            //Instantiate(prefab, rootHex, quaternion.identity, ???).addComponent(Root).Init(this, roothex, _branchLevel + 1);
            //Player.AddRoot();
            //_childRoots.Add();
        }
    }

}
