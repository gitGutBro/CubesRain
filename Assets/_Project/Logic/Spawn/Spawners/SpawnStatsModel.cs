using System;

namespace Spawn.Spawners
{
    internal sealed class SpawnStatsModel
    {
        private int _totalSpawned;
        private int _countCreated;
        private int _countActive;

        public event Action<int> SpawnedAdded;
        public event Action<int> CreatedAdded;
        public event Action<int> ActiveChanged;

        public void AddSpawned()
        {
            _totalSpawned++;
            SpawnedAdded?.Invoke(_totalSpawned);
        }

        public void AddCreated()
        {
            _countCreated++;
            CreatedAdded?.Invoke(_countCreated);
        }

        public void AddActive()
        {
            _countActive++;
            ActiveChanged?.Invoke(_countActive);
        }

        public void RemoveActive()
        {
            _countActive--;
            ActiveChanged?.Invoke(_countActive);
        }
    }
}
