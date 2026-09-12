using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;
using Spawn.SpawnObjects;

namespace Spawn.Spawners
{
    internal sealed class CubeSpawner : Spawner<Cube>
    {
        [Space]
        [SerializeField] private BoxCollider _spawnArea;
        [Space]
        [Range(0.1f, 1f)]
        [SerializeField] private float _minDelayInSeconds;

        private Action<Vector3> _spawnBomb;

        private void Start()
        {
            if (_spawnBomb == null)
                throw new NullReferenceException($"{nameof(_spawnBomb)} not init");

            if (IsStatsModelNull)
                throw new NullReferenceException($"Model not init");

            Spawning().Forget();
        }

        public void InitBombSpawn(Action<Vector3> spawnBomb) =>
            _spawnBomb = spawnBomb;

        private async UniTaskVoid Spawning()
        {
            while (enabled)
            {
                Cube cube = CurrentObj;

                cube.LifetimeEnded += Returner;
                cube.LifetimeEnded += OnSpawnBombAt;

                cube.transform.position = RandomGenerator.GetRandomPoint(_spawnArea.bounds);

                await UniTask.WaitForSeconds(_minDelayInSeconds);
            }
        }

        private void OnSpawnBombAt(Cube cube) =>
            _spawnBomb(cube.transform.position);
    }
}
