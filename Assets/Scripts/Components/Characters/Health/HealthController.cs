using UniRx;
using UnityEngine;

namespace Components
{
    public class HealthController : MonoBehaviour, IHealthController
    {
        private IReactiveCommand<DeathData> _onDead;
        private IReactiveCommand<TakeDamageData> _onTakeDamage;
        private float _currentHealth;
        private float _maxHealth;

        private bool _dead;

        public void Initialize(float maxHp, IReactiveCommand<DeathData> onDead,
            IReactiveCommand<TakeDamageData> onTakeDamage = null)
        {
            _maxHealth = maxHp;
            _currentHealth = maxHp;

            _onDead = onDead;
            _onTakeDamage = onTakeDamage;
        }

        public void ProcessDamage(float damage)
        {
            if (_dead)
            {
                return;
            }

            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _onDead?.Execute(new DeathData()
                {
                    Position = transform.position
                });
                return;
            }
            
            _onTakeDamage?.Execute(new TakeDamageData()
            {
                CurrentHealth = _currentHealth,
                MaxHealth = _maxHealth,
            });
        }
    }
}