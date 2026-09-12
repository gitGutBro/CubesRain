using System;
using UnityEngine;

namespace Helpers
{
    [Serializable]
    internal sealed class Exploder
    {
        private static readonly Collider[] s_overlapBuffer = new Collider[32];

        [Space]
        [Header("Explosion")]
        [SerializeField] private float _radius = 5f;
        [SerializeField] private float _force = 100f;
        [SerializeField] private float _upwardsModifier = 0.5f;
        [SerializeField] private LayerMask _affectedLayers;

        public void Explode(Vector3 position)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                position,
                _radius,
                s_overlapBuffer,
                _affectedLayers);

            for (int i = 0; i < hitCount; i++)
            {
                Collider hitCollider = s_overlapBuffer[i];

                if (hitCollider.attachedRigidbody == null)
                    continue;

                hitCollider.attachedRigidbody.AddExplosionForce(
                    _force,
                    position,
                    _radius,
                    _upwardsModifier,
                    ForceMode.Impulse);
            }
        }
    }
}
