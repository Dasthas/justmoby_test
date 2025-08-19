using System;
using Components;
using UniRx;

namespace Services
{
    public interface IEnemyService
    {
        IObservable<DeathData> EveryEnemyDeath { get; }
    }
}