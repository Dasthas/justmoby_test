using System;
using Components.Characters;

namespace Services.Enemy
{
    public interface IEnemyService
    {
        IObservable<DeathData> OnAnyEnemyDeath { get; }
    }
}