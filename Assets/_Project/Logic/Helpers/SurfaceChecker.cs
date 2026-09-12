using System;
using UnityEngine;

namespace Helpers
{
    internal sealed class SurfaceChecker : MonoBehaviour
    {
        public event Action<ISurface> SurfaceHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ISurface surface))
                SurfaceHit?.Invoke(surface);
        }
    }
}
