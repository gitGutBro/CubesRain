using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    [Serializable]
    internal sealed class ObjectPool<TObject> where TObject : MonoBehaviour
    {
        [SerializeField] private TObject _prefab;

        private readonly Queue<TObject> _pool = new();

        public event Action<TObject> Created;

        public TObject Get()
        {
            TObject obj = _pool.Count > 0 ? _pool.Dequeue() : Create();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Put(TObject obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }

        private TObject Create()
        {
            TObject obj = Object.Instantiate(_prefab);
            obj.gameObject.SetActive(false);
            Created?.Invoke(obj);
            return obj;
        }
    }
}
