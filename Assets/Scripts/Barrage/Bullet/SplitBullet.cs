using Barrage.Iterators;
using Barrage.ObjectPool;
using Barrage.Shot;
using UnityEngine;

namespace Barrage.Bullet
{
    public class SplitBullet : BaseBullet
    {
        protected override void AdditionalUpdate(){ }
        
        protected override void ResetObject()
        {
            if (Data.SplitData != null)
            {
                ObjectPools.Instance.GetBullet(1, BulletType.Funnel)[0].Initialize(AttackerType, transform.position, Vector3.zero, Data, Target, Vector3.zero);
            }
            base.ResetObject();
        }
    }
}