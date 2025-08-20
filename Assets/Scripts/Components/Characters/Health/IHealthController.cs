using UniRx;

namespace Components.Characters
{
    public interface IHealthController
    {
        void ProcessDamage(float damage);
        void Initialize(float maxHp, IReactiveCommand<DeathData> onDead, IReactiveCommand<HealthChangedData> onTakeDamage = null);
    }
}