using System.Collections.Generic;

namespace GGJ23M
{
    public class Player
    {
        int _currentEnergy;

        bool _playerDead;

        List<Root> _roots;

        public void AddEnergy(int addValue)
        {
            _currentEnergy += addValue;
        }

        public void RemoveEnergy(int removeValue)
        {
            _currentEnergy -= removeValue;

            if (_currentEnergy <= 0)
            {
                _playerDead = true;
            }
        }

        public void AddRoot(Root root)
        {
            _roots.Add(root);
        }

        public List<Root> ReturnRoots()
        {
            return _roots;
        }

        public bool IfPlayerDead()
        {
            return _playerDead;
        }
    }
}
