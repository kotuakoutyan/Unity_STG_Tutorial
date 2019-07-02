using UnityEngine;

namespace Barrage.Bullet
{
    public class ChaseBullet : BaseBullet
    {
        [SerializeField] protected bool FirstContact = false;
        [SerializeField] protected bool IsEnable = true;

        protected override void AdditionalUpdate()
        {
            if (!FirstContact &&!IsEnable && Time.time - InitializeTime > Data.ChaseEnableTime) IsEnable = true;
            if (IsEnable && Target != null)
            {
                var target_vec = (Target.transform.position - transform.position).normalized * Data.Speed;

                if (!FirstContact && Vector3.Dot(target_vec, Velocity) > 0.8f) FirstContact = true;
                if (FirstContact && Vector3.Dot(target_vec, Velocity) < 0.8f)
                {
                    InitializeTime = Time.time - Data.ValidTime + 0.5f;
                    IsEnable = false;
                }
                Velocity = Vector3.Slerp(Velocity, target_vec, Data.ChasePower * Time.fixedDeltaTime);
            }
        }
        public override void Initialize(AttackerType attackerType, Vector3 position, Vector3 velocity, BulletData data, GameObject target, Vector3 offset)
        {
            base.Initialize(attackerType, position, velocity, data, target, offset);
            FirstContact = false;
            IsEnable = false;
        }
    }
}
