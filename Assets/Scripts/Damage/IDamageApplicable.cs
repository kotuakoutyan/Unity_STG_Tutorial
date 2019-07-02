using Barrage.Bullet;

namespace Damage
{
    public interface IDamageApplicable
    {
        AttackerType GetAttackerType();
        void ApplyDamage(DamageData data);
    }
}
