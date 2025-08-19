using System;

namespace Components
{
    public interface ICharacter
    {
        public IObservable<DeathData> OnDead { get; }
        IObservable<TakeDamageData> OnTakeDamage { get; }
    }
}