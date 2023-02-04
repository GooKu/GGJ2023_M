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

        public bool WasMainRoot => _wasMainRoot;

        Root _lastRoot;
        List<Root> _childRoots = new();
        Hex _rootHex;

        bool _wasMainRoot;

        public Root(Root lastRoot, Hex rootHex, Level branchLevel)
        {
            _lastRoot = lastRoot;
            _rootHex = rootHex;
            BranchLevel = branchLevel;
            _wasMainRoot = branchLevel == Level.Main;
        }

        public void UpdateLevel(Level level)
        {
            BranchLevel = level;
        }

        public Hex ReturnHex()
        {
            return _rootHex;
        }

    }

}
