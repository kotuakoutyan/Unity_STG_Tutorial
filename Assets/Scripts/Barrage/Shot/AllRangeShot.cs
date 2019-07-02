using UnityEngine;
using System.Linq;
using Barrage.Bullet;
using Barrage.ObjectPool;

namespace Barrage.Shot
{
    public enum AllRangeShotShape
    {
        Random, Plane, Tetrahedron = 4, Hexahedron = 8, Octahedron = 6, Dodecahedron = 12, Icosahedron = 20,
        HighDensity10 = 36, HighDensity20 = 143, HighDensity30 = 327, HighDensity40 = 510, HighDensity50 = 912
    }

    public class AllRangeShot : NormalShot
    {
        protected readonly float INITIALDISTANCE = 3.0f;

        /// <summary>
        /// 全方位に対して弾を複数打つメソッド
        /// </summary>
        /// <param name="bulletNum">発射する弾数</param>
        /// <param name="attackerType">発射するキャラの所属</param>
        /// <param name="position">発射位置</param>
        /// <param name="shape">弾幕の形</param>
        /// <param name="bulletType">弾の種類</param>
        /// <param name="data">弾の基本データ</param>
        /// <param name="time">連射時の発射角度に影響する時間情報</param>
        /// <param name="angularSpeed">連射時に変化する角速度</param>
        /// <param name="axis">連射時に変化する基準軸</param>
        /// <param name="forward">発射方向(AllRangeShotType.Planeのみ有効)</param>
        public void Shot(int bulletNum, AttackerType attackerType, GameObject shooter, Vector3[] directions, BulletData data, GameObject target, float time = 0.0f, float angularSpeed = 0.0f, Vector3 axis = default)
        {
            var bullets = ObjectPools.Instance.GetBullet(bulletNum, data.BulletType);

            if (data.BulletType == BulletType.Curve) SetRotateOrigin(bullets, shooter, data);
            else if (data.BulletType == BulletType.Funnel) SetRotateOrigin(bullets, shooter, data, target);

            if (directions == default) directions = ShotManager.Instance.RandomVectors(bulletNum);
            if (time != 0.0f && angularSpeed != 0 && axis != default)
            {
                var rotate = Quaternion.AngleAxis(time * angularSpeed, axis);
                for (int i = 0; i < bulletNum; i++) directions[i] = rotate * directions[i];
            }
            var position = shooter.transform.position;
            for (int i = 0; i < bulletNum; i++) Shot(bullets[i], attackerType, position + directions[i] * INITIALDISTANCE, directions[i], data, target);
        }
    }
}