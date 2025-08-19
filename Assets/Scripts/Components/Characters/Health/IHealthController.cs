using UniRx;

namespace Components
{
    public interface IHealthController
    {
        void ProcessDamage(float damage);
        void Initialize(float maxHp, IReactiveCommand<DeathData> onDead, IReactiveCommand<TakeDamageData> onTakeDamage = null);
    }
}