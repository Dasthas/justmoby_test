using System;
using UnityEngine;

namespace Components.Characters
{
    public interface ICharacter
    {
        public IObservable<DeathData> OnDead { get; }
        IObservable<HealthChangedData> OnHealthChanged { get; }
        void Initialize(float maxHp);
        void Move(Vector3 velocity);
        void LookToDirectionSmooth(Vector3 direction, float speed);
    }
}