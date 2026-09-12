using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Utilities;

namespace Spawn.SpawnObjects
{
    [RequireComponent(typeof(Renderer))]
    internal sealed class Bomb : MonoBehaviour
    {
        private const float MinLifetimeInSeconds = 2f;
        private const float MaxLifetimeInSeconds = 5f;

        [Space]
        [Header("Explosion")]
        [SerializeField] private float _explosionRadius = 5f;

        [SerializeField] private float _explosionForce = 100f;
        [SerializeField] private float _explosionUpwardsModifier = 0.5f;
        [SerializeField] private LayerMask _affectedLayers;

        [SerializeField] private Renderer _renderer;

        private static readonly Collider[] _overlapBuffer = new Collider[32];

        private bool _isActive;
        private Material _material;
        private Color _initialColor;
        private Action<Bomb> _returner;
        private CancellationTokenSource _lifetimeCts;

        private static float RandomDelayInSeconds => RandomGenerator.Range(MinLifetimeInSeconds, MaxLifetimeInSeconds);

        private void Awake()
        {
            _material = _renderer.material;
            _initialColor = _material.color;
        }

        private void OnValidate()
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
        }

        private void OnDisable()
        {
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            _lifetimeCts = null;
        }

        public void SetReturner(Action<Bomb> returner) =>
            _returner = returner;

        public async UniTaskVoid Activate()
        {
            if (_returner is null)
            {
                Debug.LogException(new NullReferenceException("Returner is null"));
                return;
            }

            if (_isActive)
            {
                Debug.LogWarning($"Bomb is already active, ignoring repeated {nameof(Activate)} call", this);
                return;
            }

            _isActive = true;

            try
            {
                await BecomingTransparent();
                Explode();
            }
            catch (OperationCanceledException)
            {
                return;
            }
            finally
            {
                _isActive = false;
            }

            _material.color = _initialColor;
            _returner(this);
        }

        private async UniTask BecomingTransparent()
        {
            float duration = RandomDelayInSeconds;

            _lifetimeCts?.Dispose();
            _lifetimeCts = new CancellationTokenSource();

            await _material
                .DOFade(endValue: 0f, duration)
                .ToUniTask(cancellationToken: _lifetimeCts.Token);
        }

        private void Explode()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _explosionRadius,
                _overlapBuffer,
                _affectedLayers);

            for (int i = 0; i < hitCount; i++)
            {
                Collider hitCollider = _overlapBuffer[i];

                if (hitCollider.TryGetComponent(out Cube cube) == false)
                    continue;

                cube.Rigidbody.AddExplosionForce(
                    _explosionForce,
                    transform.position,
                    _explosionRadius,
                    _explosionUpwardsModifier,
                    ForceMode.Impulse);
            }
        }
    }
}
