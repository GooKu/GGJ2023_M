using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class Root : MonoBehaviour
    {
        int _energyCost;
        int _branchLevel;

        Root _lastRoot;
        List<Root> _childRoots;
        Hex _rootHex;


        public void Init(Root lastRoot, Hex rootHex, int branchLevel)
        {
            _lastRoot = lastRoot;
            _rootHex = rootHex;
            _branchLevel = branchLevel;
        }
        public int CalculateCost()
        {
            //or just return branchLevel
            switch (_branchLevel)
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
