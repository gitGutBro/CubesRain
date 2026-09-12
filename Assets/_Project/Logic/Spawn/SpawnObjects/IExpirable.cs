using System;

namespace Spawn.SpawnObjects
{
    internal interface IExpirable<TSelf>
    {
        event Action<TSelf> LifetimeEnded;
    }
}
