
using Barrage;
using Barrage.Bullet;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace FlyCharacter
{
    public class FlyCharacterStatus : MonoBehaviour
    {
        public AttackerType AttackerType;

        public int Hp;
        public int Mp;
        public int Stamina;

        public GameObject Target;

        public BarrageData BarrageData1;
        public BarrageData BarrageData2;
        public BarrageData BarrageData3;
        public BarrageData BarrageData4;

        public IReadOnlyReactiveProperty<bool> IsRendered => isRendered;
        private readonly ReactiveProperty<bool> isRendered = new ReactiveProperty<bool>(false);


        void Start()
        {
            FlyCharacterManager.Instance.Characters.Add(this);
            this.UpdateAsObservable().Subscribe(_ => isRendered.Value = false);
        }

        private void OnWillRenderObject()
        {
            if (Camera.current.tag == "MainCamera")
            {
                isRendered.Value = true;
            }
        }
        /*
        //デモ用のBulletを設定
        void Start()
        {
            //通常弾
            ShotData1 = new ShotData
            {
                BulletData = new BulletData
                {
                    Speed = 30,
                    ValidTime = 30,
                    BulletShake = 0.2f
                },
                ShotType = ShotType.Normal,
                BulletType = BulletType.Normal,
                BulletNum = 20,
            };

            //全方位弾
            ShotData2 = new ShotData
            {
                BulletData = new BulletData
                {
                    Speed = 30,
                    ValidTime = 30,
                },
                ShotType = ShotType.AllRange,
                AllRangeShotShape = AllRangeShotShape.HighDensity,
                BulletType = BulletType.Normal,
                Cycle = 1.0f,
                Count = 3,
                AngularSpeed = 30,
                Axis = Vector3.up
            };

            //全方位回転弾
            ShotData3 = new ShotData
            {
                BulletData = new BulletData
                {
                    Speed = 10,
                    ValidTime = 30,
                    AngularSpeed = 90,
                    Axis = Vector3.up
                },
                ShotType = ShotType.AllRange,
                AllRangeShotShape = AllRangeShotShape.Random,
                BulletType = BulletType.Curve,
                BulletNum = 40
            };

            //分裂した後の弾
            var attackerType = GetComponent<FlyCharacterStatus>().AttackerType;
            var splitBullet = new ShotData
            {
                BulletData = new BulletData
                {
                    Speed = 30,
                    ValidTime = 30,
                },
                ShotType = ShotType.AllRange,
                AllRangeShotShape = AllRangeShotShape.Random,
                BulletType = BulletType.Normal,
                BulletNum = 40
            };

            //分裂弾
            ShotData4 = new ShotData
            {
                BulletData = new BulletData
                {
                    Speed = 10,
                    ValidTime = 5,
                    SplitShotData = splitBullet,
                    BulletShake = 1.0f
                },
                ShotType = ShotType.Normal,
                BulletType = BulletType.Split,
                BulletNum = 10
            };
        }
        */
    }
}