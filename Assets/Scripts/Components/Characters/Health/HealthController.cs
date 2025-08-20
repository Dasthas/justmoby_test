using UniRx;
using UnityEngine;

namespace Components.Characters
{
    public class HealthController : MonoBehaviour, IHealthController
    {
        private IReactiveCommand<DeathData> _onDead;
        private IReactiveCommand<HealthChangedData> _onTakeDamage;
        private float _currentHealth;
        private float _maxHealth;

        private bool _dead;

        public void Initialize(float maxHp, IReactiveCommand<DeathData> onDead,
            IReactiveCommand<HealthChangedData> onTakeDamage = null)
        {
            _maxHealth = maxHp;
            _currentHealth = maxHp;

            _onDead = onDead;
            _onTakeDamage = onTakeDamage;
            SendHealthChangedData();
        }

        public void SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            
            SendHealthChangedData();
        }
        
        public void Heal(float heal)
        {
            _currentHealth += heal;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            
            SendHealthChangedData();
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
                _dead = true;
                _onDead?.Execute(new DeathData()
                {
                    Position = transform.position
                });
                return;
            }

            SendHealthChangedData();
        }

        private void SendHealthChangedData()
        {
            _onTakeDamage?.Execute(new HealthChangedData()
            {
                CurrentHealth = _currentHealth,
                MaxHealth = _maxHealth,
            });
        }
    }
}