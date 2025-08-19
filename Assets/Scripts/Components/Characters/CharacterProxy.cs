using System;
using UniRx;
using UnityEngine;

namespace Components
{
    public class CharacterProxy : MonoBehaviour, ICharacter
    {
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private HealthController _healthController;

        private ReactiveCommand<DeathData> _onDead = new ReactiveCommand<DeathData>();
        private ReactiveCommand<TakeDamageData> _onTakeDamage = new ReactiveCommand<TakeDamageData>();
        public IObservable<DeathData> OnDead => _onDead;
        public IObservable<TakeDamageData> OnTakeDamage => _onTakeDamage;

        public void Initialize(float maxHp)
        {
            _healthController.Initialize(maxHp, _onDead, _onTakeDamage);
        }
    }
}