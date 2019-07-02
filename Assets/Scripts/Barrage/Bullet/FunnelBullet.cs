using Barrage.Iterators;
using UnityEngine;

namespace Barrage.Bullet
{
    public class FunnelBullet : BaseBullet
    {
        public Iterator Iterator;
        
        protected Vector3 Acceleration;
        
        protected override void Awake()
        {
            base.Awake();
            Iterator = gameObject.AddComponent<Iterator>();
        }
        
        public override void Initialize(AttackerType attackerType, Vector3 position, Vector3 velocity, BulletData data, GameObject target, Vector3 offset)
        {
            Iterator.SetUnityAction(attackerType, data.SplitData, target, target);
                        
            Acceleration = 2 * (data.DeploymentDistance - data.Speed *  data.DeploymentTime) / data.DeploymentTime / data.DeploymentTime * velocity;
            base.Initialize(attackerType, position, velocity, data, target, offset);
        }

        protected override void AdditionalUpdate()
        {
            if (Time.time - InitializeTime < Data.DeploymentTime) Velocity += Acceleration * Time.fixedDeltaTime;
        }

        protected override void ResetObject()
        {
            base.ResetObject();
            Iterator.RemoveAll();

            var parent = transform.parent;
            if (parent != null) parent.GetComponent<RotateOrigin>().ResetObject();
        }
    }
}
