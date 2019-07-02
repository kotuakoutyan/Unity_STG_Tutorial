using UnityEngine;
using Barrage.Bullet;
using Barrage.ObjectPool;

namespace Barrage.Shot
{
    public class NormalShot
    {
        /// <summary>
        /// 時間による発射角度の変化を行うか
        /// </summary>
        protected bool RotateByTime(AllRangeShotShape shape) => shape != AllRangeShotShape.Random;

        /// <summary>
        /// 進行方向を指定するか
        /// </summary>
        protected bool RotateForDirection(AllRangeShotShape shape) => shape == AllRangeShotShape.Plane;

        /// <summary>
        /// 弾のブレの最高角度
        /// </summary>
        protected readonly float MAX_SHAKE = 60.0f;

        /// <summary>
        /// 弾を複数発射する一番基本的なメソッド
        /// </summary>
        /// <param name="bulletNum">発射する弾数</param>
        /// <param name="attackerType">発射するキャラの所属</param>
        /// <param name="position">発射位置</param>
        /// <param name="velocity">弾の進行方向</param>
        /// <param name="bulletType">弾の種類</param>
        /// <param name="data">弾の基本データ</param>
        /// <param name="randomDiffusion">弾のブレをランダムにするFlag</param>
        /// <param name="bulletShake">弾のブレ(0.0-1.0)</param>
        public void Shot(int bulletNum, AttackerType attackerType, GameObject shooter, BulletType bulletType, BulletData data, GameObject aim, GameObject target, bool randomDiffusion, float bulletShake)
        {
            var bullets = ObjectPools.Instance.GetBullet(bulletNum, bulletType);

            Vector3 velocity = default;
            if (aim != default) velocity = (aim.transform.position - shooter.transform.position).normalized;
            else velocity = shooter.transform.forward;

            if (data.BulletType == BulletType.Curve) SetRotateOrigin(bullets, shooter, data);
            else if (data.BulletType == BulletType.Funnel) SetRotateOrigin(bullets, shooter, data, target);

            var position = shooter.transform.position;
            if (randomDiffusion || bulletShake == default) foreach (BaseBullet bullet in bullets) Shot(bullet, attackerType, position, velocity.RandamShake(MAX_SHAKE * bulletShake), data, target);
            else
            {
                var newVelocitys = FixedShake(velocity, MAX_SHAKE * bulletShake, bulletNum);
                for(int i = 0;i < bulletNum;i++) Shot(bullets[i], attackerType, position, newVelocitys[i], data, target);
            }
        }

        /// <summary>
        /// 弾を発射する一番基本的なメソッド
        /// </summary>
        /// <param name="bullet">発射する弾</param>
        /// <param name="attackerType">発射するキャラの所属</param>
        /// <param name="position">発射位置</param>
        /// <param name="velocity">弾の進行方向</param>
        /// <param name="data">弾の基本データ</param>
        /// <param name="offset">発射位置からズレるVector</param>
        protected void Shot(BaseBullet bullet, AttackerType attackerType, Vector3 position, Vector3 velocity,  BulletData data, GameObject target = default, Vector3 offset = default)
        {
             bullet.Initialize(attackerType, position, velocity, data, target, offset);
        }

        /// <summary>
        /// 弾道を均等にばらけさせるメソッド
        /// </summary>
        /// <param name="velocity">基準となる弾道</param>
        /// <param name="bulletShake">基準からズレる割合</param>
        /// <param name="bulletNum">必要な進行方向の数</param>
        private Vector3[] FixedShake(Vector3 velocity, float bulletShake, int bulletNum)
        {
            var cycle = 360f / bulletNum;
            var velocitys = new Vector3[bulletNum];
            for (int i = 0; i < bulletNum; i++)
            {
                var newDirection = Quaternion.Euler(0, 0, cycle * i) * Quaternion.Euler(bulletShake, 0, 0) * Vector3.forward;
                var direction = Quaternion.FromToRotation(Vector3.forward, velocity);
                velocitys[i] = direction * newDirection;
            }
            return velocitys;
        }

        /// <summary>
        /// 回転する弾に対して、回転する親オブジェクトを設定するメソッド
        /// </summary>
        /// <param name="bullets"></param>
        /// <param name="shooter"></param>
        /// <param name="data"></param>
        /// <param name="target"></param>
        protected void SetRotateOrigin(BaseBullet[] bullets, GameObject shooter, BulletData data, GameObject target = default)
        {
            var origin = new GameObject().AddComponent<RotateOrigin>();
            var transform = origin.transform;
            origin.Initialize(shooter, data.AngularSpeed, data.Axis, target);

            foreach (BaseBullet bullet in bullets) bullet.transform.SetParent(origin.transform);
        }
    }
}
