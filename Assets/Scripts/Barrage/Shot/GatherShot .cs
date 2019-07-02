﻿using UnityEngine;
using System.Linq;
using Barrage.Bullet;
using Barrage.ObjectPool;

using Random = UnityEngine.Random;

namespace Barrage.Shot
{
    public class GatherShot : NormalShot
    {
        /// <summary>
        /// 発射した場所に向かって飛ぶ弾を発射するメソッド
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
        /// <param name="throughShootPoint">発射位置を通り過ぎて対称点で消失するフラグ</param>
        /// <param name="distanceFromShootPoint">通過位置を発射位置からズラす距離</param>
        public void Shot(int bulletNum, AttackerType attackerType, GameObject shooter, Vector3[] directions, BulletData data, GameObject target, float time, float angularSpeed, Vector3 axis, bool throughShootPoint, float distanceFromShootPoint)
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

            Vector3[] offsets = new Vector3[bulletNum];
            var backDistance = -data.Speed * data.ValidTime;
            if (throughShootPoint) backDistance /= 2;
            for (int i = 0; i < bulletNum; i++) offsets[i] = directions[i] * backDistance;

            var position = shooter.transform.position;
            for (int i = 0; i < bulletNum; i++) Shot(bullets[i], attackerType, position, directions[i], data, default, offsets[i] + directions[i].GetOrthogonalUnitVector() * distanceFromShootPoint);
        }
    }
}