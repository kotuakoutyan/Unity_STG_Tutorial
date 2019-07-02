using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Barrage.Shot;
using Barrage.Bullet;

namespace Barrage.Iterators
{
    /// <summary>
    /// 登録されたEventを指定間隔で指定回数発動するクラス
    /// </summary>
    public class Iterator : MonoBehaviour
    {
        private float RandomNum;      //何かしらに使えるそのIteratorごとに割り振った乱数

        /// <summary>
        /// UnityActionとそのサイクル、期限を持たせたクラス
        /// </summary>
        private class MyEvent
        {
            UnityAction<float> Action = null;
            float TotalTime = 0.0f;
            float Timer = 0.0f;
            int Count = 0;
            ShotData Data = null;

            
            public MyEvent(UnityAction action, ShotData data)
            {
                Action = (t) => action();
                Timer = data.Delay;
                Count = data.Count;
                Data = data;
            }
            public MyEvent(UnityAction<float> action, ShotData data)
            {
                Action = action;
                Timer = data.Delay;
                Count = data.Count;
                Data = data;
            }

            public bool Invoke()
            {
                TotalTime += Time.deltaTime;
                if (Timer > 0) Timer -= Time.deltaTime;
                else
                {
                    if (Count < 1) return true;
                    else Count--;
                    Action(TotalTime);
                    Timer = Data.Cycle;
                }
                return false;
            }
        }

        private List<MyEvent> Events = new List<MyEvent>();
        private void SetEvent(UnityAction action, ShotData data) => Events.Add(new MyEvent(action, data));
        private void SetEvent(UnityAction<float> action, ShotData data) => Events.Add(new MyEvent(action, data));
        public void RemoveAll() => Events.RemoveAll(_ => true);

        void Awake()
        {
            RandomNum = Random.value;
        }

        void Update()
        {
            for (int i = Events.Count - 1; i >= 0; i--)
            {
                if (Events[i].Invoke()) Events.RemoveAt(i);
            }
        }

        /// <summary>
        /// ShotDataをもとに生成したActionをIteratorにセット or 単発なら実行するメソッド
        /// </summary>
        /// <param name="data">発射するShotData</param>
        public void SetUnityAction(AttackerType attackerType, BarrageData data, GameObject aim = default, GameObject target = default)
        {
            if (data == null) return;
            for (int i = 0; i < data.ShotDatas.Count; i++)
            {
                if (data.ShotDatas[i] == default || data.BulletDatas[i] == default) continue;
                if (data.ShotDatas[i].ShotType == ShotType.Normal && data.BulletDatas[i].BulletType == BulletType.Funnel) continue;
                var cnt = data.ShotDatas[i].Count;
                if (cnt > 1) SetEvent(GetUnityAction(attackerType, data.ShotDatas[i], data.BulletDatas[i], aim, target), data.ShotDatas[i]);
                else GetUnityAction(attackerType, data.ShotDatas[i], data.BulletDatas[i], aim, target)(0.0f);
            }
        }

        /// <summary>
        /// ShotDataをもとにActionを生成するメソッド
        /// </summary>
        /// <param name="shotData">発射するShotData</param>
        /// <returns></returns>
        private UnityAction<float> GetUnityAction(AttackerType attackerType, ShotData shotData, BulletData bulletData, GameObject aim, GameObject target)
        {
            // var position = shooter.transform.position; //発射位置を固定したい場合はキャッシュしておく ※固定するとFunnelから出る弾の発射位置も移動しなくなるので注意
            var velocity = (aim == default) ? default : (aim.transform.position - transform.position).normalized;
            Vector3[] directions = ShotManager.Instance.GetShotShape(shotData.AllRangeShotShape, shotData.GetBulletNum, RandomNum);
            switch (shotData.ShotType)
            {
                case ShotType.Normal:
                    return (t) => ShotManager.Instance.NormalShot.Shot(shotData.GetBulletNum, attackerType, gameObject, bulletData.BulletType,
                        bulletData, aim, target, shotData.RandomDiffusion, shotData.BulletShake);

                case ShotType.AllRange:
                    if (velocity != default && shotData.AllRangeShotShape == AllRangeShotShape.Plane)
                    {
                        for (int i = 0; i < shotData.BulletNum; i++) directions[i].TransformDirection(velocity);
                    }
                    return (t) => ShotManager.Instance.AllRangeShot.Shot(shotData.GetBulletNum, attackerType, gameObject, directions, bulletData, target, t,
                        shotData.AngularSpeed, shotData.Axis);

                case ShotType.Gather:
                    var gatherAtTarget = target != default && shotData.GatherAtTarget;
                    if (velocity != default && shotData.AllRangeShotShape == AllRangeShotShape.Plane)
                    {
                        for (int i = 0; i < shotData.BulletNum; i++) directions[i] = directions[i].TransformDirection(velocity);
                    }
                    return (t) => ShotManager.Instance.GatherShot.Shot(shotData.GetBulletNum, attackerType, gatherAtTarget ? target : gameObject, directions,
                        bulletData, target, t, shotData.AngularSpeed, shotData.Axis, shotData.PassGatherPoint, shotData.DistanceFromGatherPoint);
                default:
                    return default;
            }
        }
    }
}
