using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Helpers;

namespace Spawn.SpawnObjects
{
    [RequireComponent(typeof(Renderer))]
    internal sealed class Bomb : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Fader _fader;
        [SerializeField] private Exploder _exploder;

        private bool _isActive;
        private Material _material;
        private Color _initialColor;
        private Action<Bomb> _returner;
        private CancellationTokenSource _lifetimeCts;

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

            _lifetimeCts?.Dispose();
            _lifetimeCts = new CancellationTokenSource();

            try
            {
                await _fader.FadeOutByRandomDelay(_material, _lifetimeCts.Token);
                _exploder.Explode(transform.position);
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
    }
}
