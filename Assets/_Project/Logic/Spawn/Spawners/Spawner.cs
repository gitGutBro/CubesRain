using System;
using UnityEngine;
using Core;
using Spawn.Spawners;

namespace Spawn
{
    internal abstract class Spawner<TSpawnable> : MonoBehaviour where TSpawnable : MonoBehaviour
    {
        [SerializeField] private ObjectPool<TSpawnable> _pool;

        private SpawnStatsModel _statsModel;
        private Action<TSpawnable> _returner;

        protected TSpawnable CurrentObj => GetObject();
        protected Action<TSpawnable> Returner => _returner ??= ReturnObject;
        protected bool IsStatsModelNull => _statsModel == null;

        private void Awake() =>
            _pool.Created += OnCreated;

        private void OnDestroy() =>
            _pool.Created -= OnCreated;

        public void InitStatsModel(SpawnStatsModel statsModel) =>
            _statsModel = statsModel;

        private TSpawnable GetObject()
        {
            TSpawnable obj = _pool.Get();
            _statsModel?.AddSpawned();
            _statsModel?.AddActive();
            return obj;
        }

        private void ReturnObject(TSpawnable obj)
        {
            _pool.Put(obj);
            _statsModel?.RemoveActive();
        }

        private void OnCreated(TSpawnable obj) =>
            _statsModel?.AddCreated();
    }
}
