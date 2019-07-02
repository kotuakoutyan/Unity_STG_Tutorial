using UnityEngine;
using System.Collections.Generic;
using Barrage.Shot;
using Barrage.Bullet;
using System.Linq;

namespace Barrage.ObjectPool
{
    public class ObjectPools
    {
        private ObjectPools() { }
        private static ObjectPools instance;
        public static ObjectPools Instance
        {
            get
            {
                if (instance == null) instance = new ObjectPools();
                return instance;
            }
        }
            
        private ObjectPool<NormalBullet> NormalBulletPool = new ObjectPool<NormalBullet>();
        private ObjectPool<SplitBullet> SplitBulletPool = new ObjectPool<SplitBullet>();
        private ObjectPool<ChaseBullet> ChaseBulletPool = new ObjectPool<ChaseBullet>();
        private ObjectPool<FunnelBullet> FunnelBulletPool = new ObjectPool<FunnelBullet>();

        public BaseBullet[] GetBullet(int num,  BulletType bulletType)
        {
            switch (bulletType)
            {
                case BulletType.Normal:
                case BulletType.Curve:
                    return NormalBulletPool.GetBullet(num);
                case BulletType.Split:
                    return SplitBulletPool.GetBullet(num);
                case BulletType.Chase:
                    return ChaseBulletPool.GetBullet(num);
                case BulletType.Funnel:
                    return FunnelBulletPool.GetBullet(num);
                default:
                    return null;
            }
        }

        public void ResetPools()
        {
            NormalBulletPool.ResetPool();
            SplitBulletPool.ResetPool();
            ChaseBulletPool.ResetPool();
            FunnelBulletPool.ResetPool();
        }
    }

    /// <typeparam name="T">抽象クラスBulletを継承したクラス</typeparam>
    public class ObjectPool<T> where T : BaseBullet
    {
        private List<T> BulletList = new List<T>();
        [SerializeField] private GameObject Bullet;

        public ObjectPool()
        {
            Bullet = Resources.Load<GameObject>("Sphere");
        }

        public void CreatePool(GameObject bullet, int maxCount)
        {
            for (int i = 0; i < maxCount; i++)
            {
                var newObj = CreateNewObject();
                newObj.gameObject.SetActive(false);
                BulletList.Add(newObj);
            }
        }

        public void ResetPool()
        {
            BulletList = new List<T>();
        }

        /// <summary>
        /// 使用中でないBulletを探してListで返す
        /// </summary>
        /// <param name="requestNum">必要なBulletの数</param>
        public BaseBullet[] GetBullet(int requestNum = 1)
        {
            if (requestNum < 1) return null;

            var array = new BaseBullet[requestNum];
            var supplyNum = 0;
            var bulletNum = BulletList.Count;
            for (int i = 0;i < bulletNum; i++)
            {
                var bullet = BulletList[i];
                if (bullet.gameObject.activeSelf == false)
                {
                    array[supplyNum] = bullet;
                    supplyNum++;
                    if (supplyNum == requestNum) return array;
                }
            }

            // 全て使用中だったら新しく作って返す
            while (supplyNum < requestNum)
            {
                var newObj = CreateNewObject();
                BulletList.Add(newObj);
                array[supplyNum] = newObj;
                supplyNum++;
            }
            return array;
        }

        /// <summary>
        /// Bulletを作り、それを返すメソッド
        /// </summary>
        private T CreateNewObject()
        {
            var newObj = Object.Instantiate(Bullet).AddComponent<T>();
            newObj.name = Bullet.name + (BulletList.Count + 1);

            return newObj;
        }
    }
}
