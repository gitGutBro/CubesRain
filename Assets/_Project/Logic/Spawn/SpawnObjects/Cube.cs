using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Helpers;
using Utilities;

namespace Spawn.SpawnObjects
{
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(Rigidbody))]
    internal sealed class Cube : MonoBehaviour, IExpirable<Cube>
    {
        private const float MinLifetimeInSeconds = 2f;
        private const float MaxLifetimeInSeconds = 5f;

        [SerializeField] private Renderer _renderer;
        [SerializeField] private SurfaceChecker _surfaceChecker;

        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        private bool _isColorChanged;
        private Color _originalColor;

        public event Action<Cube> LifetimeEnded;

        private static float LifetimeDelay => RandomGenerator.Range(MinLifetimeInSeconds, MaxLifetimeInSeconds);

        private void Awake()
        {
            _originalColor = _renderer.material.color;

            _surfaceChecker.SurfaceHit += OnSurfaceHit;
        }

        private void OnValidate()
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();

            if (Rigidbody == null)
                Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            LifetimeEnded = null;
            _surfaceChecker.SurfaceHit -= OnSurfaceHit;
        }

        private void OnSurfaceHit(ISurface surface) =>
            Hit().Forget();

        private async UniTaskVoid Hit()
        {
            if (_isColorChanged)
                return;

            _isColorChanged = true;

            try
            {
                await DoHitActions();
            }
            finally
            {
                ResetState();
            }
        }

        private void ResetState()
        {
            try
            {
                LifetimeEnded?.Invoke(this);
            }
            finally
            {
                _isColorChanged = false;
                _renderer.material.color = _originalColor;
                LifetimeEnded = null;
            }
        }

        private async UniTask DoHitActions()
        {
            _renderer.material.color = RandomGenerator.ColorHSV;
            await UniTask.WaitForSeconds(LifetimeDelay);
        }
    }
}
