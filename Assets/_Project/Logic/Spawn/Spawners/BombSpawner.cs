using UnityEngine;
using Spawn.SpawnObjects;

namespace Spawn.Spawners
{
    internal sealed class BombSpawner : Spawner<Bomb>
    {
        public void Spawn(Vector3 position)
        {
            Bomb bomb = CurrentObj;
            bomb.SetReturner(Returner);
            bomb.transform.position = position;

            bomb.Activate().Forget();
        }
    }
}
