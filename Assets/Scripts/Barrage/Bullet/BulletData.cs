using UnityEngine;

namespace Barrage.Bullet
{
    public enum BulletType
    {
        Normal, Curve, Split, Chase, Funnel
    }

    [CreateAssetMenu(
        fileName = "BulletData",
        menuName = "ScriptableObject/BulletData",
        order = 0)]
    public class BulletData : ScriptableObject
    {
        /*標準パラメータ*/
        /// <summary>
        /// 弾の種類
        /// </summary>
        public BulletType BulletType;

        /// <summary>
        /// 弾のスピード
        /// </summary>
        public float Speed;

        /// <summary>
        /// 弾の有効時間
        /// </summary>
        public float ValidTime;

        /// <summary>
        /// 弾のダメージ量
        /// </summary>
        public int Damage;



        /*SplitBulelt用のパラメータ*/
        /// <summary>
        /// 弾が有効時間を迎えたときに発動する弾幕のデータ
        /// </summary>
        public BarrageData SplitData;



        /*CurveBullet / FunnelBullet用のパラメータ*/
        /// <summary>
        /// 弾の角速度
        /// </summary>
        public float AngularSpeed;

        /// <summary>
        /// 弾の回転する軸
        /// </summary>
        public Vector3 Axis;



        /*FunnelBullet用のパラメータ*/
        /// <summary>
        /// ファンネルが展開する距離
        /// </summary>
        public float DeploymentDistance;

        /// <summary>
        /// ファンネルが展開するまでの時間
        /// </summary>
        public float DeploymentTime;



        /*ChaseBullet用のパラメータ*/
        /// <summary>
        /// 弾がターゲットを追尾する力
        /// </summary>
        public float ChasePower;
        
        /// <summary>
        /// 弾が追尾を開始するまでの時間
        /// </summary>
        public float ChaseEnableTime;
    }
}
