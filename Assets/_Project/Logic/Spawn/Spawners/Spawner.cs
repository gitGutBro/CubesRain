using System;
using UnityEngine;
using Core;
using Spawn.Spawners;
using Spawn.SpawnObjects;

namespace Spawn
{
    internal abstract class Spawner<TSpawnable> : MonoBehaviour
        where TSpawnable : MonoBehaviour, IExpirable<TSpawnable>
    {
        [SerializeField] private ObjectPool<TSpawnable> _pool;

        private SpawnStatsModel _statsModel;
        private Action<TSpawnable> _returner;

        protected bool IsStatsModelNull => _statsModel == null;

        private void Awake()
        {
            _pool.Created += OnCreated;
            _returner = ReturnObject;

            OnAwake();
        }

        private void OnDestroy() =>
            _pool.Created -= OnCreated;

        public void InitStatsModel(SpawnStatsModel statsModel) =>
            _statsModel = statsModel;

        protected virtual void OnAwake() { }

        protected TSpawnable GetObject()
        {
            TSpawnable obj = _pool.Get();
            obj.LifetimeEnded += _returner;
            _statsModel?.AddSpawned();
            _statsModel?.AddActive();
            return obj;
        }

        private void ReturnObject(TSpawnable obj)
        {
            obj.LifetimeEnded -= _returner;
            _pool.Put(obj);
            _statsModel?.RemoveActive();
        }

        private void OnCreated(TSpawnable obj) =>
            _statsModel?.AddCreated();
    }
}
