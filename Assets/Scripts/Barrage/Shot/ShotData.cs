using UnityEngine;

namespace Barrage.Shot
{
    public enum ShotType
    {
        Normal, AllRange, Gather
    }

    [CreateAssetMenu(
        fileName = "ShotData",
        menuName = "ScriptableObject/ShotData",
        order = 1)]
    public class ShotData : ScriptableObject
    {
        /*標準パラメータ*/
        /// <summary>
        /// Shotの種類
        /// </summary>
        public ShotType ShotType;

        /// <summary>
        /// AllRangeShotShapeで弾数を指定されていないときに適用される、発射する弾数
        /// </summary>
        public int BulletNum;


        /*NormalShotのパラメータ*/
        /// <summary>
        /// NormalBulletを撃つ際に適用される、拡散性がランダムかのフラグ
        /// </summary>
        public bool RandomDiffusion; 

        /// <summary>
        /// NormalBulletを撃つ際に適用される、目標ベクトルからズレる割合
        /// </summary>
        [Range(0.0f, 1.0f)] public float BulletShake; 



        /*AllRangeShotのパラメータ*/
        /// <summary>
        /// ShotTypeがAllRangeShotの場合に適用される、弾幕の形
        /// </summary>
        public AllRangeShotShape AllRangeShotShape;



        /*GatherShotのパラメータ*/
        /// <summary>
        /// ターゲットに対して集まるように撃つかのフラグ
        /// </summary>
        public bool GatherAtTarget;

        /// <summary>
        /// GatherShotを撃つ際に適用される、発射位置を通過してその対称点まで有効にするかのフラグ
        /// </summary>
        public bool PassGatherPoint;

        /// <summary>
        /// IsThroughが有効の時、発射位置からどの程度離れて通過するか
        /// </summary>
        public float DistanceFromGatherPoint; 



        /*繰り返しに関するパラメータ*/
        /// <summary>
        /// 発射までの遅延時間
        /// </summary>
        public float Delay;

        /// <summary>
        /// 発射周期
        /// </summary>
        public float Cycle;

        /// <summary>
        /// 発射の繰り返し回数
        /// </summary>
        public int Count;
        
        /// <summary>
        /// 連射時に変化する角速度
        /// </summary>
        public float AngularSpeed;

        /// <summary>
        /// 連射時に変化する基準軸
        /// </summary>
        public Vector3 Axis;

        public int GetBulletNum => (AllRangeShotShape == AllRangeShotShape.Random || AllRangeShotShape == AllRangeShotShape.Plane) ? BulletNum : (int)AllRangeShotShape;
    }
}