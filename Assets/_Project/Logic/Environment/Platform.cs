using UnityEngine;
using Spawn.SpawnObjects;

namespace Environment
{
    internal sealed class Platform : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Cube cube))
                cube.Hit().Forget();
        }
    }
}
