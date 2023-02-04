using System.Collections.Generic;

namespace GGJ23M
{
    public class Player
    {
        public System.Action<int> EnergyChnageEvent;

        int _currentEnergy;

        bool _playerDead;

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

            if (_currentEnergy <= 0)
            {
                _playerDead = true;
            }

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

        public bool IfPlayerDead()
        {
            return _playerDead;
        }
    }
}
