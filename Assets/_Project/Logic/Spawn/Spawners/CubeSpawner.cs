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

        public event Action<Vector3> CubeDied;

        private void Start()
        {
            if (IsStatsModelNull)
                throw new NullReferenceException("Model not init");

            Spawning().Forget();
        }

        private async UniTaskVoid Spawning()
        {
            while (enabled)
            {
                Cube cube = GetObject();
                cube.LifetimeEnded += OnCubeDied;
                cube.transform.position = RandomGenerator.GetRandomPoint(_spawnArea.bounds);

                await UniTask.WaitForSeconds(_minDelayInSeconds);
            }
        }

        private void OnCubeDied(Cube cube)
        {
            cube.LifetimeEnded -= OnCubeDied;
            CubeDied?.Invoke(cube.transform.position);
        }
    }
}
