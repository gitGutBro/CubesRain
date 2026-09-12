using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Utilities;

namespace Helpers
{
    [Serializable]
    internal sealed class Fader
    {
        [Space]
        [Header("Fader")]
        [SerializeField] private float _minDurationInSeconds = 2f;
        [SerializeField] private float _maxDurationInSeconds = 5f;

        private float RandomDurationInSeconds => RandomGenerator.Range(_minDurationInSeconds, _maxDurationInSeconds);

        public async UniTask FadeOutByRandomDelay(Material material, CancellationToken token) =>
            await material
                .DOFade(endValue: 0f, RandomDurationInSeconds)
                .ToUniTask(cancellationToken: token);
    }
}
