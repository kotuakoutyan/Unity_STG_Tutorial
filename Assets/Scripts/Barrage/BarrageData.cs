using Barrage.Bullet;
using Barrage.Shot;
using System.Collections.Generic;
using UnityEngine;

namespace Barrage
{
    [CreateAssetMenu(
        fileName = "BarrageData",
        menuName = "ScriptableObject/BarrageData",
        order = 2)]
    public class BarrageData : ScriptableObject
    {
        public List<ShotData> ShotDatas = new List<ShotData>();
        public List<BulletData> BulletDatas = new List<BulletData>();
    }
}